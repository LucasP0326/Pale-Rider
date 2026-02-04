using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Hallucination2FadeIn : MonoBehaviour
{
    public Image fadeImage; // Assign your canvas image in the inspector
    private float fadeDuration = 3f;

    void Start()
    {
        StartCoroutine(FadeIn());
    }

    void Update()
    {
        
    }

    IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        Color imageColor = fadeImage.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            imageColor.a = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            fadeImage.color = imageColor;
            yield return null;
        }

        imageColor.a = 0f;
        fadeImage.color = imageColor;
    }
}
