using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float enemyHP = 100;

    private Color originColor;
    private Renderer objecteRenderer;
    public float colorChangeDuration = .05f;

    [Header("attack effect transform")]
    public Transform[] particleTransform;
    [Header("hit camera shake")]
    public float duration = 0.2f;
    public float magnitude = 0.03f;

    private Animator animator;

    [Header("카메라 쉐이크 설정")]
    public CinemachineImpulseSource impulseSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        objecteRenderer = GetComponent<Renderer>();
        originColor = objecteRenderer.material.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GenerateCameraImpulse()
    {
        if(impulseSource != null)
        {
            Debug.Log("카메라 임펄스 발생");
            impulseSource.GenerateImpulse();
        }
        else
        {
            Debug.LogWarning("Impulse Source가 연결 안됨");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "PlayerAttack")
        {
            StartCoroutine(HitCoroutine());
            if(particleTransform.Length > 0)
            {
                int randIndex = Random.Range(0, particleTransform.Length);
                ParticleManager.Instance.ParticlePlay(ParticleType.PlayerAttack, particleTransform[randIndex].position, Vector3.one);
            }
        }
        else if(collision.tag == "Player")
        {
            if (animator != null)
            {
                animator.SetTrigger("Attack");
            }
        }
    }

    private IEnumerator HitCoroutine()
    {
        // 소리를 아직 못구함 > 임시로 패링 소리 삽입
        SoundManager.Instance.PlaySFX(SFXType.Parrying);
        StartCoroutine(Shake(duration, magnitude));
        //GenerateCameraImpulse();
        objecteRenderer.material.color = Color.red;
        yield return new WaitForSeconds(colorChangeDuration);
        objecteRenderer.material.color = originColor;
    }

    private IEnumerator Shake(float duration, float magnitude)
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
