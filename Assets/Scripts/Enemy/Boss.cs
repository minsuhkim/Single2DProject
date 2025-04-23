using System.Collections;
using UnityEngine;

public enum BossState
{
    Idle, Battle
}

public class Boss : Enemy
{
    [SerializeField]
    protected float patternChangeTime = 0.2f;
    [SerializeField]
    protected float attackDelay = 2f;
    [SerializeField]
    protected float attack2Delay = 2f;

    public BossState bossState;

    protected override void Start()
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

        bossState = BossState.Idle;

        if (type != EnemyType.None)
        {
            maxHP = statData.maxHP;
            curHP = maxHP;
            damage = statData.damage;
            moveSpeed = statData.moveSpeed;
            attackSpeed = statData.attackSpeed;
            StartCoroutine(Think());
        }

    }


    protected virtual IEnumerator Think()
    {
        yield return new WaitForSeconds(patternChangeTime);

        if(bossState == BossState.Idle)
        {
            StartCoroutine(Think());
        }

        else
        {
            int pattern = Random.Range(0, 2);
            switch (pattern)
            {
                case 0:
                    StartCoroutine(Attack());
                    break;
                case 1:
                    StartCoroutine(Attack2());
                    break;
            }
        }
    }

    protected virtual IEnumerator Attack()
    {
        LookAtPlayer();
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(attackDelay);
        StartCoroutine(Think());
    }

    protected virtual IEnumerator Attack2()
    {
        LookAtPlayer();
        animator.SetTrigger("Attack2");
        yield return new WaitForSeconds(attack2Delay);
        StartCoroutine(Think());
    }

    protected void LookAtPlayer()
    {
        if(target.position.x < transform.position.x)
        {
            spriteRenderer.flipX = true;
        }
        else if(target.position.x > transform.position.x)
        {
            spriteRenderer.flipX = false;
        }
    }
}
