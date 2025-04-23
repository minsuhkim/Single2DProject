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
        //dialog.lines.Add((0, "���䡦 ������ �ʾ�. ������ ��ﳪ�� �ʾ�."));
        //dialog.lines.Add((0, "�� �� �ʸӿ� ���𰡰� �� ��ٸ��� �־�. �̲����� �쿬�� �ƴ� �ž�."));
        //dialog.lines.Add((0, "������ � ����̵硦 ������ �����ؾ� �� ����."));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            StartCoroutine(DialogueManager.Instance.ShowDialog(dialog));
        }
    }
}
