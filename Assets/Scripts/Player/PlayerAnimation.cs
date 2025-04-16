using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void TriggerAttack()
    {
        animator.SetTrigger("Attack");
    }

    public void TriggerAttack2()
    {
        animator.SetTrigger("Attack2");
    }


    public void TriggerParring()
    {
        animator.SetTrigger("Parring");
    }
    public void TriggerJump()
    {
        animator.SetTrigger("Jump");
    }

    public void TriggerDash()
    {
        animator.SetTrigger("Dash");
    }

    public void TriggerSlide()
    {
        animator.SetTrigger("Slide");
    }

    public void TriggerHurt()
    {
        animator.SetTrigger("Hurt");
    }

    public void TriggerTeleport()
    {
        animator.SetTrigger("Teleport");
    }

    public void SetIsGrounded(bool isGrounded)
    {
        animator.SetBool("isGround", isGrounded);
    }

    public void SetFall(float velocityY)
    {
        animator.SetFloat("velocityY", velocityY);
    }

    public void SetWalking(bool isMove)
    {
        animator.SetBool("isMove", isMove);
    }
}
