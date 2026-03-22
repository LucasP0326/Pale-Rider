using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicFadeController : MonoBehaviour
{
    [Tooltip("Tag used to identify the player object")]
    public string playerTag = "Player";

    [Tooltip("Seconds it takes to fade in/out")]
    public float fadeDuration = 1f;

    [Tooltip("Target volume when faded in")]
    [Range(0f, 1f)]
    public float targetVolume = 1f;

    private Dictionary<AudioSource, Coroutine> fadeCoroutines = new Dictionary<AudioSource, Coroutine>();
    private Dictionary<AudioSource, float> originalVolumes = new Dictionary<AudioSource, float>();

    void Awake()
    {
        Collider[] cols = GetComponentsInChildren<Collider>(true);
        foreach (var col in cols)
        {
            if (!col.isTrigger) continue;
            var audio = col.GetComponent<AudioSource>();
            if (audio == null) continue;
            // record inspector-set volume and ensure audio starts muted on game start
            float origVol = audio.volume;
            originalVolumes[audio] = origVol;
            audio.volume = 0f;
            if (audio.isPlaying) audio.Stop();
            var reporter = col.gameObject.GetComponent<TriggerReporter>();
            if (reporter == null) reporter = col.gameObject.AddComponent<TriggerReporter>();
            reporter.SetController(this);
            reporter.audioSource = audio;
        }
    }

    internal void PlayerEnteredTrigger(AudioSource source)
    {
        if (source == null) return;
        float to = targetVolume;
        if (originalVolumes.TryGetValue(source, out var orig)) to = orig;
        StartFade(source, to, fadeDuration, true);
    }

    internal void PlayerExitedTrigger(AudioSource source)
    {
        if (source == null) return;
        StartFade(source, 0f, fadeDuration, false);
    }

    void StartFade(AudioSource source, float toVolume, float duration, bool ensurePlaying)
    {
        if (fadeCoroutines.TryGetValue(source, out var running) && running != null)
        {
            StopCoroutine(running);
            fadeCoroutines.Remove(source);
        }
        var c = StartCoroutine(FadeCoroutine(source, toVolume, duration, ensurePlaying));
        fadeCoroutines[source] = c;
    }

    IEnumerator FadeCoroutine(AudioSource source, float toVolume, float duration, bool ensurePlaying)
    {
        if (ensurePlaying && !source.isPlaying) source.Play();
        float start = source.volume;
        if (duration <= 0f)
        {
            source.volume = toVolume;
            if (toVolume <= 0.0001f) source.Stop();
            yield break;
        }
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            source.volume = Mathf.Lerp(start, toVolume, t);
            yield return null;
        }
        source.volume = toVolume;
        if (toVolume <= 0.0001f) source.Stop();
        fadeCoroutines.Remove(source);
    }

    [DisallowMultipleComponent]
    private class TriggerReporter : MonoBehaviour
    {
        public AudioSource audioSource;
        private MusicFadeController controller;

        public void SetController(MusicFadeController c)
        {
            controller = c;
        }

        void OnTriggerEnter(Collider other)
        {
            if (controller == null) return;
            if (!other.CompareTag(controller.playerTag)) return;
            controller.PlayerEnteredTrigger(audioSource);
        }

        void OnTriggerExit(Collider other)
        {
            if (controller == null) return;
            if (!other.CompareTag(controller.playerTag)) return;
            controller.PlayerExitedTrigger(audioSource);
        }
    }
}
