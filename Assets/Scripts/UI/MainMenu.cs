using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using Articy.Pale_Rider;
using Articy.Pale_Rider.GlobalVariables;
using Articy.Unity;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private const string PREF_SKILL_SFX = "SkillSFXEnabled";
    private const string PREF_SKILL_VOICES = "SkillVoicesEnabled";

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

    [Header("Settings")]
    public GameObject targettedCheckbox;
    public Sprite emptyCheckbox;
    public Sprite fullCheckbox;
    public bool skillSFXEnabled = true;
    public bool skillVoicesEnabled = true;
    public Image skillSFXCheckbox;  // Add reference to the SFX checkbox Image
    public Image skillVoicesCheckbox;  // Add reference to the Voices checkbox Image
    public GameObject skillSFXButton;  // Add reference to the SFX toggle button
    public GameObject skillVoicesButton;  // Add reference to the Voices toggle button

    void Start()
    {
        // Ensure the AudioSource is assigned
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        // Load saved settings from PlayerPrefs (default to current serialized values)
        skillSFXEnabled = PlayerPrefs.GetInt(PREF_SKILL_SFX, skillSFXEnabled ? 1 : 0) == 1;
        skillVoicesEnabled = PlayerPrefs.GetInt(PREF_SKILL_VOICES, skillVoicesEnabled ? 1 : 0) == 1;

        // Update Serialized checkbox images to match loaded state
        UpdateCheckboxSprite(skillSFXCheckbox, skillSFXEnabled);
        UpdateCheckboxSprite(skillVoicesCheckbox, skillVoicesEnabled);

        mainMenuPanel.SetActive(true); // Hide the main menu panel at the start
        optionsPanel.SetActive(false);

        if (skillSFXEnabled)
            skillSFXButton.GetComponent<Image>().sprite = fullCheckbox;
        else
            skillSFXButton.GetComponent<Image>().sprite = emptyCheckbox;

        if (skillVoicesEnabled)
            skillVoicesButton.GetComponent<Image>().sprite = fullCheckbox;
        else
            skillVoicesButton.GetComponent<Image>().sprite = emptyCheckbox;
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
        // Reset any key variables or states here before starting a new game
        // For example, you might want to reset player stats, inventory, etc.
        Debug.Log("Resetting key variables for a new game.");
    }

    public void ToggleSkillSFX()
    {
        skillSFXEnabled = !skillSFXEnabled;

        // Use the UI button that was pressed as the targeted checkbox
        GameObject selected = EventSystem.current != null ? EventSystem.current.currentSelectedGameObject : null;
        if (selected != null)
            targettedCheckbox = selected;
        PlayClickSound();

        // Prefer the button's Image; fall back to the serialized reference if needed
        Image checkboxImage = null;
        if (targettedCheckbox != null)
            checkboxImage = targettedCheckbox.GetComponent<Image>() ?? targettedCheckbox.GetComponentInChildren<Image>();
        if (checkboxImage == null)
            checkboxImage = skillSFXCheckbox;

        UpdateCheckboxSprite(checkboxImage, skillSFXEnabled);

        // Save preference
        PlayerPrefs.SetInt(PREF_SKILL_SFX, skillSFXEnabled ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void ToggleSkillVoices()
    {
        skillVoicesEnabled = !skillVoicesEnabled;

        // Use the UI button that was pressed as the targeted checkbox
        GameObject selected = EventSystem.current != null ? EventSystem.current.currentSelectedGameObject : null;
        if (selected != null)
            targettedCheckbox = selected;
        PlayClickSound();

        // Prefer the button's Image; fall back to the serialized reference if needed
        Image checkboxImage = null;
        if (targettedCheckbox != null)
            checkboxImage = targettedCheckbox.GetComponent<Image>() ?? targettedCheckbox.GetComponentInChildren<Image>();
        if (checkboxImage == null)
            checkboxImage = skillVoicesCheckbox;

        UpdateCheckboxSprite(checkboxImage, skillVoicesEnabled);

        // Save preference
        PlayerPrefs.SetInt(PREF_SKILL_VOICES, skillVoicesEnabled ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void UpdateCheckboxSprite(Image checkboxImage, bool isEnabled)
    {
        if (checkboxImage != null)
        {
            checkboxImage.sprite = isEnabled ? fullCheckbox : emptyCheckbox;
        }
    }
}
