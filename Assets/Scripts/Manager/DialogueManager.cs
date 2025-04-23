using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Dialog
{
    public List<GameObject> dialogImages;
    public List<(int,string)> lines;
}

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    public GameObject dialogueBox;
    public Text dialogText;
    public int charPerSeconds = 15;


    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public IEnumerator TypeDialog((int, string) dialog)
    {
        dialogText.text = "";
        foreach(var ch in dialog.Item2.ToCharArray())
        {
            // ���⿡ �Ҹ� ������ �۾� �����鼭 �Ҹ��� ���� ����
            dialogText.text += ch;
            yield return new WaitForSeconds(1f / charPerSeconds);
        }
    }

    public IEnumerator ShowDialog(Dialog dialog)
    {
        dialogueBox.SetActive(true);

        foreach(var line in dialog.lines)
        {
            dialog.dialogImages[line.Item1].SetActive(true);
            yield return TypeDialog(line);
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
            dialog.dialogImages[line.Item1].SetActive(false);
        }

        dialogueBox.SetActive(false);
    }
}
