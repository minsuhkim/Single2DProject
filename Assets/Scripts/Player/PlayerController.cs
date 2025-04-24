using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    private PlayerExchangeForm changeForm;
    public PlayerAnimation anim;


    private Rigidbody2D rb;

    private Vector3 startPos;

    private bool isFirstChange = true;
    public GameObject bringer;
    public GameObject bringerSkillDescGroup;

    [Header("무적")]
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
    public PlayerStats stats;

    [Header("Heal")]
    public float curMP;
    public float healCount = 0;
    public float healCoolTime = 1.5f;
    public bool isHeal = false;
    public float requireMPToHeal = 2.5f;
    public GameObject healEffect;


    [Header("BossRoom")]
    public GameObject[] bossRoom;

    [Header("Dialog")]
    public bool isDialog = false;


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
        anim = GetComponent<PlayerAnimation>();
        rb = GetComponent<Rigidbody2D>();
        changeForm = GetComponent<PlayerExchangeForm>();
        stats = GetComponent<PlayerStats>();
        curMP = stats.maxMP;
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

        if (movement.isGrounded && Input.GetKeyDown(KeyCode.Z) && !movement.isSlide && !isDialog)
        {
            attack.PerformAttack();
        }

        if (stats.level > 0 && movement.isGrounded && Input.GetKeyDown(KeyCode.C) && !movement.isSlide && !isDialog)
        {
            attack.PerformAttack2();
        }

        if (movement.isGrounded && Input.GetKey(KeyCode.S) && !movement.isSlide && movement.moveInputX == 0 && !isDialog)
        {
            if(isHeal == false && curMP >= requireMPToHeal && stats.maxHP > stats.currentHp)
            {
                healCount += Time.deltaTime;
                healEffect.SetActive(true);
                if (healCount >= 1f)
                {
                    stats.Heal(1);
                    curMP -= requireMPToHeal;
                    UIManager.Instance.SetMPImage(curMP);
                    StartCoroutine(HealCoolDownCoroutine());
                    healEffect.SetActive(false);
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.S) || movement.moveInputX != 0)
        {
            healCount = 0;
            healEffect.SetActive(false);
        }

        if (stats.level > 2 &&  movement.isGrounded && Input.GetKeyDown(KeyCode.A) && !movement.isSlide && !attack.isAttack && !isDialog)
        {
            if (state == PlayerState.Warrior)
            {
                if (isFirstChange)
                {
                    if(bringer && bringerSkillDescGroup)
                    {
                        bringer.SetActive(false);
                        bringerSkillDescGroup.SetActive(true);
                    }
                    isFirstChange = false;
                }
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

        if(state == PlayerState.Bringer)
        {
            curMP -= Time.deltaTime;
            if(curMP <= 0)
            {
                curMP = 0;
                changeForm.ExchangeFormWarrior();
                state = PlayerState.Warrior;
                stats.SetStats(state);
            }
            UIManager.Instance.SetMPImage(curMP);
        }
         
    }



    private IEnumerator HealCoolDownCoroutine()
    {
        isHeal = true;
        yield return new WaitForSeconds(healCoolTime);
        isHeal = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "DeathZone")
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
        else if(collision.tag == "Door")
        {
            // 일단 하드 코딩으로 씬 넘겨주기
            string currentScene = SceneManager.GetActiveScene().name;
            int chapter = currentScene[7] - '0';
            SceneManagerController.Instance.StartSceneTransition("Chapter" + (chapter+1).ToString());
        }
        else if(collision.tag == "Item")
        {
            // 해금
            SoundManager.Instance.PlaySFX(SFXType.Item);
            stats.LevelUp();
            collision.gameObject.SetActive(false);
        }
        else if(collision.tag == "Trap")
        {
            Hurt(collision.gameObject.transform);
            transform.position = startPos;
        }
        else if(collision.tag == "EnterBossRoom")
        {
            // 보스룸 입장
            StartBossRoom();
            collision.gameObject.SetActive(false);
        }
    }

    private void StartBossRoom()
    {
        foreach(var obj in bossRoom)
        {
            obj.SetActive(true);
        }
        SoundManager.Instance.PlayBGM(BGMType.BossBattleBGM);
        CameraManager.Instance.StartZoomBoss();
    }

    private void Hurt(Transform enemyTransform)
    {
        if (!isInvincible)
        {
            anim.TriggerHurt();
            stats.TakeDamage(1);
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
