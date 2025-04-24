using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public GameObject[] hpImages;
    public Image mpImage;
    public GameObject bossNameGroup;
    public GameObject playerGroup;

    [Header("Pause")]
    public GameObject pauseGroup;

    [Header("Resume")]
    public GameObject gameGroup;

    [Header("GameOver")]
    public GameObject gameOverGroup;

    [Header("First Hit")]
    public GameObject firstHitText;

    [Header("Option")]
    public GameObject optionGroup;

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

    public void OffOptionGroup()
    {
        optionGroup.SetActive(false);
    }

    public void OnGameOverGroup()
    {
        gameOverGroup.SetActive(true);
    }

    public void SetPauseGroup(bool isPause)
    {
        pauseGroup.SetActive(isPause);
    }

    public void SetGameGroup(bool isPlay)
    {
        gameGroup.SetActive(isPlay);
    }

    public void SetHPImage(int curHP)
    {
        for(int i=0; i<curHP; i++)
        {
            hpImages[i].SetActive(true);
        }
        for(int i= curHP; i<hpImages.Length; i++)
        {
            hpImages[i].SetActive(false);
        }
    }

    public void SetMPImage(float curMP)
    {
        mpImage.fillAmount = curMP / PlayerController.Instance.stats.maxMP;
    }

    public void SetBossName()
    {
        bossNameGroup.SetActive(true);
    }

    public IEnumerator OnFirstHitTextCoroutine()
    {
        firstHitText.SetActive(true);
        yield return new WaitForSeconds(1f);
        firstHitText.SetActive(false);
    }
}
