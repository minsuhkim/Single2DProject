using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMage : Boss
{
    public GameObject projectilePrefab;

    private Vector3 rangeAttackPos;

    public Dialog mageDialog;
    public DialogueInfo mageDialogueInfo;

    protected override IEnumerator Think()
    {
        yield return new WaitForSeconds(patternChangeTime);

        if (bossState == BossState.Idle)
        {
            curCoroutine = StartCoroutine(Think());
        }
        else if (isLive)
        {
            int pattern = Random.Range(0, 2);
            switch (pattern)
            {
                case 0:
                    curCoroutine = StartCoroutine(TeleportToAttack());
                    break;
                case 1:
                    curCoroutine = StartCoroutine(Attack2());
                    break;
            }
        }
        
    }

    public void OnAttackSound()
    {
        SoundManager.Instance.PlaySFX(SFXType.MageAttack2);
    }

    private IEnumerator TeleportToAttack()
    {
        SoundManager.Instance.PlaySFX(SFXType.MageTeleport);
        animator.SetTrigger("Teleport");
        yield return new WaitForSeconds(0.5f);
        Teleport();
        yield return new WaitForSeconds(0.5f);
        curCoroutine = StartCoroutine(Attack());
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

    private void SetDialog()
    {
        mageDialog.lines = new List<(int, string)>();
        for (int i = 0; i < mageDialogueInfo.ids.Count; i++)
        {
            mageDialog.lines.Add((mageDialogueInfo.ids[i], mageDialogueInfo.lines[i]));
        }
    }

    protected override IEnumerator DeadCoroutine()
    {
        StopCoroutine(curCoroutine);
        isLive = false;
        CameraManager.Instance.StartCameraShake(duration * 2f, magnitude * 2f);
        animator.SetTrigger("Dead");
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(stateInfo.length);
        GetComponent<CapsuleCollider2D>().enabled = false;

        SetDialog();
        yield return StartCoroutine(DialogueManager.Instance.ShowDialog(mageDialog));
        // 게임클리어
        GameManager.Instance.GameClear();
    }
}
