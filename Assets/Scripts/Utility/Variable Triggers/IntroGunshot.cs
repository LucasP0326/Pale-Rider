using System.Collections;
using UnityEngine;
using UnityEngine.UI; // Required for Image components
using Articy.Unity; // Import Articy namespace
using Articy.Unity.Interfaces;
using Articy.Pale_Rider;
using Articy.Pale_Rider.GlobalVariables;
using TMPro; // Import TextMeshPro namespace

public class IntroGunshot : MonoBehaviour
{
    public AudioSource gunshotAudioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gunshotAudioSource = GetComponent<AudioSource>();
        gunshotAudioSource.pitch = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (ArticyGlobalVariables.Default.GlobalVariables.GunshotSoundEffect == true)
        {
            gunshotAudioSource.Play();
            ArticyGlobalVariables.Default.GlobalVariables.GunshotSoundEffect = false;
        }
    }
}
