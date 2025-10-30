using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;
using Articy.Pale_Rider;
using UnityEngine.UI;

public class LeavePaleOpeningSequence : MonoBehaviour
{
    public AudioSource paleRiderIntroMusic;
    public TextMeshProUGUI paleRiderIntroText;
    public GameObject fadeToBlack;
    public GameObject saloon;
    public string nextSceneName;
    public GameObject hud;

    [Tooltip("Seconds it takes for the fade-to-black to go from 0 to 1")]
    public float fadeDuration = 5f;

    [Tooltip("Seconds to wait after the text disappears before starting the unfade")]
    public float unfadeDelayAfterText = 5f;

    // persistent music singleton (prevents duplicates across scenes)
    private static AudioSource s_persistentMusic;

    // persistent fade object & controller (used only if we load a new scene and want to unfade there)
    private static GameObject s_persistentFade;
    private static FadeController s_persistentFadeController;

    void Start()
    {
    }

    void Update()
    {
        {
            string sceneName = SceneManager.GetActiveScene().name;
            if (sceneName != "Altamesa" && sceneName != "Zureton" && sceneName != "End Scene")
            {
                Destroy(gameObject);
            }
        }
    }

    public void triggerIntro()
    {
        Debug.Log("Triggering Pale Rider Intro Sequence");

        // Ensure music persists across scene load and avoid duplicates
        MakeMusicPersistent();

        StartCoroutine(playIntroSequence());
    }

    private void MakeMusicPersistent()
    {
        if (paleRiderIntroMusic == null)
            return;

        // If no persistent music exists yet, make this one persistent
        if (s_persistentMusic == null)
        {
            s_persistentMusic = paleRiderIntroMusic;
            DontDestroyOnLoad(s_persistentMusic.gameObject);
        }
        else
        {
            // If a different music object is already persistent, destroy this duplicate to avoid multiple tracks.
            if (s_persistentMusic != paleRiderIntroMusic)
            {
                if (paleRiderIntroMusic.gameObject != s_persistentMusic.gameObject)
                {
                    Destroy(paleRiderIntroMusic.gameObject);
                    paleRiderIntroMusic = s_persistentMusic;
                }
            }
        }
    }

    /// <summary>
    /// Make the fadeToBlack object persistent (DontDestroyOnLoad) and ensure it has a FadeController.
    /// </summary>
    private void MakeFadePersistent()
    {
        if (fadeToBlack == null)
            return;

        if (s_persistentFade == null)
        {
            s_persistentFade = fadeToBlack;
            DontDestroyOnLoad(s_persistentFade);
            s_persistentFadeController = s_persistentFade.GetComponent<FadeController>();
            if (s_persistentFadeController == null)
                s_persistentFadeController = s_persistentFade.AddComponent<FadeController>();
        }
        else
        {
            // If there is already a persistent fade object, destroy the local duplicate to avoid duplicates
            if (s_persistentFade != fadeToBlack)
            {
                Destroy(fadeToBlack);
                fadeToBlack = s_persistentFade;
                s_persistentFadeController = s_persistentFade.GetComponent<FadeController>();
            }
        }
    }

    public IEnumerator playIntroSequence()
    {
        if (paleRiderIntroMusic != null && !paleRiderIntroMusic.isPlaying)
            paleRiderIntroMusic.Play();

        saloon.SetActive(false);

        // Wait before showing the text (existing timing)
        yield return new WaitForSeconds(4f);

        // Show the intro text
        if (paleRiderIntroText != null)
        {
            hud.SetActive(false);
            paleRiderIntroText.gameObject.SetActive(true);
        }

        // Start fading to black immediately after the text appears (runs in parallel)
        if (fadeToBlack != null)
            StartCoroutine(FadeToBlackRoutine(fadeDuration));

        // Keep the text visible for the original 10 seconds
        yield return new WaitForSeconds(10f);

        if (paleRiderIntroText != null)
            paleRiderIntroText.gameObject.SetActive(false);

        // After the text disappears: start unfade after unfadeDelayAfterText seconds.
        // If we are changing scenes, make the fade persistent so the unfade can run in the new scene.
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            // Make fade persistent so we can unfade after the scene loads
            MakeFadePersistent();

            Debug.Log($"Loading next scene: {nextSceneName}");
            SceneManager.LoadScene(nextSceneName);

            // Start the unfade on the persistent fade controller (it persists across scenes)
            if (s_persistentFadeController != null)
            {
                s_persistentFadeController.StartUnfadeWithDelay(unfadeDelayAfterText, fadeDuration, true);
            }
            else
            {
                Debug.LogWarning("Persistent fade controller missing; cannot start unfade in new scene.");
            }
        }
        else
        {
            // No scene change: just unfade in the current scene after the configured delay
            hud.SetActive(true);
            StartCoroutine(UnfadeRoutine(unfadeDelayAfterText, fadeDuration));
        }
    }

    private IEnumerator FadeToBlackRoutine(float duration)
    {
        if (fadeToBlack == null)
            yield break;

        // Try Image first
        Image img = fadeToBlack.GetComponent<Image>();
        CanvasGroup cg = null;

        // If there is no Image, try CanvasGroup as fallback
        if (img == null)
        {
            cg = fadeToBlack.GetComponent<CanvasGroup>();
            if (cg == null)
            {
                Debug.LogWarning("FadeToBlack GameObject needs an Image or CanvasGroup component to fade.");
                yield break;
            }
        }

        // Ensure object is active
        fadeToBlack.SetActive(true);

        float elapsed = 0f;

        // initialize alpha to 0
        if (img != null)
        {
            Color c = img.color;
            c.a = 0f;
            img.color = c;
        }
        else
        {
            cg.alpha = 0f;
            cg.blocksRaycasts = true;
            cg.interactable = false;
        }

        // Fade loop
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            if (img != null)
            {
                Color c = img.color;
                c.a = Mathf.Lerp(0f, 1f, t);
                img.color = c;
            }
            else
            {
                cg.alpha = Mathf.Lerp(0f, 1f, t);
            }
            yield return null;
        }

        // Ensure fully opaque at end
        if (img != null)
        {
            Color c = img.color;
            c.a = 1f;
            img.color = c;
        }
        else
        {
            cg.alpha = 1f;
        }
    }

    /// <summary>
    /// Unfade in the current scene (non-persistent path).
    /// </summary>
    private IEnumerator UnfadeRoutine(float delaySeconds, float duration)
    {
        if (fadeToBlack == null)
            yield break;

        yield return new WaitForSeconds(delaySeconds);

        Image img = fadeToBlack.GetComponent<Image>();
        CanvasGroup cg = null;

        if (img == null)
        {
            cg = fadeToBlack.GetComponent<CanvasGroup>();
            if (cg == null)
            {
                Debug.LogWarning("FadeToBlack GameObject needs an Image or CanvasGroup component to unfade.");
                yield break;
            }
        }

        float elapsed = 0f;

        // Ensure starting alpha is 1
        if (img != null)
        {
            Color c = img.color;
            c.a = 1f;
            img.color = c;
        }
        else
        {
            cg.alpha = 1f;
        }

        // Unfade loop (1 -> 0)
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float alpha = Mathf.Lerp(1f, 0f, t);
            if (img != null)
            {
                Color c = img.color;
                c.a = alpha;
                img.color = c;
            }
            else
            {
                cg.alpha = alpha;
            }
            yield return null;
        }

        // Ensure fully transparent, then disable and (optionally) destroy the object
        if (img != null)
        {
            Color c = img.color;
            c.a = 0f;
            img.color = c;
        }
        else
        {
            cg.alpha = 0f;
        }

        fadeToBlack.SetActive(false);
    }
}

/// <summary>
/// Small helper component that can live on the fade GameObject (and be DontDestroyOnLoad)
/// to run unfade after a delay across a scene load.
/// </summary>
public class FadeController : MonoBehaviour
{
    private Image _img;
    private CanvasGroup _cg;
    private Coroutine _running;

    void Awake()
    {
        _img = GetComponent<Image>();
        _cg = GetComponent<CanvasGroup>();
    }

    /// <summary>
    /// Starts an unfade after a delay, then optionally destroys the GameObject when complete.
    /// </summary>
    public void StartUnfadeWithDelay(float delaySeconds, float duration, bool destroyOnComplete)
    {
        if (_running != null)
            StopCoroutine(_running);

        _running = StartCoroutine(UnfadeCoroutine(delaySeconds, duration, destroyOnComplete));
    }

    private IEnumerator UnfadeCoroutine(float delaySeconds, float duration, bool destroyOnComplete)
    {
        yield return new WaitForSeconds(delaySeconds);

        // Ensure starting alpha is 1
        if (_img != null)
        {
            var c = _img.color;
            c.a = 1f;
            _img.color = c;
        }
        else if (_cg != null)
        {
            _cg.alpha = 1f;
        }
        else
        {
            Debug.LogWarning("FadeController requires an Image or CanvasGroup on the GameObject.");
            yield break;
        }

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float alpha = Mathf.Lerp(1f, 0f, t);
            if (_img != null)
            {
                var c = _img.color;
                c.a = alpha;
                _img.color = c;
            }
            else
            {
                _cg.alpha = alpha;
            }
            yield return null;
        }

        // Ensure transparent
        if (_img != null)
        {
            var c = _img.color;
            c.a = 0f;
            _img.color = c;
        }
        else
        {
            _cg.alpha = 0f;
        }

        // Disable the object so it doesn't block UI events anymore
        gameObject.SetActive(false);

        if (destroyOnComplete)
        {
            Destroy(gameObject);
        }
    }
}
