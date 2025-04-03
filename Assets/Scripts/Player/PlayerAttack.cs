using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private PlayerAnimation playerAnimation;
    private Animator animator;

    public bool isAttack = false;
    public bool isParrying = false;

    [Header("���� �ִϸ��̼� ���� �̸�")]
    public string attackStateName = "Attack";

    public string parringStateName = "Parring";

    private void Start()
    {
        playerAnimation = GetComponent<PlayerAnimation>();
        animator = GetComponent<Animator>();
    }

    public void PerformAttack()
    {
        if (isAttack)
        {
            return;
        }

        if (playerAnimation != null)
        {
            playerAnimation.TriggerAttack();
            SoundManager.Instance.PlaySFX(SFXType.Attack);
        }

        StartCoroutine(AttackCooldownByAnimation());
    }

    private IEnumerator AttackCooldownByAnimation()
    {
        isAttack = true;

        //�������� ����(���� �����ӱ��� ���)
        yield return null;
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName(attackStateName))
        {
            float animationLength = stateInfo.length;
            yield return new WaitForSeconds(animationLength);
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
        }
        isAttack = false;
    }

    public void PerformParrying()
    {
        if (isParrying)
        {
            return;
        }

        if (playerAnimation != null)
        {
            playerAnimation.TriggerParring();
            SoundManager.Instance.PlaySFX(SFXType.Attack);
        }

        StartCoroutine(ParryingCooldownByAnimation());
    }

    private IEnumerator ParryingCooldownByAnimation()
    {
        isParrying = true;

        //�������� ����(���� �����ӱ��� ���)
        yield return null;
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName(parringStateName))
        {
            float animationLength = stateInfo.length;
            yield return new WaitForSeconds(animationLength);
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
        }
        isParrying = false;
    }

}
