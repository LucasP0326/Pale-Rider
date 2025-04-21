using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MixerControl : MonoBehaviour
{
    public GameManager gameManager; // Reference to the GameManager script
    [SerializeField] private AudioMixer audioMixer; // Reference to the AudioMixer
    public string groupName;
    [SerializeField] private Slider volumeSlider; // Reference to the UI slider

    private string playerPrefKey; // Key for saving and loading the volume setting

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>(); // Find the GameManager in the scene
        volumeSlider = GetComponent<Slider>(); // Get the Slider component attached to this GameObject

        // Generate a unique PlayerPrefs key for this group
        playerPrefKey = $"{groupName}_Volume";

        // Load the saved volume value (default to 0.75 if not set)
        float savedVolume = PlayerPrefs.GetFloat(playerPrefKey, 0.75f);

        // Apply the saved volume to the AudioMixer
        SetVolume(savedVolume);

        // Set the slider value to match the saved volume
        if (volumeSlider != null)
        {
            //Debug.Log($"Setting volume slider value to: {savedVolume}");
            volumeSlider.value = savedVolume;
        }
    }

    public void SetVolume(float sliderValue)
    {
        audioMixer.SetFloat(groupName, Mathf.Log10(sliderValue) * 20); // Set the volume in decibels

        // Save the slider value to PlayerPrefs
        PlayerPrefs.SetFloat(playerPrefKey, sliderValue);
    }
}
