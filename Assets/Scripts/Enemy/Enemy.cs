using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

 public enum EnemyType
{
    None, Bringer
}

public enum EnemyState
{
    Patrol,
    Chase,
    Attack,
    Idle,
}

public class Enemy : MonoBehaviour
{
    public EnemyState state = EnemyState.Patrol;
    [Header("Type")]
    public EnemyType type;

    [Header("Move")]
    public float speed = 2.0f;
    public float maxDistance = 3.0f;
    private Vector3 startPos;
    private int direction = 1;

    private float enemyHP = 100;

    private Color originColor;
    private Renderer objecteRenderer;
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

    [Header("Wait")]
    public bool isWait = false;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        animator = GetComponent<Animator>();
        objecteRenderer = GetComponent<Renderer>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        originColor = objecteRenderer.material.color;

        startPos = transform.position;

        target = PlayerController.Instance.transform;
    }

    void Update()
    {
        switch (state)
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
        }
    }

    public void ChangeState(EnemyState state)
    {
        if (isWait)
        {
            return;
        }
        this.state = state;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "PlayerAttack")
        {
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
        // 소리를 아직 못구함 > 임시로 패링 소리 삽입
        SoundManager.Instance.PlaySFX(SFXType.Parrying);
        //StartCoroutine(CameraShake(duration, magnitude));
        CameraManager.Instance.StartCameraShake(duration, magnitude);
        //GenerateCameraImpulse();
        objecteRenderer.material.color = Color.red;
        yield return new WaitForSeconds(colorChangeDuration);
        objecteRenderer.material.color = originColor;
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
    }
}
