using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeOut : MonoBehaviour
{
    [SerializeField] private Image panelToFade;
    [SerializeField] private float fadeDuration = 4f;
    [SerializeField] private bool fadeOnStart = true;  // Option to fade automatically on start

    private void Start()
    {
        // If no panel is assigned, try to get it from this GameObject
        if (panelToFade == null)
        {
            panelToFade = GetComponent<Image>();
        }

        if (fadeOnStart)
        {
            StartFadeOut();
        }
    }

    public void StartFadeOut()
    {
        if (panelToFade != null)
        {
            StartCoroutine(FadeOutRoutine());
        }
    }

    private IEnumerator FadeOutRoutine()
    {
        float elapsedTime = 0f;
        Color panelColor = panelToFade.color;
        panelColor.a = 1f;  // Ensure we start fully visible
        panelToFade.color = panelColor;

        // Fade from black to transparent
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            panelColor.a = 1f - (elapsedTime / fadeDuration);
            panelToFade.color = panelColor;
            yield return null;
        }

        // Ensure we're fully transparent
        panelColor.a = 0f;
        panelToFade.color = panelColor;
    }
} 