using System.Collections;
using UnityEngine;

public enum EnemyType
{
    None, Bringer, ArchDemon, Elf, Druid, DemonKin, Mage, Knight
}

public enum EnemyState
{
    Patrol,
    Chase,
    Attack,
    Idle,
    Dead,
}

public class Enemy : MonoBehaviour
{
    public EnemyState currentState = EnemyState.Patrol;
    [Header("Type")]
    public EnemyType type;

    [Header("Move")]
    public float maxDistance = 3.0f;
    protected Vector3 startPos;
    protected int direction = 1;

    protected Color originColor;
    protected Renderer objectRenderer;
    public float colorChangeDuration = .05f;

    [Header("attack effect transform")]
    public Transform[] particleTransform;
    [Header("hit camera shake")]
    public float duration = 0.2f;
    public float magnitude = 0.03f;

    [Header("Chase")]
    [SerializeField]
    protected Transform target;
    [SerializeField]
    protected float chaseDistance = 5f;

    [Header("Attack")]
    public bool isAttack = false;
    [SerializeField]
    protected float attackDistance = 2f;
    //public GameObject attackRangeLeft;
    //public GameObject attackRangeRight;

    [Header("Dead")]
    protected float deadTime = 1f;
    public bool isLive = true;

    [Header("Wait")]
    public bool isWait = false;

    [Header("Stats")]
    protected int maxHP;
    protected int curHP;
    protected int damage;
    protected float moveSpeed;
    protected float attackSpeed;
    [SerializeField]
    protected EnemyData statData;

    protected Animator animator;
    protected SpriteRenderer spriteRenderer;

    protected Coroutine curCoroutine;

    [Header("BossSpawn")]
    public BossSpawner bossSpawner;

    protected virtual void Start()
    {
        if ((type != EnemyType.None))
        {
            animator = GetComponent<Animator>();
        }
        objectRenderer = GetComponent<Renderer>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        originColor = objectRenderer.material.color;

        startPos = transform.position;

        target = PlayerController.Instance.transform;

        maxHP = statData.maxHP;
        curHP = maxHP;

        if (type != EnemyType.None)
        {
            damage = statData.damage;
            moveSpeed = statData.moveSpeed;
            attackSpeed = statData.attackSpeed;
            ChangeState(EnemyState.Patrol);
        }
    }

    protected virtual void ChangeState(EnemyState state)
    {
        if (!isLive)
        {
            return;
        }

        if(type != EnemyType.None)
        {
            currentState = state;

            if (curCoroutine != null)
            {
                StopCoroutine(curCoroutine);
            }

            switch (state)
            {
                case EnemyState.Attack:
                    curCoroutine = StartCoroutine(AttackCoroutine());
                    break;
                case EnemyState.Patrol:
                    curCoroutine = StartCoroutine(PatrolCoroutine());
                    break;
                case EnemyState.Idle:
                    curCoroutine = StartCoroutine(IdleCoroutine());
                    break;
                case EnemyState.Dead:
                    curCoroutine = StartCoroutine(DeadCoroutine());
                    break;
            }
        }
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerAttack")
        {
            PlayerController.Instance.curMP += 2f;
            if(PlayerController.Instance.curMP > PlayerController.Instance.stats.maxMP)
            {
                PlayerController.Instance.curMP = PlayerController.Instance.stats.maxMP;
            }
            UIManager.Instance.SetMPImage(PlayerController.Instance.curMP);
            int damage = collision.GetComponentInParent<PlayerStats>().attackDamage;
            TakeDamage(damage);
        }
        else if (collision.tag == "PlayerAttack2")
        {
            PlayerController.Instance.curMP += 3f;
            if (PlayerController.Instance.curMP > PlayerController.Instance.stats.maxMP)
            {
                PlayerController.Instance.curMP = PlayerController.Instance.stats.maxMP;
            }
            UIManager.Instance.SetMPImage(PlayerController.Instance.curMP);
            int damage = collision.GetComponentInParent<PlayerStats>().attack2Damage;
            TakeDamage(damage);
        }
    }

    public void TakeDamage(int damage)
    {
        SoundManager.Instance.PlaySFX(SFXType.EnemyHit);
        curHP -= damage;
        if (curHP <= 0)
        {
            if (bossSpawner != null)
            {
                bossSpawner.KillEnemy();
            }
            ChangeState(EnemyState.Dead);
        }
        else
        {
            StartCoroutine(HitCoroutine());
            if (particleTransform.Length > 0)
            {
                int randIndex = Random.Range(0, particleTransform.Length);
                ParticleManager.Instance.ParticlePlay(ParticleType.PlayerAttack, particleTransform[randIndex].position, Vector3.one);
            }
        }
    }

    protected virtual IEnumerator PatrolCoroutine()
    {
        animator.SetBool("isMove", true);

        while (currentState == EnemyState.Patrol)
        {
            if (transform.position.x > startPos.x + maxDistance)
            {
                animator.SetBool("isMove", false);
                yield return new WaitForSeconds(0.5f);
                animator.SetBool("isMove", true);
                direction = -1;

            }
            else if (transform.position.x < startPos.x - maxDistance)
            {
                animator.SetBool("isMove", false);
                yield return new WaitForSeconds(0.5f);
                animator.SetBool("isMove", true);
                direction = 1;
            }
            spriteRenderer.flipX = direction == -1 ? true : false;
            transform.position += new Vector3(moveSpeed * direction * Time.deltaTime, 0, 0);

            float distance = Vector2.Distance(target.transform.position, transform.position);
            if (distance < attackDistance)
            {
                ChangeState(EnemyState.Attack);
            }
            yield return null;
        }
    }

    //protected virtual IEnumerator ChaseCoroutine()
    //{
    //    if (curCoroutine != null)
    //    {
    //        StopCoroutine(curCoroutine);
    //    }

    //    yield return null;
    //}

    protected virtual IEnumerator DeadCoroutine()
    {
        isLive = false;
        CameraManager.Instance.StartCameraShake(duration * 1.5f, magnitude * 1.5f);
        animator.SetTrigger("Dead");
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(stateInfo.length);
        //Destroy(gameObject);
        gameObject.SetActive(false);
    }

    protected virtual IEnumerator HitCoroutine()
    {
        CameraManager.Instance.StartCameraShake(duration, magnitude);
        objectRenderer.material.color = Color.red;
        yield return new WaitForSeconds(colorChangeDuration);
        objectRenderer.material.color = originColor;
    }

    protected virtual IEnumerator AttackCoroutine()
    {
        yield return null;
    }

    protected virtual IEnumerator IdleCoroutine()
    {
        isWait = true;
        yield return new WaitForSeconds(1f);
        isWait = false;

        float distance = Vector2.Distance(target.transform.position, transform.position);
        if (distance < attackDistance)
        {
            //ChangeState(EnemyState.Attack);
            ChangeState(EnemyState.Attack);
        }
        else
        {
            //ChangeState(EnemyState.Patrol);
            ChangeState(EnemyState.Patrol);
        }
    }

    //public void ChangeState(EnemyState bossState)
    //{
    //    if (type == EnemyType.None || isWait)
    //    {
    //        return;
    //    }
    //    this.currentState = bossState;
    //    Debug.Log(currentState);
    //}


    //private void Idle()
    //{
    //    animator.SetBool("isWalk", false);
    //    StartCoroutine(IdleCoroutine());
    //}

    //private void Chase()
    //{
    //    if (isAttack)
    //    {
    //        return;
    //    }

    //    animator.SetBool("isWalk", true);
    //    direction = (target.position.x - transform.position.x) > 0 ? 1 : -1;
    //    spriteRenderer.flipX = direction == 1 ? true : false;
    //    transform.position += new Vector3(direction * speed * Time.deltaTime, 0, 0);
    //}

    //private void Patrol()
    //{
    //    if (isAttack)
    //    {
    //        return;
    //    }

    //    animator.SetBool("isWalk", true);

    //    if (type != EnemyType.None)
    //    {
    //        if (transform.position.x > startPos.x + maxDistance)
    //        {
    //            direction = -1;

    //        }
    //        else if (transform.position.x < startPos.x - maxDistance)
    //        {
    //            direction = 1;
    //        }
    //        spriteRenderer.flipX = direction == 1 ? true : false;
    //        transform.position += new Vector3(speed * direction * Time.deltaTime, 0, 0);
    //    }

    //}

    //private void Attack()
    //{
    //    if (isAttack)
    //    {
    //        return;
    //    }

    //    direction = (target.position.x - transform.position.x) > 0 ? 1 : -1;
    //    spriteRenderer.flipX = direction == 1 ? true : false;

    //    if (animator != null)
    //    {
    //        animator.SetTrigger("Attack");
    //    }

    //    StartCoroutine(AttackCoroutine());
    //}

    //public void OnAttackRange()
    //{
    //    if (spriteRenderer.flipX)
    //    {
    //        attackRangeRight.SetActive(true);
    //    }
    //    else
    //    {
    //        attackRangeLeft.SetActive(true);
    //    }
    //}

    //public void OffAttackRange()
    //{
    //    attackRangeLeft.SetActive(false);
    //    attackRangeRight.SetActive(false);
    //}
}
