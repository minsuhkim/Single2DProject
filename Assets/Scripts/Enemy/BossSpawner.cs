using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    public BossMage boss;

    public Dialog chapter2Dialog;
    public DialogueInfo chapter2DialogueInfo;

    private int enemyCnt = 4;

    private void Start()
    {
        chapter2Dialog.lines = new List<(int, string)>();

        for(int i=0; i<chapter2DialogueInfo.ids.Count; i++)
        {
            chapter2Dialog.lines.Add((chapter2DialogueInfo.ids[i], chapter2DialogueInfo.lines[i]));
        }
    }

    public void KillEnemy()
    {
        enemyCnt--;
        if (enemyCnt == 0)
        {
            //SpawnBoss();
            // 컷신 실행 후에 보스 소환
            SoundManager.Instance.PlayBGM(BGMType.BossBGM2);
            StartCoroutine(StartSpawnBossCoroutine());
        }
    }

    private IEnumerator StartSpawnBossCoroutine()
    {
        yield return StartCoroutine(DialogueManager.Instance.ShowDialog(chapter2Dialog));
        SpawnBoss();
    }

    private void SpawnBoss()
    {
        SoundManager.Instance.PlayBGM(BGMType.BossBattleBGM2);
        boss.gameObject.SetActive(true);
        CameraManager.Instance.StartZoomBoss();
    }


}
