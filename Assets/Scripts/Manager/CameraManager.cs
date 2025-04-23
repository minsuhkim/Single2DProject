using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }

    public GameObject cinemachineCamera;
    public GameObject boss;
    public Transform bossZoomPos;
    private float zoomSpeed = 2f;

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

    public void StartZoomBoss()
    {
        StartCoroutine(ZoomBossCoroutine());
    }

    private IEnumerator ZoomBossCoroutine()
    {
        cinemachineCamera.SetActive(false);
        Camera.main.orthographicSize = 2.5f;
        UIManager.Instance.playerGroup.SetActive(false);
        UIManager.Instance.SetBossName();
        while (Vector3.Distance(bossZoomPos.position, Camera.main.transform.position) > 0.01f)
        {
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, bossZoomPos.position, zoomSpeed * Time.deltaTime);
            yield return null;
        }
        Camera.main.transform.position = bossZoomPos.position;
        cinemachineCamera.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        UIManager.Instance.bossNameGroup.SetActive(false);
        UIManager.Instance.playerGroup.SetActive(true);
        boss.GetComponent<Boss>().bossState = BossState.Battle;
    }

    public void StartCameraShake(float duration, float magnitude)
    {
        StartCoroutine(CameraShake(duration, magnitude));
    }

    private IEnumerator CameraShake(float duration, float magnitude)
    {
        if (Camera.main == null)
        {
            yield break;
        }
        Vector3 originPos = Camera.main.transform.localPosition;
        Camera.main.GetComponent<CinemachineBrain>().enabled = false;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            float x = Random.Range(-1, 1) * magnitude;
            float y = Random.Range(-1, 1) * magnitude;

            Camera.main.transform.localPosition = new Vector3(originPos.x + x, originPos.y + y, originPos.z);

            elapsed += Time.deltaTime;
            yield return null;
        }
        Camera.main.transform.localPosition = originPos;
        Camera.main.GetComponent<CinemachineBrain>().enabled = true;
    }
}
