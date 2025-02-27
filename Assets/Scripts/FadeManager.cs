using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class FadeManager : MonoBehaviour
{
    [SerializeField] private Image fadePanel; // Assign a black UI Image that covers the screen
    [SerializeField] private float fadeDuration = 4f;
    [SerializeField] private string sceneToLoad = "GameScene";

    private void Start()
    {
        // Ensure panel starts transparent
        if (fadePanel != null)
        {
            Color startColor = fadePanel.color;
            startColor.a = 0f;
            fadePanel.color = startColor;
        }
    }

    public void StartFadeAndLoad()
    {
        StartCoroutine(FadeAndLoadScene());
    }

    private IEnumerator FadeAndLoadScene()
    {
        float elapsedTime = 0f;
        Color panelColor = fadePanel.color;

        // Fade from transparent to black
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            panelColor.a = elapsedTime / fadeDuration;
            fadePanel.color = panelColor;
            yield return null;
        }

        // Ensure we're fully black
        panelColor.a = 1f;
        fadePanel.color = panelColor;

        // Load the new scene
        SceneManager.LoadScene(sceneToLoad);
    }
} 