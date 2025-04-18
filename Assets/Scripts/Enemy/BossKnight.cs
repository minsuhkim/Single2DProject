using System.Collections;
using UnityEngine;

public class BossKnight : Boss
{
    [SerializeField]
    private float attack3Delay = 3f;

    public GameObject attackRangeLeft;
    public GameObject attackRangeRight;

    public GameObject attack2RangeLeft;
    public GameObject attack2RangeRight;

    public GameObject attack3RangeLeft;
    public GameObject attack3RangeRight;


    protected override IEnumerator Think()
    {
        yield return new WaitForSeconds(patternChangeTime);

        int pattern = Random.Range(0, 3);
        switch (pattern)
        {
            case 0:
                StartCoroutine(Attack());
                break;
            case 1:
                StartCoroutine(Attack2());
                break;
            case 2:
                StartCoroutine(Attack3());
                break;
        }
    }


    private IEnumerator Attack3()
    {
        LookAtPlayer();
        animator.SetTrigger("Attack3");
        yield return new WaitForSeconds(attack3Delay);
        StartCoroutine(Think());
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
        // ���� �ر�
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

    public void OnAttack2Range()
    {
        if (spriteRenderer.flipX)
        {
            attack2RangeLeft.SetActive(true);
        }
        else
        {
            attack2RangeRight.SetActive(true);
        }
    }

    public void OnAttack3Range()
    {
        if (spriteRenderer.flipX)
        {
            attack3RangeLeft.SetActive(true);
        }
        else
        {
            attack3RangeRight.SetActive(true);
        }
    }

    public void OffAttackRange()
    {
        attackRangeLeft.SetActive(false);
        attackRangeRight.SetActive(false);
        attack2RangeLeft.SetActive(false);
        attack2RangeRight.SetActive(false);
        attack3RangeLeft.SetActive(false);
        attack3RangeRight.SetActive(false);
    }
}
