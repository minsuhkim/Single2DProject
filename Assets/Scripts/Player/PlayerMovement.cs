using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float walkSpeed = 5.0f;
    public float jumpForce = 10.0f;

    private Rigidbody2D rb;
    public float moveInputX;
    private float moveInputY;

    [Header("GroundCheck")]
    public bool isGrounded;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private PlayerAnimation playerAnimation;
    private PlayerAttack playerAttack;
    private SpriteRenderer spriteRenderer;

    //[Header("Dash")]
    //public float dashSpeed = 10f;
    //public bool isDash = false;
    //public string dashStateName = "Dash";

    [Header("Slide")]
    public float slideSpeed = 10f;
    public bool isSlide = false;
    public string slideStateName = "Slide";

    [Header("Teleport")]
    public bool isTeleport = false;
    public float teleportDistance = 5f;
    public float teleportCoolTime = 1f;
    private Vector2 teleportDirection = Vector2.zero;

    [Header("Stats")]
    private PlayerStats stats;

    private Animator animator;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerAnimation = GetComponent<PlayerAnimation>();
        playerAttack = GetComponent<PlayerAttack>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        stats = GetComponent<PlayerStats>();
    }

    public void HandleMovement()
    {
        if (!isSlide && !PlayerController.Instance.isKnockback)
        {
            moveInputX = Input.GetAxisRaw("Horizontal");
        }
        moveInputY = Input.GetAxisRaw("Vertical");
        moveInputY = Mathf.Clamp(moveInputY, 0, 1);

        teleportDirection = new Vector2(moveInputX, moveInputY);

        rb.linearVelocity = new Vector2(moveInputX * stats.moveSpeed, rb.linearVelocity.y);

        if (playerAttack.isAttack || playerAttack.isParrying)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocityY);
        }

        if (PlayerController.Instance.state == PlayerState.Warrior)
        {
            if (rb.linearVelocityX > 0)
            {
                spriteRenderer.flipX = false;
            }
            else if (rb.linearVelocityX < 0)
            {
                spriteRenderer.flipX = true;
            }
        }
        else
        {
            if (rb.linearVelocityX > 0)
            {
                spriteRenderer.flipX = true;
            }
            else if (rb.linearVelocityX < 0)
            {
                spriteRenderer.flipX = false;
            }
        }

        if (playerAnimation != null)
        {
            playerAnimation.SetWalking(moveInputX != 0);
        }

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        if (PlayerController.Instance.state == PlayerState.Warrior)
        {
            // 점프 후 착지 했는지 체크
            playerAnimation.SetIsGrounded(isGrounded);
            // 점프 후 떨어지는 애니메이션
            playerAnimation.SetFall(rb.linearVelocityY);

            Jump();
            if(PlayerController.Instance.stats.level > 1)
            {
                Slide();
            }
        }
        else
        {
            StartTeleport();
        }
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
            playerAttack.OffAttackCollider();
        }
    }

    private void StartTeleport()
    {
        if (!isGrounded || isTeleport)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            SoundManager.Instance.PlaySFX(SFXType.Slide);
            isTeleport = true;
            playerAnimation.TriggerTeleport();
            StartCoroutine(TeleportCoolDown());
        }
    }

    public void MoveTeleport()
    {
        transform.position += new Vector3(teleportDirection.x, teleportDirection.y, 0) * teleportDistance;
    }

    private IEnumerator TeleportCoolDown()
    {
        yield return new WaitForSeconds(teleportCoolTime);
        teleportDirection = Vector2.zero;
        isTeleport = false;
    }

    private void Slide()
    {
        if (!isGrounded || isSlide || (moveInputX == 0))
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.V))
        {

            SoundManager.Instance.PlaySFX(SFXType.Slide);
            isSlide = true;
            playerAttack.isAttack = false;
            stats.moveSpeed = slideSpeed;
            playerAnimation.TriggerSlide();
            playerAttack.OffAttackCollider();
            //playerAttack.OffParryingCollider();
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
        stats.moveSpeed = walkSpeed;
        PlayerController.Instance.isInvincible = false;
        isSlide = false;
    }

    public void PlayRunSFX()
    {
        SoundManager.Instance.PlaySFX(SFXType.Walk);
    }
}
