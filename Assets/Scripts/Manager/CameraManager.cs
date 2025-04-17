using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }

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
