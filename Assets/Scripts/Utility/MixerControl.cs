using UnityEngine;
using UnityEngine.Audio;

public class MixerControl : MonoBehaviour
{
    public GameManager gameManager; // Reference to the GameManager script
    [SerializeField] private AudioMixer audioMixer; // Reference to the AudioMixer
    public string groupName;

    private string playerPrefKey; // Key for saving and loading the volume setting

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>(); // Find the GameManager in the scene

        // Generate a unique PlayerPrefs key for this group
        playerPrefKey = $"{groupName}_Volume";

        // Load the saved volume value (default to 0.75 if not set)
        float savedVolume = PlayerPrefs.GetFloat(playerPrefKey, 0.75f);

        // Apply the saved volume to the AudioMixer
        SetVolume(savedVolume);
    }

    public void SetVolume(float sliderValue)
    {
        audioMixer.SetFloat(groupName, Mathf.Log10(sliderValue) * 20); // Set the volume in decibels

        // Save the slider value to PlayerPrefs
        PlayerPrefs.SetFloat(playerPrefKey, sliderValue);
    }
}
