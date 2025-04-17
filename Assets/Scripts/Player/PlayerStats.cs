using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;

    [Header("warrior �ɷ�ġ")]
    public int warriorMaxHP = 100;
    public int warriorDamage = 10;
    public float warriorAttackSpeed = 0.5f;
    public float warriorAttack2Speed = 0.5f;
    public float warriorMoveSpeed = 3.0f;

    [Header("bringer �ɷ�ġ")]
    public int bringerMaxHP = 200;
    public int bringerDamage = 20;
    public float bringerAttackSpeed = 0.7f;
    public float bringerAttack2Speed = 1f;
    public float bringerMoveSpeed = 3.0f;

    [Header("���� �ɷ�ġ")]
    public int maxHP = 100;
    public int currentHp;
    public int damage;
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
            currentHp = maxHP;
            damage = warriorDamage;
            attackSpeed = warriorAttackSpeed;
            attack2Speed = warriorAttack2Speed;
            moveSpeed = warriorMoveSpeed;
        }
        else
        {
            maxHP = bringerMaxHP;
            currentHp = maxHP;
            damage = bringerDamage;
            attackSpeed = bringerAttackSpeed;
            attack2Speed = bringerAttack2Speed;
            moveSpeed = bringerMoveSpeed;
        }
    }

    public void TakeDamage(int amount)
    {
        SoundManager.Instance.PlaySFX(SFXType.Hit);
        currentHp -= amount;
        if(currentHp <= 0)
        {
            // dead
        }
    }

    public void Heal(int amount)
    {
        currentHp += amount;
        if(currentHp > warriorMaxHP)
        {
            currentHp = warriorMaxHP;
        }
    }

    public void Die()
    {
        // GameOver â ����
    }

    public void LevelUp()
    {
        level++;
    }

    public int GetDamage() => warriorDamage;
    public float GetAttackSpeed() => warriorAttackSpeed;
    
    public void UpgradeDamage(int amount)
    {
        warriorDamage += amount;
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