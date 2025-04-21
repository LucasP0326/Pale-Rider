using UnityEngine;
using UnityEngine.UI; // Required for Image components
using Articy.Unity; // Import Articy namespace
using Articy.Unity.Interfaces;
using Articy.Pale_Rider;
using Articy.Pale_Rider.GlobalVariables;

public class PlayerStats : MonoBehaviour
{
    [Header("Health Stats")]
    public int currentHealth;
    public int currentResolve;
    public int maxHealth;
    public int maxResolve;

    [Header("Player Skills")]

    [Header("UI")]
    public GameObject HUD;
    public GameObject healthBar; // Parent object for health boxes
    public GameObject resolveBar; // Parent object for resolve boxes
    public GameObject healthBoxPrefab; // Prefab for a single health box
    public GameObject resolveBoxPrefab; // Prefab for a single resolve box

    private GameObject[] healthBoxes; // Array to store health box instances
    private GameObject[] resolveBoxes; // Array to store resolve box instances

    private void Start()
    {
        // Initialize health and resolve from Articy variables
        maxHealth = ArticyGlobalVariables.Default.PlayerStats.MaxHealth;
        maxResolve = ArticyGlobalVariables.Default.PlayerStats.MaxResolve;
        currentHealth = maxHealth;
        currentResolve = maxResolve;

        // Populate the health and resolve bars
        InitializeHealthBar();
        InitializeResolveBar();
        UpdateHealthBar();
        UpdateResolveBar();
    }

    private void Update()
    {
        // Sync current health and resolve with Articy variables
        currentHealth = ArticyGlobalVariables.Default.PlayerStats.Health;
        currentResolve = ArticyGlobalVariables.Default.PlayerStats.Resolve;

        // Update the health and resolve bars
        UpdateHealthBar();
        UpdateResolveBar();
    }

    private void InitializeHealthBar()
    {
        // Clear existing health boxes
        foreach (Transform child in healthBar.transform)
        {
            Destroy(child.gameObject);
        }

        // Create health boxes based on maxHealth
        healthBoxes = new GameObject[maxHealth];
        for (int i = 0; i < maxHealth; i++)
        {
            GameObject healthBox = Instantiate(healthBoxPrefab, healthBar.transform);
            healthBoxes[i] = healthBox;
        }
    }

    private void InitializeResolveBar()
    {
        // Clear existing resolve boxes
        foreach (Transform child in resolveBar.transform)
        {
            Destroy(child.gameObject);
        }

        // Create resolve boxes based on maxResolve
        resolveBoxes = new GameObject[maxResolve];
        for (int i = 0; i < maxResolve; i++)
        {
            GameObject resolveBox = Instantiate(resolveBoxPrefab, resolveBar.transform);
            resolveBoxes[i] = resolveBox;
        }
    }

    private void UpdateHealthBar()
    {
        // Enable or disable the Image component of each health box based on currentHealth
        for (int i = 0; i < maxHealth; i++)
        {
            Image healthImage = healthBoxes[i].GetComponent<Image>();
            if (healthImage != null)
            {
                healthImage.enabled = i < currentHealth; // Enable if within currentHealth, disable otherwise
            }
        }
    }

    private void UpdateResolveBar()
    {
        // Enable or disable the Image component of each resolve box based on currentResolve
        for (int i = 0; i < maxResolve; i++)
        {
            Image resolveImage = resolveBoxes[i].GetComponent<Image>();
            if (resolveImage != null)
            {
                resolveImage.enabled = i < currentResolve; // Enable if within currentResolve, disable otherwise
            }
        }
    }
}
