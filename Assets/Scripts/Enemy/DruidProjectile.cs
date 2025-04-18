using UnityEngine;

public class DruidProjectile : MonoBehaviour
{
    [SerializeField]
    private float destroyTime = 1f;
    void Start()
    {
        Destroy(gameObject, 1f);
    }

}
