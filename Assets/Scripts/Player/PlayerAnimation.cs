using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void TriggerAttack()
    {
        animator.SetTrigger("Attack");
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
