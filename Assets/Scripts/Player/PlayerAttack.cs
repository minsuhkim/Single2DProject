using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private PlayerAnimation playerAnimation;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    public bool isAttack = false;
    public bool isParrying = false;

    [Header("공격 애니메이션 상태 이름")]
    public string attackStateName = "Attack";

    public string parringStateName = "Parring";

    public GameObject attackRangeLeft;
    public GameObject attackRangeRight;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
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

    public void OnAttackCollider()
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

    public void OffAttackCollider()
    {
        attackRangeLeft.SetActive(false);
        attackRangeRight.SetActive(false);
    }

    private IEnumerator AttackCooldownByAnimation()
    {
        isAttack = true;

        //안정성을 위함(다음 프레임까지 대기)
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

        //안정성을 위함(다음 프레임까지 대기)
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
