using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerMovement movement;
    private PlayerAttack attack;
    private PlayerHP hp;

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        attack = GetComponent<PlayerAttack>();
        hp = GetComponent<PlayerHP>();
    }

    private void Update()
    {
        movement.HandleMovement();
        if (movement.isGrounded && Input.GetKeyDown(KeyCode.Z) && !movement.isDash)
        {
            attack.PerformAttack();
        }
    }
}
