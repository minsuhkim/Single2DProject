using UnityEngine;

public class AIManager : MonoBehaviour
{
    public GameObject monsterPrefab;
    public float spawnRangeX = 10.0f;
    public float spawnRangeY = 5.0f;
    public int enemyCount = 5;
    public Transform[] spawnPoints;
    private float enemySpeed = 1.0f;

    private EnemyType type;

    private void Start()
    {
        SpawnEnemies();
    }

    void SpawnEnemies()
    {
        for(int i=0; i<enemyCount; i++)
        {
            if(spawnPoints.Length> 0)
            {
                int randIdx = Random.Range(0, spawnPoints.Length);
                Vector2 spawnPosition = spawnPoints[randIdx].position;
                Instantiate(monsterPrefab, spawnPosition, Quaternion.identity);
            }
            else
            {
                float randomX = Random.Range(-spawnRangeX, spawnRangeX);
                float randomY = Random.Range(-spawnRangeY, spawnRangeY);
                Vector2 randomPosition = new Vector2(randomX, randomY);
                Instantiate(monsterPrefab, randomPosition, Quaternion.identity);
            }
        }
    }

    void MonsterSetState()
    {
        Enemy enemy = monsterPrefab.GetComponent<Enemy>();

        float minSpeed = 1f;
        float maxSpeed = 10f;
        if(type == EnemyType.Bringer)
        {
            minSpeed = 1f;
            maxSpeed = 5f;
        }

        enemySpeed = Random.Range(minSpeed, maxSpeed);
        enemy.speed = enemySpeed;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(Vector2.zero, new Vector2(spawnRangeX * 2, spawnRangeY * 2));
        Gizmos.color = Color.blue;
        if(spawnPoints.Length > 0)
        {
            foreach(Transform spawnPoint in spawnPoints)
            {
                Gizmos.DrawWireSphere(spawnPoint.position, 0.5f);
            }
        }
    }
}
