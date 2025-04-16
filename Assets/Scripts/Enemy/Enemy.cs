using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

 public enum EnemyType
{
    None, Bringer, ArchDemon, Elf, Druid, DemonKin,
}

public enum EnemyState
{
    Patrol,
    Chase,
    Attack,
    Idle,
    Stun
}

public class Enemy : MonoBehaviour
{
    public EnemyState currentState = EnemyState.Patrol;
    [Header("Type")]
    public EnemyType type;

    [Header("Move")]
    public float speed = 2.0f;
    public float maxDistance = 3.0f;
    private Vector3 startPos;
    private int direction = 1;

    private float enemyHP = 100;

    private Color originColor;
    private Renderer objectRenderer;
    public float colorChangeDuration = .05f;

    [Header("attack effect transform")]
    public Transform[] particleTransform;
    [Header("hit camera shake")]
    public float duration = 0.2f;
    public float magnitude = 0.03f;

    [Header("Chase")]
    public Transform target;

    [Header("Attack")]
    public bool isAttack = false;
    public GameObject attackRangeLeft;
    public GameObject attackRangeRight;

    [Header("Parrying")]
    public GameObject parryingRangeLeft;
    public GameObject parryingRangeRight;

    [Header("Wait")]
    public bool isWait = false;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        if (!(type == EnemyType.None))
        {
            animator = GetComponent<Animator>();
        }
        objectRenderer = GetComponent<Renderer>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        originColor = objectRenderer.material.color;

        startPos = transform.position;

        target = PlayerController.Instance.transform;
    }

    void Update()
    {
        if(type == EnemyType.None)
        {
            return;
        }
        switch (currentState)
        {
            case EnemyState.Patrol:
                Patrol();
                break;
            case EnemyState.Chase:
                Chase();
                break;
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Stun:
                Stun();
                break;
        }
    }


    public void ChangeState(EnemyState state)
    {
        if (type == EnemyType.None || isWait)
        {
            return;
        }
        this.currentState = state;
        Debug.Log(currentState);
    }

    private void Stun()
    {
        //StopAllCoroutines();
        OffAttackRange();
        animator.SetBool("isWalk", false);
        animator.SetTrigger("Stun");
        StartCoroutine(StunCoroutine());
    }

    private void Idle()
    {
        animator.SetBool("isWalk", false);
        StartCoroutine(IdleCoroutine());
    }

    private void Chase()
    {
        if (isAttack)
        {
            return;
        }

        animator.SetBool("isWalk", true);
        direction = (target.position.x - transform.position.x) > 0 ? 1 : -1;
        spriteRenderer.flipX = direction == 1 ? true : false;
        transform.position += new Vector3(direction * speed * Time.deltaTime, 0, 0);
    }

    private void Patrol()
    {
        if (isAttack)
        {
            return;
        }

        animator.SetBool("isWalk", true);

        if (type != EnemyType.None)
        {
            if (transform.position.x > startPos.x + maxDistance)
            {
                direction = -1;

            }
            else if (transform.position.x < startPos.x - maxDistance)
            {
                direction = 1;
            }
            spriteRenderer.flipX = direction == 1 ? true : false;
            transform.position += new Vector3(speed * direction * Time.deltaTime, 0, 0);
        }

    }

    private void Attack()
    {
        if (isAttack)
        {
            return;
        }

        direction = (target.position.x - transform.position.x) > 0 ? 1 : -1;
        spriteRenderer.flipX = direction == 1 ? true : false;

        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        StartCoroutine(AttackCoroutine());
    }

    public void OnAttackRange()
    {
        if (spriteRenderer.flipX)
        {
            attackRangeRight.SetActive(true);
        }
        else
        {
            attackRangeLeft.SetActive(true);
        }
    }

    public void OffAttackRange()
    {
        attackRangeLeft.SetActive(false);
        attackRangeRight.SetActive(false);
    }

    public void OnParryingRange()
    {
        if (spriteRenderer.flipX)
        {
            parryingRangeRight.SetActive(true);
        }
        else
        {
            parryingRangeLeft.SetActive(true);
        }
    }

    public void OffParryingRange()
    {
        parryingRangeRight.SetActive(false);
        parryingRangeLeft.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "PlayerAttack")
        {
            Debug.Log("플레이어한테 공격 당함");
            StartCoroutine(HitCoroutine());
            if(particleTransform.Length > 0)
            {
                int randIndex = Random.Range(0, particleTransform.Length);
                ParticleManager.Instance.ParticlePlay(ParticleType.PlayerAttack, particleTransform[randIndex].position, Vector3.one);
            }
        }
    }

    private IEnumerator HitCoroutine()
    {
        Debug.Log("공격 성공");
        SoundManager.Instance.PlaySFX(SFXType.Hit);
        CameraManager.Instance.StartCameraShake(duration, magnitude);
        objectRenderer.material.color = Color.red;
        yield return new WaitForSeconds(colorChangeDuration);
        objectRenderer.material.color = originColor;
    }

    private IEnumerator AttackCoroutine()
    {
        isAttack = true;
        yield return null;

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("Attack"))
        {
            float animationLength = stateInfo.length;
            yield return new WaitForSeconds(animationLength);
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
        }
        ChangeState(EnemyState.Idle);
        isAttack = false;
    }

    private IEnumerator IdleCoroutine()
    {
        isWait = true;
        yield return new WaitForSeconds(1f);
        isWait = false;

        float distance = Mathf.Abs(target.position.x - transform.position.x);
        if(distance < 2)
        {
            ChangeState(EnemyState.Attack);
        }
        else if(distance < 5)
        {
            ChangeState(EnemyState.Chase);
        }
        else
        {
            ChangeState(EnemyState.Patrol);
        }
    }

    private IEnumerator StunCoroutine()
    {
        isWait = true;
        yield return new WaitForSeconds(1.5f);
        isWait = false;

        float distance = Mathf.Abs(target.position.x - transform.position.x);
        if (distance < 2)
        {
            ChangeState(EnemyState.Attack);
        }
        else if (distance < 5)
        {
            ChangeState(EnemyState.Chase);
        }
        else
        {
            ChangeState(EnemyState.Patrol);
        }
    }
}
