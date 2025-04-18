using UnityEngine;

public class MageProjectile : MonoBehaviour
{
    private void Start()
    {
        Destroy(gameObject, 1f);
    }
}
