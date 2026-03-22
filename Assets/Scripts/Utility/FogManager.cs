using UnityEngine;

public class FogManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public float defaultFogDensity = 0.025f; // Default fog density (inspector-visible)
    public Color defaultFogColor = Color.white; // Default fog color (inspector-visible)

    public float fogDensity = 0.025f; // Fog density to apply when triggered
    public Color fogColor = Color.white; // Fog color to apply when triggered

    // If non-empty, only colliders with this tag will trigger the fog change.
    // Leave empty to allow any collider to trigger.
    public string triggerTag = "Player";

    // Whether to restore the original RenderSettings when exiting the trigger.
    public bool restoreOnExit = true;

    // Stored original scene fog settings so we can restore them on exit
    private bool originalFogEnabled;
    private float originalFogDensity;
    private Color originalFogColor;
    // Transition settings
    public float transitionDuration = 1.0f;
    public bool useUnscaledTime = false;

    private Coroutine transitionCoroutine;

    void Start()
    {
        originalFogEnabled = RenderSettings.fog;
        originalFogDensity = RenderSettings.fogDensity;
        originalFogColor = RenderSettings.fogColor;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!string.IsNullOrEmpty(triggerTag) && !other.CompareTag(triggerTag)) return;
        StartFogTransition(fogDensity, fogColor, true);
    }

    void OnTriggerExit(Collider other)
    {
        if (!string.IsNullOrEmpty(triggerTag) && !other.CompareTag(triggerTag)) return;
        if (restoreOnExit) StartFogTransition(originalFogDensity, originalFogColor, false);
    }

    void OnTriggerEnter2D(UnityEngine.Collider2D other)
    {
        if (!string.IsNullOrEmpty(triggerTag) && !other.CompareTag(triggerTag)) return;
        StartFogTransition(fogDensity, fogColor, true);
    }

    void OnTriggerExit2D(UnityEngine.Collider2D other)
    {
        if (!string.IsNullOrEmpty(triggerTag) && !other.CompareTag(triggerTag)) return;
        if (restoreOnExit) StartFogTransition(originalFogDensity, originalFogColor, false);
    }

    private void StartFogTransition(float targetDensity, Color targetColor, bool enableFogImmediately)
    {
        if (transitionCoroutine != null) StopCoroutine(transitionCoroutine);
        if (enableFogImmediately) RenderSettings.fog = true;
        transitionCoroutine = StartCoroutine(TransitionCoroutine(targetDensity, targetColor, enableFogImmediately));
    }

    private System.Collections.IEnumerator TransitionCoroutine(float targetDensity, Color targetColor, bool enableFogImmediately)
    {
        float startDensity = RenderSettings.fogDensity;
        Color startColor = RenderSettings.fogColor;
        float elapsed = 0f;
        float dur = Mathf.Max(0.0001f, transitionDuration);
        while (elapsed < dur)
        {
            float delta = useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
            elapsed += delta;
            float t = Mathf.Clamp01(elapsed / dur);
            RenderSettings.fogDensity = Mathf.Lerp(startDensity, targetDensity, t);
            RenderSettings.fogColor = Color.Lerp(startColor, targetColor, t);
            yield return null;
        }

        RenderSettings.fogDensity = targetDensity;
        RenderSettings.fogColor = targetColor;

        // If we were restoring and the original scene had fog disabled, turn it off now.
        if (!enableFogImmediately && !originalFogEnabled)
        {
            RenderSettings.fog = originalFogEnabled;
        }

        transitionCoroutine = null;
    }
}
