using UnityEngine;

public class EnemyStun : MonoBehaviour
{
    private Enemy enemy;

    private void Start()
    {
        enemy = GetComponentInParent<Enemy>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if(collision.tag == "PlayerParrying")
        {
            SoundManager.Instance.PlaySFX(SFXType.Parrying);
            CameraManager.Instance.StartCameraShake(0.1f, 1f);
            enemy.ChangeState(EnemyState.Stun);
        }
    }
}
