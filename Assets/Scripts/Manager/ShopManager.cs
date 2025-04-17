using UnityEngine;

public class ShopManager : MonoBehaviour
{
    // 기본 업그레이드 비용
    public int baseDamageCost = 1;
    public int baseAttackSpeedCost = 1;
    public int baseMoveSpeedCost = 1;
    public int baseHPCost = 1;

    // 업그레이드 수치
    public int damageUpgradeAmount = 5;
    public float attackSpeedUpgradeAmount = 0.2f;
    public float moveSpeedUgradeAmount = 0.5f;
    public int hpUpgradeAmount = 10;

    // 업그레이드 횟수 추적
    private int damageUpgradeCount = 0;
    private int attackSpeedUpgradeCount = 0;
    private int moveSpeedUpgradeCount = 0;
    private int hpUpgradeCount = 0;

    // 가격 상승 조건
    private const int increaseThreshold = 3;            // 3회 이상일 때 가격 증가
    private const float priceIncreaseRate = 1.5f;       // 비용 50퍼 증가

    private PlayerStats playerStats;

    private void Start()
    {
        playerStats = PlayerStats.Instance;
    }

    private int GetCost(int baseCost, int upgradeCount)
    {
        if (upgradeCount < increaseThreshold)
        {
            return baseCost;
        }

        return Mathf.FloorToInt(baseCost * priceIncreaseRate);
    }

    public void UpgradeDamage()
    {
        int cost = GetCost(baseDamageCost, damageUpgradeCount);

        if (GameManager.Instance.UseCoin(cost))
        {
            damageUpgradeCount++;
            playerStats.UpgradeDamage(damageUpgradeAmount);
        }
        else
        {
            // 비용 부족
        }
    }

    public void UpgradeAttackSpeed()
    {
        int cost = GetCost(baseAttackSpeedCost, attackSpeedUpgradeCount);

        if (GameManager.Instance.UseCoin(cost))
        {
            attackSpeedUpgradeCount++;
            playerStats.UpgradeAttackSpeed(attackSpeedUpgradeAmount);
        }
        else
        {
            // 비용 부족
        }
    }

    public void UpgradeHP()
    {
        int cost = GetCost(baseHPCost, hpUpgradeCount);

        if (GameManager.Instance.UseCoin(cost))
        {
            hpUpgradeCount++;
            playerStats.UpgradeHP(hpUpgradeAmount);
        }
        else
        {
            // 비용 부족
        }
    }

    public void UpgradeMoveSpeed()
    {
        int cost = GetCost(baseMoveSpeedCost, moveSpeedUpgradeCount);

        if (GameManager.Instance.UseCoin(cost))
        {
            moveSpeedUpgradeCount++;
            playerStats.UpgradeAttackSpeed(moveSpeedUgradeAmount);
        }
        else
        {
            // 비용 부족
        }
    }

    //public void ResetStats()
    //{
    //    if (GameManager.Instance.UseCoin(100))
    //    {
    //        playerStats.warriorDamage = 
    //    }
    //}
}
