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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        oxygenHandler = GameObject.Find("Player").GetComponent<OxygenHandler>(); // Get the OxygenHandler component from the Player GameObject
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
