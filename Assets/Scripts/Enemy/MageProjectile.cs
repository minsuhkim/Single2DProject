using UnityEngine;

public class MageProjectile : MonoBehaviour
{
    private void Start()
    {
        SoundManager.Instance.PlaySFX(SFXType.MageAttack);
        Destroy(gameObject, 1f);
    }
}
