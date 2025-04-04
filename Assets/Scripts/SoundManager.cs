using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioSource bgmSource;
    public AudioSource sfxSource;

    public Dictionary<BGMType, AudioClip> bgmDict = new Dictionary<BGMType, AudioClip>();
    public Dictionary<SFXType, AudioClip> sfxDict = new Dictionary<SFXType, AudioClip>();

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        SetBGMVolume(0.15f);
    }

    // ���� ���� �� �ڵ����� ����Ǵ� �ʱ�ȭ �Լ�
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void InitSoundManager()
    {
        GameObject obj = new GameObject("SoundManager");
        Instance = obj.AddComponent<SoundManager>();
        DontDestroyOnLoad(obj);

        //bgm
        GameObject bgmObj = new GameObject("BGM");
        SoundManager.Instance.bgmSource = bgmObj.AddComponent<AudioSource>();
        bgmObj.transform.SetParent(obj.transform);
        SoundManager.Instance.bgmSource.loop = true;
        SoundManager.Instance.bgmSource.volume = PlayerPrefs.GetFloat("BGMVolume", 1f);


        //sfx
        GameObject sfxObj = new GameObject("SFX");
        SoundManager.Instance.sfxSource = sfxObj.AddComponent<AudioSource>();
        SoundManager.Instance.sfxSource.volume = PlayerPrefs.GetFloat("SFXVolume", 1.0f);
        sfxObj.transform.SetParent(obj.transform);

        AudioClip[] bgmClips = Resources.LoadAll<AudioClip>("Sound/BGM");
        foreach(var clip in bgmClips)
        {
            try
            {
                BGMType type = (BGMType)Enum.Parse(typeof(BGMType), clip.name);
                SoundManager.Instance.bgmDict.Add(type, clip);
            }
            catch
            {
                Debug.LogWarning("BGM Enum �ʿ� : " + clip.name);
            }
        }

        AudioClip[] sfxClips = Resources.LoadAll<AudioClip>("Sound/SFX");

        foreach (var clip in sfxClips)
        {
            try
            {
                SFXType type = (SFXType)Enum.Parse(typeof(SFXType), clip.name);
                SoundManager.Instance.sfxDict.Add(type, clip);
            }
            catch
            {
                Debug.LogWarning("SFX Enum �ʿ� : " + clip.name);
            }
        }

        SceneManager.sceneLoaded += SoundManager.Instance.OnSceneLoadCompleted;
    }

    // Scene Loading�� �Ϸ�Ǿ��� �� �����ų �Լ�(BGM ����)
    public void OnSceneLoadCompleted(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "Tutorial")
        {
            PlayBGM(BGMType.GeneralBGM, 1f);
        }
        else if(scene.name == "Boss")
        {
            PlayBGM(BGMType.BossBGM, 1f);
        }
    }

    // SFX Play
    public void PlaySFX(SFXType type) //ȿ���� ���
    {
        // sfxDict�� �ش� sfx�� ������ ���� x
        if (!sfxDict.ContainsKey(type))
        {
            return;
        }
        sfxSource.PlayOneShot(sfxDict[type]);
    }

    // BGM Play
    public void PlayBGM(BGMType type, float fadeTime = 0f)
    {
        // bgmDict�� �ش� bgm�� ������ ���� x
        if (!bgmDict.ContainsKey(type))
        {
            return;
        }

        // BGM Source�� �̹� Clip�� ����
        if(bgmSource.clip != null)
        {
            // �̹� �����ϴ� clip�� ���� �����Ϸ��� clip
            if(bgmSource.clip.name == type.ToString())
            {
                return;
            }
            // fade�� �������� ������ �ٷ� ��ȯ
            if(fadeTime == 0)
            {
                bgmSource.clip = bgmDict[type];
                bgmSource.Play();
            }
            // fade ����
            else
            {
                StartCoroutine(FadeOutBGM (() =>
                {
                    bgmSource.clip = bgmDict[type];
                    bgmSource.Play();
                    StartCoroutine(FadeInBGM(fadeTime));
                }, fadeTime));
            }
        }
        // BGM Source�� Ŭ���� ����
        else
        {
            // fade�� �������� ������ �ٷ� ����
            if (fadeTime == 0)
            {
                bgmSource.clip = bgmDict[type];
                bgmSource.Play();
            }
            // FadeIn�� ����(FadeOut�� Clip�� �������� �ʾƼ�)
            else
            {
                bgmSource.volume = 0;
                bgmSource.clip = bgmDict[type];
                bgmSource.Play();
                StartCoroutine(FadeInBGM(fadeTime));
            }
        }
    }

    // bgm volume�� ������ ������ FadeInBGM �Լ� ���� ����
    private IEnumerator FadeOutBGM(Action onComplete, float duration = 1.0f)
    {
        float startVolume = bgmSource.volume;
        float time = 0;

        while(time < duration)
        {
            bgmSource.volume = Mathf.Lerp(startVolume, 0f, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        bgmSource.volume = 0f;
        onComplete?.Invoke();
    }

    // bgm volume�� ������ �ø�
    private IEnumerator FadeInBGM(float duration = 1.0f)
    {
        float targetVolume = PlayerPrefs.GetFloat("BGMVolume", 1.0f);
        float time = 0f;

        while(time < duration)
        {
            bgmSource.volume = Mathf.Lerp(0f, targetVolume, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        bgmSource.volume = targetVolume;
    }

    // bgm volume ����
    public void SetBGMVolume(float volume)
    {
        bgmSource.volume = volume;
        PlayerPrefs.SetFloat("BGMVolume", volume);
    }

    // sfx volume ����
    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume;
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }
}

public enum BGMType
{
    GeneralBGM,
    BossBGM,
    BossBattleBGM
}

public enum SFXType
{
    Attack,
    Coin,
    Dash,
    Jump,
    Slide,
    Walk,
    Hit,
    Parrying
}
