using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManagerController : MonoBehaviour
{
    public static SceneManagerController Instance { get; private set; }

    public Image panel;
    public float fadeDuration = 1.0f;
    public string nextSceneName;
    public bool isFading = false;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartSceneTransition(string sceneName)
    {
        if (!isFading)
        {
            nextSceneName = sceneName;
            Time.timeScale = 1;
            StartCoroutine(FadeInAndLoadScene());
        }
    }

    IEnumerator FadeInAndLoadScene()
    {
        isFading = true;
        yield return StartCoroutine(FadeImage(0, 1, fadeDuration));

        yield return StartCoroutine(LoadLoadingAndNextScene());

        yield return StartCoroutine(FadeImage(1, 0, fadeDuration));

        isFading = false;
    }

    IEnumerator FadeImage(float startAlpha, float endAlpha, float duration)
    {
        float elapsedTime = 0f;
        Color panelColor = panel.color;

        while(elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            panelColor.a = newAlpha;
            panel.color = panelColor;
            yield return null;
        }

        panelColor.a = endAlpha;
        panel.color = panelColor;
    }

    IEnumerator LoadLoadingAndNextScene()
    {
        AsyncOperation nextSceneOp = SceneManager.LoadSceneAsync(nextSceneName);
        while (!nextSceneOp.isDone)
        {
            yield return null;
        }

    }

    public void ExitScene()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
    }
}
