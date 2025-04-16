using UnityEngine;

public class ShopManager : MonoBehaviour
{
    // �⺻ ���׷��̵� ���
    public int baseDamageCost = 1;
    public int baseAttackSpeedCost = 1;
    public int baseMoveSpeedCost = 1;
    public int baseHPCost = 1;

    // ���׷��̵� ��ġ
    public int damageUpgradeAmount = 5;
    public float attackSpeedUpgradeAmount = 0.2f;
    public float moveSpeedUgradeAmount = 0.5f;
    public int hpUpgradeAmount = 10;

    // ���׷��̵� Ƚ�� ����
    private int damageUpgradeCount = 0;
    private int attackSpeedUpgradeCount = 0;
    private int moveSpeedUpgradeCount = 0;
    private int hpUpgradeCount = 0;

    // ���� ��� ����
    private const int increaseThreshold = 3;            // 3ȸ �̻��� �� ���� ����
    private const float priceIncreaseRate = 1.5f;       // ��� 50�� ����

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
            // ��� ����
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
            // ��� ����
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
            // ��� ����
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
            // ��� ����
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
