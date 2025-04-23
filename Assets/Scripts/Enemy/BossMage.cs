using System.Collections;
using UnityEngine;

public class BossMage : Boss
{
    public GameObject projectilePrefab;

    private Vector3 rangeAttackPos;

    protected override IEnumerator Think()
    {
        yield return new WaitForSeconds(patternChangeTime);

        if (bossState == BossState.Idle)
        {
            StartCoroutine(Think());
        }

        else if (isLive)
        {
            int pattern = Random.Range(0, 2);
            switch (pattern)
            {
                case 0:
                    StartCoroutine(TeleportToAttack());
                    break;
                case 1:
                    StartCoroutine(Attack2());
                    break;
            }
        }
        
    }

    private IEnumerator TeleportToAttack()
    {
        animator.SetTrigger("Teleport");
        yield return new WaitForSeconds(0.5f);
        Teleport();
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(Attack());
    }

    public void Teleport()
    {
        transform.position = target.position;
    }

    public void SetTargetPos()
    {
        rangeAttackPos = target.position;
    }

    public void RangeAttack()
    {
        Instantiate(projectilePrefab, rangeAttackPos, Quaternion.identity);
    }

    protected override IEnumerator DeadCoroutine()
    {
        isLive = false;
        CameraManager.Instance.StartCameraShake(duration * 1.5f, magnitude * 1.5f);
        animator.SetTrigger("Dead");
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(stateInfo.length);
        GetComponent<CapsuleCollider2D>().enabled = false;
        StopAllCoroutines();
        // 게임클리어
        GameManager.Instance.GameClear();
    }
}
