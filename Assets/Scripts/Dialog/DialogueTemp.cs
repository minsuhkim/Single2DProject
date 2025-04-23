using System.Collections.Generic;
using UnityEngine;

public class DialogueTemp : MonoBehaviour
{
    public Dialog dialog = new Dialog();
    public DialogueInfo info;
    private void Start()
    {
        dialog.lines = new List<(int, string)>();
        for(int i=0; i<info.ids.Count; i++)
        {
            dialog.lines.Add((info.ids[i], info.lines[i]));
        }
        //dialog.lines.Add((0, "여긴… 낯설지 않아. 하지만 기억나진 않아."));
        //dialog.lines.Add((0, "이 문 너머에 무언가가 날 기다리고 있어. 이끌림은 우연이 아닐 거야."));
        //dialog.lines.Add((0, "진실이 어떤 모습이든… 이제는 마주해야 할 때야."));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            StartCoroutine(DialogueManager.Instance.ShowDialog(dialog));
        }
    }
}
