using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private const string DAMAGE_KEY = "PlayerDamage";
    private const string ATTACK_SPEED_KEY = "PlayerAttackSpeed";
    private const string MOVE_SPEED_KEY = "PlayerMoveSpeed";
    private const string HP_KEY = "PlayerHP";

    public bool isPause = false;

    [Header("Clear")]
    public GameObject clearPanel;
    private bool isClear = false;

    public GameObject doorToNextChapter;

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
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPause)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }

        if (isClear)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                LoadMainMenu();
            }
        }
    }

    public void OnButtonClickSound()
    {
        SoundManager.Instance.PlaySFX(SFXType.ButtonClick);
    }

    public void OpenDoor()
    {
        doorToNextChapter.SetActive(true);
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        isPause = true;
        UIManager.Instance.SetPauseGroup(isPause);
        UIManager.Instance.SetGameGroup(!isPause);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        isPause = false;
        UIManager.Instance.SetPauseGroup(isPause);
        UIManager.Instance.SetGameGroup(!isPause);
        UIManager.Instance.OffOptionGroup();
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        UIManager.Instance.OnGameOverGroup();
        UIManager.Instance.SetGameGroup(false);
    }

    public void GameClear()
    {
        //SoundManager.Instance.PlayBGM(BGMType.GameClearBGM);
        isClear = true;
        Time.timeScale = 0;
        clearPanel.SetActive(true);

    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1;
        SceneManagerController.Instance.StartSceneTransition("Menu");
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
    }

    public void SavePlayerStats(PlayerStats stats)
    {
        PlayerPrefs.SetInt(DAMAGE_KEY, stats.warriorAttackDamage);
        PlayerPrefs.SetInt(HP_KEY, stats.warriorMaxHP);
        PlayerPrefs.SetFloat(MOVE_SPEED_KEY, stats.warriorMoveSpeed);
        PlayerPrefs.SetFloat(ATTACK_SPEED_KEY, stats.warriorAttackSpeed);
    }

    public void LoadPlayerStats(PlayerStats stats)
    {
        if (PlayerPrefs.HasKey(DAMAGE_KEY))
        {
            stats.warriorAttackDamage = PlayerPrefs.GetInt(DAMAGE_KEY);
        }
        if (PlayerPrefs.HasKey(HP_KEY))
        {
            stats.warriorMaxHP = PlayerPrefs.GetInt(HP_KEY);
        }
        if (PlayerPrefs.HasKey(MOVE_SPEED_KEY))
        {
            stats.warriorMoveSpeed = PlayerPrefs.GetFloat(MOVE_SPEED_KEY);
        }
        if (PlayerPrefs.HasKey(ATTACK_SPEED_KEY))
        {
            stats.warriorAttackSpeed = PlayerPrefs.GetFloat(ATTACK_SPEED_KEY);
        }
    }
}
