using System.Collections;
using UnityEngine;

public class EnemyDemonKin : Enemy
{
    [SerializeField]
    private GameObject attackRangeLeft;
    [SerializeField]
    private GameObject attackRangeRight;
    protected override void Start()
    {
        base.Start();
        attackDistance = 1.5f;
        chaseDistance = 7f;
    }

    protected override IEnumerator AttackCoroutine()
    {
        if (target.position.x > transform.position.x)
        {
            spriteRenderer.flipX = false;
        }
        else if (target.position.x < transform.position.x)
        {
            spriteRenderer.flipX = true;
        }
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(attackSpeed);

        float distance = Vector2.Distance(target.position, transform.position);
        if (distance < attackDistance)
        {
            ChangeState(EnemyState.Attack);
        }
        else
        {
            ChangeState(EnemyState.Patrol);
        }
    }

    public void OnAttackRange()
    {
        if (spriteRenderer.flipX)
        {
            attackRangeLeft.SetActive(true);
        }
        else
        {
            attackRangeRight.SetActive(true);
        }
    }

    public void OffAttackRange()
    {
        attackRangeRight.SetActive(false);
        attackRangeLeft.SetActive(false);
    }
}
