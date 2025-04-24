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
    public GameObject nextKeyImage;

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
            // 여기에 소리 넣으면 글씨 나오면서 소리도 같이 나옴
            SoundManager.Instance.PlaySFX(SFXType.Text);
            dialogText.text += ch;
            yield return new WaitForSeconds(1f / charPerSeconds);
        }
    }

    public IEnumerator ShowDialog(Dialog dialog)
    {
        dialogueBox.SetActive(true);
        PlayerController.Instance.isDialog = true;

        foreach(var line in dialog.lines)
        {
            dialog.dialogImages[line.Item1].SetActive(true);
            yield return TypeDialog(line);
            nextKeyImage.SetActive(true);
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
            nextKeyImage.SetActive(false);
            dialog.dialogImages[line.Item1].SetActive(false);
        }

        dialogueBox.SetActive(false);
        PlayerController.Instance.isDialog = false;
    }
}
