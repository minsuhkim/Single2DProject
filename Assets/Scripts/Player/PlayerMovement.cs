using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float walkSpeed = 5.0f;
    public float jumpForce = 10.0f;

    private Rigidbody2D rb;
    private float moveInput;

    [Header("GroundCheck")]
    public bool isGrounded;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private PlayerAnimation playerAnimation;
    private PlayerAttack playerAttack;
    private SpriteRenderer spriteRenderer;

    [Header("Dash")]
    public float dashSpeed = 10f;
    public bool isDash = false;
    public string dashStateName = "Dash";

    [Header("Slide")]
    public float slideSpeed = 10f;
    public bool isSlide = false;
    public string slideStateName = "Slide";

    private Animator animator;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerAnimation = GetComponent<PlayerAnimation>();
        playerAttack = GetComponent<PlayerAttack>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void HandleMovement()
    {
        if(!isDash && !isSlide && !PlayerController.Instance.isKnockback)
        {
            moveInput = Input.GetAxisRaw("Horizontal");
        }

        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        if (playerAttack.isAttack || playerAttack.isParrying)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocityY);
        }

        if (rb.linearVelocityX > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if(rb.linearVelocityX < 0)
        {
            spriteRenderer.flipX = true;
        }

        if (playerAnimation != null)
        {
            playerAnimation.SetWalking(moveInput != 0);
        }

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        // 점프 후 착지 했는지 체크
        playerAnimation.SetIsGrounded(isGrounded);
        // 점프 후 떨어지는 애니메이션
        playerAnimation.SetFall(rb.linearVelocityY);

        Jump();
        //Dash();
        Slide();
    }


    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.X) && isGrounded)
        {
            SoundManager.Instance.PlaySFX(SFXType.Jump);
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            playerAnimation.TriggerJump();
            playerAttack.isAttack = false;
            playerAttack.isParrying = false;
        }
    }
    //private void Dash()
    //{
    //    if (!isGrounded || isDash || isSlide || (moveInput == 0))
    //    {
    //        return;
    //    }
    //    if (Input.GetKeyDown(KeyCode.C))
    //    {
    //        SoundManager.Instance.PlaySFX(SFXType.Dash);
    //        isDash = true;
    //        playerAttack.isAttack = false;
    //        playerAttack.isParrying = false;
    //        moveSpeed = dashSpeed;
    //        playerAnimation.TriggerDash();

    //        StartCoroutine(DashCooldownByAnimation());
    //    }
    //}

    //private IEnumerator DashCooldownByAnimation()
    //{
    //    AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
    //    if (stateInfo.IsName(dashStateName))
    //    {
    //        float animationLength = stateInfo.length;
    //        yield return new WaitForSeconds(animationLength);
    //    }
    //    else
    //    {
    //        yield return new WaitForSeconds(0.5f);
    //    }
    //    moveSpeed = walkSpeed;
    //    isDash = false;
    //}

    private void Slide()
    {
        if (!isGrounded || isDash || isSlide || (moveInput == 0))
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            SoundManager.Instance.PlaySFX(SFXType.Slide);
            isSlide = true;
            playerAttack.isAttack = false;
            playerAttack.isParrying = false;
            moveSpeed = slideSpeed;
            playerAnimation.TriggerSlide();
            StartCoroutine(SlideCooldownByAnimation());
        }
    }

    private IEnumerator SlideCooldownByAnimation()
    {
        PlayerController.Instance.isInvincible = true;
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName(slideStateName))
        {
            float animationLength = stateInfo.length;
            yield return new WaitForSeconds(animationLength);
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
        }
        moveSpeed = walkSpeed;
        PlayerController.Instance.isInvincible = false;
        isSlide = false;
    }

    public void PlayRunSFX()
    {
        SoundManager.Instance.PlaySFX(SFXType.Walk);
    }
}
