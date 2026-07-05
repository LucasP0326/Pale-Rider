using UnityEngine;
using UnityEngine.UI;
using StarterAssets;
using System.Collections;
using UnityEngine.SceneManagement;
using Articy.Unity;
using Articy.Unity.Interfaces;
using Articy.Pale_Rider;
using Articy.Pale_Rider.GlobalVariables;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    private const string PREF_SKILL_SFX = "SkillSFXEnabled";
    private const string PREF_SKILL_VOICES = "SkillVoicesEnabled";

    public GameObject player;
    public string mainMenuName;
    public GameObject optionsPanel;
    public GameObject mainPanel;

    //Important References
    private InventoryManager inventoryManager;
    private SaveManager saveManager;
    public bool optionsOpen = false;

    [Header("Audio")]
    [SerializeField]
    private AudioSource audioSource; // Reference to the AudioSource
    [SerializeField]
    private AudioClip clickSound; // Reference to the click sound effect

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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        inventoryManager = FindFirstObjectByType<InventoryManager>();
        saveManager = FindFirstObjectByType<SaveManager>();

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
    }

    public void Intro()
    {
        Debug.Log("Glide in Here!");
    }

    public void Resume()
    {
        player.GetComponent<ThirdPersonController>().Pause2();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(mainMenuName);
    }

    public void SaveGame()
    {
        saveManager.SaveGame();
    }

    public void LoadGame()
    {
        saveManager.LoadGame();
    }

    public void ResetGame()
    {
        saveManager.ResetGame();
    }

    public void Options()
    {
        optionsPanel.SetActive(true);
        optionsOpen = true;
        mainPanel.SetActive(false);
    }

    public void CloseOptions()
    {
        optionsOpen = false;
        optionsPanel.SetActive(false);
        mainPanel.SetActive(true);
    }

    public void PlayClickSound()
    {
        if (audioSource != null && clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
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
