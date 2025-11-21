using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public struct SongEntry
{
    [Tooltip("Optional label for this entry")]
    public string label;

    [Tooltip("Audio clip to play for the listed scenes")]
    public AudioClip clip;

    [Tooltip("List of scene names where this clip should play (exact match, case-insensitive)")]
    public string[] sceneNames;
}

public class MusicPlayer : MonoBehaviour
{
    [Tooltip("Add song entries here. Each entry contains an AudioClip and an expandable list of scene names.")]
    public List<SongEntry> songs = new List<SongEntry>();

    [Tooltip("If true, stop playing when a loaded scene doesn't match any entry.")]
    public bool stopWhenNoMatch = false;

    private AudioSource _audioSource;
    private static MusicPlayer s_instance;

    void Awake()
    {
        // Keep only one persistent music player (first created wins).
        if (s_instance == null)
        {
            s_instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (s_instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start()
    {
        if (s_instance != this) return;

        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
            _audioSource = gameObject.AddComponent<AudioSource>();

        // Apply music for the currently active scene on startup
        ApplyMusicForScene(SceneManager.GetActiveScene().name, true);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (s_instance != this) return;
        ApplyMusicForScene(scene.name, true);
    }

    private void ApplyMusicForScene(string sceneName, bool playImmediately = true)
    {
        if (_audioSource == null) return;

        // Find first entry that matches this scene (case-insensitive exact match)
        foreach (var entry in songs)
        {
            if (entry.clip == null || entry.sceneNames == null) continue;

            foreach (var s in entry.sceneNames)
            {
                if (string.IsNullOrEmpty(s)) continue;
                if (string.Equals(s.Trim(), sceneName, System.StringComparison.OrdinalIgnoreCase))
                {
                    if (_audioSource.clip != entry.clip)
                    {
                        _audioSource.clip = entry.clip;
                        if (playImmediately)
                            _audioSource.Play();
                    }
                    return; // first match wins
                }
            }
        }

        // No matching entry found
        if (stopWhenNoMatch)
        {
            _audioSource.Stop();
            _audioSource.clip = null;
        }
    }

    // Optional helper to change song by clip name at runtime
    public bool ChangeSongByClipName(string clipName)
    {
        if (string.IsNullOrEmpty(clipName)) return false;

        foreach (var entry in songs)
        {
            if (entry.clip == null) continue;
            if (string.Equals(entry.clip.name, clipName, System.StringComparison.OrdinalIgnoreCase))
            {
                if (_audioSource == null) _audioSource = GetComponent<AudioSource>();
                _audioSource.clip = entry.clip;
                _audioSource.Play();
                return true;
            }
        }
        return false;
    }

    // Expose the instance if other scripts need to query it
    public static MusicPlayer GetInstance() => s_instance;
}
