using UnityEngine;

public class PlayerExchangeForm : MonoBehaviour
{
    public RuntimeAnimatorController warriorAnimator;
    public RuntimeAnimatorController bringerAnimator;

    public void ExchangeFormWarrior()
    {
        PlayerController.Instance.anim.animator.runtimeAnimatorController = warriorAnimator;
    }

    public void ExchangeFormBringer()
    {
        PlayerController.Instance.anim.animator.runtimeAnimatorController = bringerAnimator;
    }
}
