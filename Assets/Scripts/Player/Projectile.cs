using UnityEngine;

public class Projectile : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, 1f);
    }
}
