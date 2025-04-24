using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private PlayerAnimation playerAnimation;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    public bool isAttack = false;
    public bool isParrying = false;

    [Header("Attack")]
    //public string attackStateName = "Attack";
    public GameObject warriorAttackRangeLeft;
    public GameObject warriorAttackRangeRight;
    public GameObject bringerAttackRangeLeft;
    public GameObject bringerAttackRangeRight;

    [Header("Attack2")]
    public GameObject warriorAttack2RangeLeft;
    public GameObject warriorAttack2RangeRight;

    [Header("Range Attack")]
    [SerializeField]
    private List<GameObject> detectEnemy = new List<GameObject>();
    public float detectRange = 5f;
    public LayerMask enemyLayerMask;
    public GameObject projectilePrefab;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerAnimation = GetComponent<PlayerAnimation>();
        animator = GetComponent<Animator>();
    }

    public void DetectNearestEnemy()
    {
        RaycastHit2D[] hits;
        hits = Physics2D.CircleCastAll(transform.position, detectRange, Vector2.zero, 0, layerMask: enemyLayerMask);
        if (hits.Length > 0)
        {
            foreach(var hit in hits)
            {
                detectEnemy.Add(hit.collider.gameObject);
            }
        }
        else
        {
            if(detectEnemy.Count > 0)
            {
                detectEnemy.Clear();
            }
        }
    }

    public void RangeAttack()
    {
        if (detectEnemy.Count > 0)
        {
            for(int i=0; i<detectEnemy.Count; i++)
            {
                if (!detectEnemy[i].GetComponent<Enemy>().isLive)
                {
                    continue;
                }
                Instantiate(projectilePrefab, detectEnemy[i].transform.position + new Vector3(0, 1, 0), Quaternion.identity);
                detectEnemy[i].GetComponent<Enemy>().TakeDamage(PlayerController.Instance.stats.attack2Damage);
            }
            detectEnemy.Clear();
        }
    }

    public void PerformAttack()
    {
        if (isAttack)
        {
            return;
        }

        if (playerAnimation != null)
        {
            playerAnimation.TriggerAttack();
            SoundManager.Instance.PlaySFX(SFXType.Attack);
        }
        StartCoroutine(AttackCooldown());
    }

    public void PerformAttack2()
    {
        if (isAttack)
        {
            return;
        }

        if (playerAnimation != null)
        {
            playerAnimation.TriggerAttack2();
            if(PlayerController.Instance.state == PlayerState.Warrior)
            {
                SoundManager.Instance.PlaySFX(SFXType.Attack);
            }
            else
            {
                SoundManager.Instance.PlaySFX(SFXType.Attack2);
            }
        }
        if(PlayerController.Instance.state == PlayerState.Bringer)
        {
            DetectNearestEnemy();
        }
        StartCoroutine(Attack2Cooldown());
    }

    public void PlayAttackSFX()
    {
        SoundManager.Instance.PlaySFX(SFXType.Attack);
    }

    public void OnAttackCollider()
    {
        if (PlayerController.Instance.state == PlayerState.Warrior)
        {
            if (spriteRenderer.flipX)
            {
                warriorAttackRangeLeft.SetActive(true);
            }
            else
            {
                warriorAttackRangeRight.SetActive(true);
            }
        }
        else
        {
            if (spriteRenderer.flipX)
            {
                bringerAttackRangeRight.SetActive(true);
            }
            else
            {
                bringerAttackRangeLeft.SetActive(true);
            }
        }
    }

    public void OnAttack2Collider()
    {
        if (spriteRenderer.flipX)
        {
            warriorAttack2RangeLeft.SetActive(true);
        }
        else
        {
            warriorAttack2RangeRight.SetActive(true);
        }
    }

    public void OffAttackCollider()
    {
        warriorAttackRangeLeft.SetActive(false);
        warriorAttackRangeRight.SetActive(false);
        warriorAttack2RangeLeft.SetActive(false);
        warriorAttack2RangeRight.SetActive(false);
        bringerAttackRangeLeft.SetActive(false);
        bringerAttackRangeRight.SetActive(false);
    }

    private IEnumerator AttackCooldown()
    {
        isAttack = true;
        //안정성을 위함(다음 프레임까지 대기)
        yield return null;

        yield return new WaitForSeconds(PlayerStats.Instance.attackSpeed);
        OffAttackCollider();
        isAttack = false;
    }

    private IEnumerator Attack2Cooldown()
    {
        isAttack = true;
        //안정성을 위함(다음 프레임까지 대기)
        yield return null;

        yield return new WaitForSeconds(PlayerStats.Instance.attack2Speed);
        if (PlayerController.Instance.state == PlayerState.Warrior)
        {
            OffAttackCollider();
        }
        isAttack = false;
    }
}
