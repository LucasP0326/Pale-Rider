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

public class OxygenHandler : MonoBehaviour
{
    [Header("References")]
    public PlayerEquipment playerEquipment; // Reference to the PlayerEquipment script
    
    [Header("Essentials")]
    public int maxOxygen = 1000;
    public int currentOxygen;
    public int oxygenDepletionRate = 1; // Oxygen depletion rate per second
    public bool gasMaskEquipped = false; // Track if the gas mask is equipped
    public bool inPale = false;
    public bool paleDeathEnabled = true; // Flag to enable or disable the Pale death scene
    [Header("Resolve Damage")]
    public int resolveDamageIntervalSeconds = 20; // Seconds between resolve hits
    private int resolveSecondsCounter = 0; // Counter to track seconds without protection

    [Header("UI Elements")]
    public Image hudOxygenTank; // Reference to the UI Image that represents the HUD oxygen tank
    public Image oxygenBarFill; // Reference to the UI Image that represents the oxygen bar fill
    public Image hudOxygenFill; // Reference to the UI Image that represents the HUD oxygen fill
    public TMP_Text oxygenText; // Reference to the TextMeshPro text that displays the oxygen percentage
    public TMP_Text hudOxygenText; // Reference to the TextMeshPro text that displays the HUD oxygen percentage

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Assign References
        playerEquipment = GetComponent<PlayerEquipment>(); // Get the PlayerEquipment component from the same GameObject

        //Assign UI
        hudOxygenTank = GameObject.Find("HUDOxygenTank").GetComponent<Image>(); // Ensure this matches the name of your HUD UI Image
        hudOxygenFill = GameObject.Find("HUDOxygenBarFill").GetComponent<Image>(); // Ensure this matches the name of your HUD UI Image
        hudOxygenText = GameObject.Find("HUDOxygenText").GetComponent<TMP_Text>(); // Ensure this matches the name of your HUD TextMeshPro text

        maxOxygen = ArticyGlobalVariables.Default.PlayerStats.MaxOxygen;
        StartCoroutine(assignUIElements()); // Start the coroutine to assign UI elements after a short delay
        StartCoroutine(DepleteOxygen()); // Start depleting oxygen if the gas mask is equipped and the player is in the Pale
    }

    // Update is called once per frame
    void Update()
    {
        if (ArticyGlobalVariables.Default.EquippedItems.EquippedFace == "Clothing_GasMask")
        {
            gasMaskEquipped = true;
            //oxygenBarFill.gameObject.SetActive(true); // Show oxygen bar when gas mask is equipped
            hudOxygenFill.gameObject.SetActive(true); // Show HUD oxygen bar when gas mask is equipped
            hudOxygenTank.color = Color.white; // Set the HUD oxygen tank color to white when the gas mask is equipped
        }
        else
        {
            gasMaskEquipped = false;
            //oxygenBarFill.gameObject.SetActive(false); // Hide oxygen bar when gas mask is not equipped
            hudOxygenFill.gameObject.SetActive(false); // Hide HUD oxygen bar when gas mask is not equipped
            hudOxygenTank.color = Color.white; // Set the HUD oxygen tank color to gray when the gas mask is not equipped
        }

        currentOxygen = ArticyGlobalVariables.Default.PlayerStats.CurrentOxygen; // Update current oxygen from global variables

        //Update UI
        if (oxygenBarFill != null)
        {
            oxygenBarFill.fillAmount = Mathf.Clamp01((float)currentOxygen / Mathf.Max(1, maxOxygen)); // Update and clamp fill amount
        }
        if (hudOxygenFill != null)
        {
            hudOxygenFill.fillAmount = Mathf.Clamp01((float)currentOxygen / Mathf.Max(1, maxOxygen)); // Update and clamp HUD fill amount
        }
        if (oxygenText != null)
        {
            int percent = Mathf.Clamp(Mathf.RoundToInt((float)currentOxygen / Mathf.Max(1, maxOxygen) * 100f), 0, 100);
            oxygenText.text = percent + "%"; // Display clamped percentage
        }
        if (hudOxygenText != null)
        {
            int percentHud = Mathf.Clamp(Mathf.RoundToInt((float)currentOxygen / Mathf.Max(1, maxOxygen) * 100f), 0, 100);
            hudOxygenText.text = percentHud + "%"; // Display clamped HUD percentage
        }
    }

    public IEnumerator DepleteOxygen()
    {
        // Run forever and check the conditions each second so changes
        // to `gasMaskEquipped` or `inPale` take effect immediately.
        while (true)
        {
            yield return new WaitForSeconds(1f); // Wait for 1 second between ticks

            if (gasMaskEquipped && inPale)
            {
                // Subtract and clamp to zero
                int newOxygen = ArticyGlobalVariables.Default.PlayerStats.CurrentOxygen - oxygenDepletionRate;
                ArticyGlobalVariables.Default.PlayerStats.CurrentOxygen = Mathf.Max(0, newOxygen);
                currentOxygen = ArticyGlobalVariables.Default.PlayerStats.CurrentOxygen;
            }
            else
            {
                // Ensure currentOxygen variable is kept in sync even when not depleting here
                currentOxygen = ArticyGlobalVariables.Default.PlayerStats.CurrentOxygen;
            }

            // Resolve damage: when player is in the Pale and either not wearing a gas mask
            // OR their oxygen is at 0, count seconds and apply a resolve hit every interval.
            if (paleDeathEnabled)
            {
                if (inPale && (!gasMaskEquipped || ArticyGlobalVariables.Default.PlayerStats.CurrentOxygen <= 0))
                {
                    resolveSecondsCounter++;
                    if (resolveSecondsCounter >= resolveDamageIntervalSeconds)
                    {
                        // Subtract one resolve point and clamp
                        int newResolve = ArticyGlobalVariables.Default.PlayerStats.Resolve - 1;
                        ArticyGlobalVariables.Default.PlayerStats.Resolve = Mathf.Max(0, newResolve);
                        // Reset counter after applying damage
                        resolveSecondsCounter = 0;
                    }
                }
                else
                {
                    // Reset counter when condition no longer holds
                    resolveSecondsCounter = 0;
                }
            }
        }
    }

    public IEnumerator assignUIElements()
    {
        yield return new WaitForSeconds(0.1f);
        oxygenBarFill = GameObject.Find("OxygenBarFill").GetComponent<Image>(); // Ensure this matches the name of your UI Image
        oxygenText = GameObject.Find("OxygenText").GetComponent<TMP_Text>(); // Ensure this matches the name of your TextMeshPro text
    }

    // Helper so other scripts / triggers can set whether the player is currently in the Pale
    public void SetInPale(bool value)
    {
        inPale = value;
    }
}
