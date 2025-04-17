using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "EnemyData/EnemyData")]
public class EnemyData : ScriptableObject
{
    public int damage;
    public int maxHP;
    public float moveSpeed;
    public float attackSpeed;
}
