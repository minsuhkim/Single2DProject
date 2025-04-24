using UnityEngine;
using UnityEngine.UI;

public class OptionManager : MonoBehaviour
{
    public Slider bgmSlider;
    public Slider sfxSlider;

    public GameObject menuGroup;
    public GameObject optionGroup;


    private void Start()
    {
        bgmSlider.value = PlayerPrefs.GetFloat("BGMVolume", 1);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1);
    }

    private void Update()
    {
        SoundManager.Instance.bgmSource.volume = bgmSlider.value;
        PlayerPrefs.SetFloat("BGMVolume", bgmSlider.value);
        SoundManager.Instance.sfxSource.volume = sfxSlider.value;
        PlayerPrefs.SetFloat("SFXVolume", sfxSlider.value);
    }

    public void OnOptionGroup()
    {
        menuGroup.SetActive(false);
        optionGroup.SetActive(true);
    }

    public void OnReturnButtonClick()
    {
        menuGroup.SetActive(true);
        optionGroup.SetActive(false);
    }
}
