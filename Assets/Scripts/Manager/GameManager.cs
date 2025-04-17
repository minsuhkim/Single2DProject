using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private int coinCount = 0;
    public Text coinText;

    private const string COIN_KEY = "CoinCount";
    private const string DAMAGE_KEY = "PlayerDamage";
    private const string ATTACK_SPEED_KEY = "PlayerAttackSpeed";
    private const string MOVE_SPEED_KEY = "PlayerMoveSpeed";
    private const string HP_KEY = "PlayerHP";

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddCoin(int amount)
    {
        coinCount += amount;
        coinText.text = coinCount.ToString();
        SoundManager.Instance.PlaySFX(SFXType.Coin);
        SaveCoin();
    }

    public bool UseCoin(int amount)
    {
        if(coinCount >= amount)
        {
            coinCount -= amount;
            SaveCoin();
            return true;
        }

        return false;
    }

    public int GetCoinCount()
    {
        return coinCount;
    }

    public void ResetCoin()
    {
        coinCount = 0;
        coinText.text = coinCount.ToString();
        SaveCoin();
    }

    private void SaveCoin()
    {
        PlayerPrefs.SetInt(COIN_KEY, coinCount);
        PlayerPrefs.Save();
    }

    private void LoadCoin()
    {
        coinCount = PlayerPrefs.GetInt(COIN_KEY, 0);
    }

    public void SavePlayerStats(PlayerStats stats)
    {
        PlayerPrefs.SetInt(DAMAGE_KEY, stats.warriorDamage);
        PlayerPrefs.SetInt(HP_KEY, stats.warriorMaxHP);
        PlayerPrefs.SetFloat(MOVE_SPEED_KEY, stats.warriorMoveSpeed);
        PlayerPrefs.SetFloat(ATTACK_SPEED_KEY, stats.warriorAttackSpeed);
    }

    public void LoadPlayerStats(PlayerStats stats)
    {
        if (PlayerPrefs.HasKey(DAMAGE_KEY))
        {
            stats.warriorDamage = PlayerPrefs.GetInt(DAMAGE_KEY);
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
