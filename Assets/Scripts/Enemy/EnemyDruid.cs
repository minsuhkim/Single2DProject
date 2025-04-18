using System.Collections;
using UnityEngine;

public class EnemyDruid : Enemy
{
    [Header("Projectile")]
    public GameObject projectilePrefab;

    private Vector3 rangeAttackPos;

    protected override void Start()
    {
        base.Start();
        attackDistance = 7.5f;
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

    public void SetRangeAttackPosition()
    {
        rangeAttackPos = target.GetComponentsInChildren<Transform>()[1].position;
    }

    public void RangeAttack()
    {
        Instantiate(projectilePrefab, rangeAttackPos, Quaternion.identity);
    }
}
