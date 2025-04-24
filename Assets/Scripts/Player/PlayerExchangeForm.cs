using UnityEngine;

public class PlayerExchangeForm : MonoBehaviour
{
    public RuntimeAnimatorController warriorAnimator;
    public RuntimeAnimatorController bringerAnimator;

    public GameObject effectPrefab;

    public void ExchangeFormWarrior()
    {
        GetComponent<CapsuleCollider2D>().size = new Vector2(GetComponent<CapsuleCollider2D>().size.x, 0.3f);
        SoundManager.Instance.PlaySFX(SFXType.ChangeForm);
        GameObject clone = Instantiate(effectPrefab, transform.position, Quaternion.identity);
        clone.transform.localScale = Vector3.one * 3f;
        Destroy(clone, 1f);
        PlayerController.Instance.anim.animator.runtimeAnimatorController = warriorAnimator;
    }

    public void ExchangeFormBringer()
    {
        GetComponent<CapsuleCollider2D>().size = new Vector2(GetComponent<CapsuleCollider2D>().size.x, 0.5f);
        SoundManager.Instance.PlaySFX(SFXType.ChangeForm);
        GameObject clone = Instantiate(effectPrefab, transform.position, Quaternion.identity);
        clone.transform.localScale = Vector3.one * 3f;
        Destroy(clone, 1f);
        PlayerController.Instance.anim.animator.runtimeAnimatorController = bringerAnimator;
    }
}
