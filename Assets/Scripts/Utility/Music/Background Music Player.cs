using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;
using Articy.Pale_Rider;
using UnityEngine.UI;

public class BackgroundMusicPlayer : MonoBehaviour
{
    public AudioClip[] musicClips; // Array to hold different music clips for different scenes
    public string currentClip; // Currently playing music clip
    public string sceneName;
    private AudioSource audioSource;

    // Static reference to our singleton instance
    private static BackgroundMusicPlayer s_instance;

    void Awake()
    {
        // If this is the first instance, make it the singleton
        if (s_instance == null)
        {
            s_instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }

        // If this isn't the first instance, destroy this one to prevent duplicates
        // This ensures the original music player continues playing
        if (s_instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Only proceed with setup if this is the surviving instance
        if (s_instance == this)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Only proceed if this is the surviving instance
        if (s_instance != this) return;

        sceneName = SceneManager.GetActiveScene().name;
        if (audioSource != null && audioSource.clip != null)
        {
            currentClip = audioSource.clip.ToString();
            SceneCheck();
        }
    }

    public void SceneCheck()
    {
        if (sceneName != "Altamesa Saloon" && sceneName != "Altamesa Saloon 2")
        {
            if (currentClip == "Aurora - Hans Zimmer")
            {
                audioSource.Stop();
            }
            else
            {
                Debug.Log("Current clip: " + currentClip);
            }
        }
    }

    public void ChangeSong(string clipName)
    {
        foreach (AudioClip clip in musicClips)
        {
            if (clip.name == clipName)
            {
                audioSource.clip = clip;
                audioSource.Play();
                Debug.Log("Changed song to: " + clipName);
                return;
            }
        }
        Debug.LogWarning("Clip not found: " + clipName);
    }

    // Public method to check if this is the main instance
    public static BackgroundMusicPlayer GetInstance()
    {
        return s_instance;
    }
}
