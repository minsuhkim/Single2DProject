using UnityEngine;

public class PlayerEvent : MonoBehaviour
{
    public GameObject arrowKeyObj;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == "TutorialEvent1")
        {
            arrowKeyObj.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.name == "TutorialEvent1")
        {
            arrowKeyObj.SetActive(false);
        }
    }
}
