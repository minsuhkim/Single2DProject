using UnityEngine;

public class ElfProjectile : MonoBehaviour
{
    [SerializeField]
    private float fireSpeed = 8f;

    private int damage = 2;
    private Vector3 direction = Vector3.zero;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        transform.position += direction * fireSpeed * Time.deltaTime;
    }

    public void SetArrow(Vector3 inDirection, int inDamage)
    {
        direction = inDirection;
        if (direction.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
        damage = inDamage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<PlayerStats>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
