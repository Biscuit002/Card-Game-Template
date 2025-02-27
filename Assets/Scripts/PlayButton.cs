using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections;

public class PlayButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 originalScale;
    private Vector3 targetScale;
    private float smoothSpeed = 5f;
    [SerializeField] private float transitionDelay = 1.0f; // Set delay time in inspector
    [SerializeField] private string sceneToLoad = "GameScene"; // Scene name to load

    void Start()
    {
        originalScale = transform.localScale;
        targetScale = originalScale;
    }

    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, smoothSpeed * Time.deltaTime);
    }

    public void OnPlayButtonClick()
    {
        // Start the coroutine for delayed scene load
        StartCoroutine(LoadSceneWithDelay());
    }
    
    private IEnumerator LoadSceneWithDelay()
    {
        // Wait for specified delay
        yield return new WaitForSeconds(transitionDelay);
        
        // Load the scene
        SceneManager.LoadScene(sceneToLoad);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        targetScale = originalScale * 1.2f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetScale = originalScale;
    }
}
