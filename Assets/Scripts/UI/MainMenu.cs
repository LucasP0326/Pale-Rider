using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class MainMenu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public string sceneName;

    [Header("Audio")]
    [SerializeField]
    private AudioSource audioSource; // Reference to the AudioSource
    [SerializeField]
    private AudioClip clickSound; // Reference to the click sound effect
    void Start()
    {
        // Ensure the AudioSource is assigned
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NewGame()
    {
        PlayClickSound();
        SceneManager.LoadScene(sceneName);
    }

    public void Quit()
    {
        PlayClickSound();
        Debug.Log("Quit called - exiting application.");
        #if UNITY_EDITOR
        // Simulate quitting in the Unity Editor
        Debug.Log("Quit called - exiting play mode in the Editor.");
        EditorApplication.isPlaying = false;
        #else
        // Quit the application in a build
        Application.Quit();
        #endif
    }

    public void LoadGame()
    {
        PlayClickSound();
        // Implement your load game logic here
        Debug.Log("Load Game called - implement your load game logic here.");
    }

    public void Options()
    {
        PlayClickSound();
        // Implement your options logic here
        Debug.Log("Options called - implement your options logic here.");
    }

    public void PlayClickSound()
    {
        if (audioSource != null && clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }
}
