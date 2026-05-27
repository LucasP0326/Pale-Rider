using UnityEngine;
using UnityEngine;
using UnityEngine;
using Articy.Unity;
using Articy.Unity.Interfaces;
using Articy.Pale_Rider;
using Articy.Pale_Rider.GlobalVariables;
using StarterAssets;
using UnityEngine.UI;
using System.Collections;
using System; // <-- added for Convert
using System.Reflection; // <-- added for reflection
using TMPro; // Import TextMeshPro namespace

public class PaleManager : MonoBehaviour
{
    public OxygenHandler oxygenHandler; // Reference to the OxygenHandler script
    public ThirdPersonController playerController; // Reference to the ThirdPersonController script

    public bool PaleDeathEnabled = true; // Flag to enable or disable the Pale death scene

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        oxygenHandler = GameObject.Find("Player").GetComponent<OxygenHandler>(); // Get the OxygenHandler component from the Player GameObject
        playerController = GameObject.Find("Player").GetComponent<ThirdPersonController>(); // Get the ThirdPersonController component from the Player GameObject
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (oxygenHandler != null)
            {
                oxygenHandler.inPale = true; // Set inPale to true when the player is inside the Pale
            }
        }
        if (other.CompareTag("Player") && PaleDeathEnabled)
        {
            if (oxygenHandler != null)
            {
                oxygenHandler.paleDeathEnabled = true; // Enable Pale death scene when the player is inside the Pale
            }
        }
        else if (other.CompareTag("Player") && !PaleDeathEnabled)
        {
            if (oxygenHandler != null)
            {
                oxygenHandler.paleDeathEnabled = false; // Disable Pale death scene when the player is inside the Pale
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (oxygenHandler != null)
            {
                oxygenHandler.inPale = false; // Set inPale to false when the player exits the Pale
            }
        }
    }
}
