using System.Collections;
using UnityEngine;

public class EnemyElf : Enemy
{
    [SerializeField]
    private GameObject projectilePrefab;

    [SerializeField]
    private Transform firePosition;

    protected override void Start()
    {
        base.Start();
        attackDistance = 5f;
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

    public void FireArrow()
    {
        SoundManager.Instance.PlaySFX(SFXType.ElfAttack);
        ElfProjectile arrow = Instantiate(projectilePrefab, firePosition.position, Quaternion.identity).GetComponent<ElfProjectile>();
        Vector3 direction = (target.position - transform.position);
        direction.y = 0;
        arrow.SetArrow(direction.normalized, damage);
    }
}
