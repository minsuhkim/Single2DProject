using System.Collections;
using UnityEngine;

public enum PlayerState
{
    Warrior,
    Bringer
}
public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    private PlayerMovement movement;
    private PlayerAttack attack;
    private PlayerHP hp;
    private PlayerExchangeForm changeForm;
    public PlayerAnimation anim;


    private Rigidbody2D rb;

    private Vector3 startPos;

    private bool isPaused = false;
    public GameObject pausePanel;

    [Header("¹«Àû")]
    public bool isInvincible = false;
    public float invincibilityDuration = 1.0f;

    [Header("CameraShake")]
    public float shakeDuration = 0.2f;
    public float shakeMagnitude = 0.03f;

    [Header("Knockback")]
    private float knockbackDuration = 0.2f;
    public float knockbackForce = 0.5f;
    public bool isKnockback = false;

    [Header("Form")]
    public PlayerState state = PlayerState.Warrior;

    [Header("Stat")]
    private PlayerStats stats;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        movement = GetComponent<PlayerMovement>();
        attack = GetComponent<PlayerAttack>();
        hp = GetComponent<PlayerHP>();
        anim = GetComponent<PlayerAnimation>();
        rb = GetComponent<Rigidbody2D>();
        changeForm = GetComponent<PlayerExchangeForm>();
        stats = GetComponent<PlayerStats>();
    }
    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        if (!isKnockback)
        {
            movement.HandleMovement();
        }

        if (movement.isGrounded && Input.GetKeyDown(KeyCode.Z) && !movement.isSlide)
        {
            attack.PerformAttack();
        }

        if (movement.isGrounded && Input.GetKeyDown(KeyCode.C) && !movement.isSlide)
        {
            attack.PerformAttack2();
        }

        if (movement.isGrounded && Input.GetKeyDown(KeyCode.A) && !movement.isSlide)
        {
            if(state == PlayerState.Warrior)
            {
                changeForm.ExchangeFormBringer();
                state = PlayerState.Bringer;
            }
            else
            {
                changeForm.ExchangeFormWarrior();
                state = PlayerState.Warrior;
            }

                stats.SetStats(state);
        }

        //if (movement.isGrounded && Input.GetKeyDown(KeyCode.C) && !movement.isDash && !movement.isSlide && !attack.isAttack)
        //{
        //    attack.PerformParrying();
        //}

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        pausePanel.SetActive(true);
        isPaused = true;
        Time.timeScale = 0;
    }

    public void Resume()
    {
        pausePanel.SetActive(false);
        isPaused = false;
        Time.timeScale = 1;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Coin")
        {
            GameManager.Instance.AddCoin(10);
            Destroy(collision.gameObject);
        }
        else if (collision.tag == "DeathZone")
        {
            // death
            transform.position = startPos;
            SoundManager.Instance.PlaySFX(SFXType.Hit);
        }
        else if (collision.tag == "BossZone")
        {
            SoundManager.Instance.PlayBGM(BGMType.BossBGM, 1);
        }
        else if (collision.tag == "EnemyAttack")
        {
            Hurt(collision.transform);
        }
    }

    private void Hurt(Transform enemyTransform)
    {
        if (!isInvincible)
        {
            anim.TriggerHurt();
            ParticleManager.Instance.ParticlePlay(ParticleType.PlayerDamage, transform.position, Vector3.one * 2);
            SoundManager.Instance.PlaySFX(SFXType.Hit);
            CameraManager.Instance.StartCameraShake(shakeDuration, shakeMagnitude);
            StartCoroutine(KnockbackCoroutine());
            rb.linearVelocity = Vector2.zero;
            rb.AddForce((transform.position - enemyTransform.position) * knockbackForce, ForceMode2D.Impulse);
            StartCoroutine(Invincibility());
        }
    }

    IEnumerator Invincibility()
    {
        isInvincible = true;
        //Time.timeScale = 0.5f;
        //float elapsedTime = 0f;
        //float blinkInterval = 0.2f;

        //while(elapsedTime < invincibilityDuration)
        //{
        //    elapsedTime += Time.deltaTime;
        //    yield return null;
        //}
        yield return new WaitForSeconds(invincibilityDuration);
        Time.timeScale = 1f;
        isInvincible = false;
    }

    IEnumerator KnockbackCoroutine()
    {
        isKnockback = true;
        yield return new WaitForSeconds(knockbackDuration);
        isKnockback = false;
    }
}
