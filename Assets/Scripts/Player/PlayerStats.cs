using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;

    [Header("warrior 능력치")]
    public int warriorMaxHP = 100;
    public int warriorAttackDamage = 10;
    public int warriorAttack2Damage = 15;
    public float warriorAttackSpeed = 0.5f;
    public float warriorAttack2Speed = 0.5f;
    public float warriorMoveSpeed = 3.0f;

    [Header("bringer 능력치")]
    public int bringerMaxHP = 200;
    public int bringerAttackDamage = 20;
    public int bringerAttack2Damage = 15;
    public float bringerAttackSpeed = 0.7f;
    public float bringerAttack2Speed = 1f;
    public float bringerMoveSpeed = 3.0f;

    [Header("현재 능력치")]
    public int maxHP = 5;
    public float maxMP = 10;
    public int currentHp;
    public int attackDamage;
    public int attack2Damage;
    public float attackSpeed;
    public float attack2Speed;
    public float moveSpeed;
    public int level = 0;


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
        currentHp = warriorMaxHP;
    }

    void Start()
    {
        GameManager.Instance.LoadPlayerStats(this);
        SetStats(PlayerState.Warrior);
    }

    public void SetStats(PlayerState state)
    {
        if(state == PlayerState.Warrior)
        {
            maxHP = warriorMaxHP;
            attackDamage = warriorAttackDamage;
            attack2Damage = warriorAttack2Damage;
            attackSpeed = warriorAttackSpeed;
            attack2Speed = warriorAttack2Speed;
            moveSpeed = warriorMoveSpeed;
        }
        else
        {
            maxHP = bringerMaxHP;
            attackDamage = bringerAttackDamage;
            attack2Damage = bringerAttack2Damage;
            attackSpeed = bringerAttackSpeed;
            attack2Speed = bringerAttack2Speed;
            moveSpeed = bringerMoveSpeed;
        }
    }

    public void TakeDamage(int amount)
    {
        SoundManager.Instance.PlaySFX(SFXType.Hit);
        currentHp -= amount;
        UIManager.Instance.SetHPImage(currentHp);
        if(currentHp <= 0)
        {
            GameManager.Instance.GameOver();
        }
    }

    public void Heal(int amount)
    {
        currentHp += amount;
        if(currentHp > warriorMaxHP)
        {
            currentHp = warriorMaxHP;
        }
        UIManager.Instance.SetHPImage(currentHp);
    }

    public void Die()
    {
        // GameOver 창 시작
    }

    public void LevelUp()
    {
        level++;
    }

    public int GetDamage() => warriorAttackDamage;
    public float GetAttackSpeed() => warriorAttackSpeed;
    
    public void UpgradeDamage(int amount)
    {
        warriorAttackDamage += amount;
        GameManager.Instance.SavePlayerStats(this);
    }

    public void UpgradeAttackSpeed(float amount)
    {
        warriorAttackSpeed += amount;
        GameManager.Instance.SavePlayerStats(this);
    }

    public void UpgradeHP(int amount)
    {
        warriorMaxHP += amount;
        GameManager.Instance.SavePlayerStats(this);
    }

    public void UpgradeMoveSpeed(float amount)
    {
        warriorMoveSpeed += amount;
        GameManager.Instance.SavePlayerStats(this);
    }
}