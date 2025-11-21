using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using Articy.Pale_Rider;
using Articy.Pale_Rider.GlobalVariables;
using Articy.Unity;

public class MainMenu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public string sceneName;

    [Header("Audio")]
    [SerializeField]
    private AudioSource audioSource; // Reference to the AudioSource
    [SerializeField]
    private AudioClip clickSound; // Reference to the click sound effect

    [Header("Panels")]
    public GameObject mainMenuPanel;
    public GameObject loadGamePanel;
    public GameObject optionsPanel;
    public GameObject settingsPanel;
    public GameObject controlsPanel;

    void Start()
    {
        // Ensure the AudioSource is assigned
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
        mainMenuPanel.SetActive(true); // Hide the main menu panel at the start
        optionsPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!mainMenuPanel.activeSelf && !optionsPanel.activeSelf && !loadGamePanel.activeSelf)
        {
            if (Input.anyKeyDown)
            {
                mainMenuPanel.SetActive(true); // Show the main menu panel
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PlayClickSound();
            if (optionsPanel != null && optionsPanel.activeSelf)
            {
                optionsPanel.SetActive(false); // Hide the options panel
                mainMenuPanel.SetActive(true); // Show the main menu panel
            }
            else if (loadGamePanel != null && loadGamePanel.activeSelf)
            {
                loadGamePanel.SetActive(false); // Hide the load game panel
                mainMenuPanel.SetActive(true); // Show the main menu panel
            }
            else if (mainMenuPanel != null && mainMenuPanel.activeSelf)
            {
                mainMenuPanel.SetActive(false);
                Debug.Log("Main menu panel is active, hiding it.");
            }
            else
            {
                //Quit(); // Call the Quit method if not in options
            }
        }
    }

    public void NewGame()
    {
        ResetKeyVariables();
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
        if (optionsPanel != null)
        {
            optionsPanel.SetActive(!optionsPanel.activeSelf); // Toggle the options panel visibility
            mainMenuPanel.SetActive(false); // Hide the main menu panel
        }
    }

    public void Settings()
    {
        settingsPanel.SetActive(!settingsPanel.activeSelf); // Toggle the settings panel visibility
        controlsPanel.SetActive(false); // Hide the controls panel
    }

    public void Controls()
    {
        controlsPanel.SetActive(!controlsPanel.activeSelf); // Toggle the controls panel visibility
        settingsPanel.SetActive(false); // Hide the settings panel
    }

    public void PlayClickSound()
    {
        if (audioSource != null && clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }

    public void ResetKeyVariables()
    {
        // Access the default global variables instance and call ResetVariables()
        if (ArticyDatabase.DefaultGlobalVariables != null)
        {
            ArticyDatabase.DefaultGlobalVariables.ResetVariables();
            Debug.Log("All articy global variables have been reset to their default values.");
        }
        else
        {
            Debug.LogError("Articy database or default global variables not found!");
        }
    }
}
