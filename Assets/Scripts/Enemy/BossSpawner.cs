using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    public BossMage boss;


    private int enemyCnt = 4;

    public void KillEnemy()
    {
        enemyCnt--;
        if (enemyCnt == 0)
        {
            //SpawnBoss();
            // 컷신 실행 후에 보스 소환
        }
    }

    private void SpawnBoss()
    {
        boss.gameObject.SetActive(true);
        CameraManager.Instance.StartZoomBoss();
    }


}
