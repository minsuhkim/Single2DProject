using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerMovement movement;
    private PlayerAttack attack;
    private PlayerHP hp;

    private Vector3 startPos;


    private bool isPaused = false;
    public GameObject pausePanel;
    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        attack = GetComponent<PlayerAttack>();
        hp = GetComponent<PlayerHP>();
    }
    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        movement.HandleMovement();
        if (movement.isGrounded && Input.GetKeyDown(KeyCode.Z) && !movement.isDash && !movement.isSlide && !attack.isParrying)
        {
            attack.PerformAttack();
        }

        if (movement.isGrounded && Input.GetKeyDown(KeyCode.C) && !movement.isDash && !movement.isSlide && !attack.isAttack)
        {
            attack.PerformParrying();
        }

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
        if(collision.tag == "Coin")
        {
            GameManager.Instance.AddCoin(10);
            Destroy(collision.gameObject);
        }
        else if(collision.tag == "DeathZone")
        {
            // death
            transform.position = startPos;
            SoundManager.Instance.PlaySFX(SFXType.Hit);
        }
        else if(collision.tag == "BossZone")
        {
            SoundManager.Instance.PlayBGM(BGMType.BossBGM, 1);
        }
    }
}
