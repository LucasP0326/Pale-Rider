using UnityEngine;
using UnityEngine.UI; // Required for Image components
using Articy.Unity; // Import Articy namespace
using Articy.Unity.Interfaces;
using Articy.Pale_Rider;
using Articy.Pale_Rider.GlobalVariables;
using TMPro; // Import TextMeshPro namespace

public class PlayerStats : MonoBehaviour
{
    private int previousHealth;
    private int previousResolve;
    private int healthChange;
    private int resolveChange;
    private bool healthChanging = false;
    private bool resolveChanging = false;

    [Header("Health Stats")]
    public int currentHealth;
    public int currentResolve;
    public int maxHealth;
    public int maxResolve;

    [Header("Player Skills")]
    public int reptilianBaseScore = 1;
    public int paleoBaseScore = 1;
    public int neoBaseScore = 1;
    public int paleBaseScore = 1;

    [Header("Player Cash")]
    public float playerCash;

    [Header("UI")]
    public GameObject HUD;
    public GameObject healthBar; // Parent object for health boxes
    public GameObject resolveBar; // Parent object for resolve boxes
    public GameObject healthBoxPrefab; // Prefab for a single health box
    public GameObject resolveBoxPrefab; // Prefab for a single resolve box
    public GameObject healthLoss;
    public TMP_Text healthLossText;
    public TMP_Text healthLossNumberText;
    public GameObject resolveLoss;
    public TMP_Text resolveLossText;
    public TMP_Text resolveLossNumberText;
    private GameObject[] healthBoxes; // Array to store health box instances
    private GameObject[] resolveBoxes; // Array to store resolve box instances
    public TMP_Text moneyText;

    private void Start()
    {
        // Initialize health and resolve from Articy variables
        maxHealth = ArticyGlobalVariables.Default.PlayerStats.MaxHealth;
        maxResolve = ArticyGlobalVariables.Default.PlayerStats.MaxResolve;
        currentHealth = maxHealth;
        currentResolve = maxResolve;
        previousHealth = currentHealth;
        previousResolve = currentResolve;

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

        if (currentHealth != previousHealth)
        {
            healthChanging = true;
            healthChange = currentHealth - previousHealth;
            DisplayHealthChange(healthChange);
            previousHealth = currentHealth;
        }
        else
        {
            healthChanging = false;
        }
        if (currentResolve != previousResolve)
        {
            resolveChanging = true;
            resolveChange = currentResolve - previousResolve;
            DisplayResolveChange(resolveChange);
            previousResolve = currentResolve;
        }
        else
        {
            resolveChanging = false;
        }

        // Update the health and resolve bars
        UpdateHealthBar();
        UpdateResolveBar();
        UpdateMoneyText();
    }

    /*public void ModifyHealth(int amount)
    {
        int previousHealth = currentHealth;
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);

        // Display health loss or gain
        DisplayHealthChange(amount);

        // Update Articy global variable
        ArticyGlobalVariables.Default.PlayerStats.Health = currentHealth;

        // Update the health bar
        UpdateHealthBar();
    }

    public void ModifyResolve(int amount)
    {
        int previousResolve = currentResolve;
        currentResolve = Mathf.Clamp(currentResolve + amount, 0, maxResolve);

        // Display resolve loss or gain
        DisplayResolveChange(amount);

        // Update Articy global variable
        ArticyGlobalVariables.Default.PlayerStats.Resolve = currentResolve;

        // Update the resolve bar
        UpdateResolveBar();
    }*/

    private void DisplayHealthChange(int amount)
    {
        if (healthLoss != null)
        {
            healthLoss.SetActive(true);
            if (amount < 0)
                healthLossNumberText.text = amount.ToString();
            else if (amount > 0)
                healthLossNumberText.text = "+" + amount.ToString();
            else
                healthLossNumberText.text = "0";
            healthLossText.text = amount > 0 ? "Health Gained" : "Health Lost";

            // Hide the healthLoss object after a short delay
            Invoke(nameof(HideHealthLoss), 2f);
        }
    }

    private void DisplayResolveChange(int amount)
    {
        if (resolveLoss != null)
        {
            resolveLoss.SetActive(true);
            if (amount < 0)
                resolveLossNumberText.text = amount.ToString();
            else if (amount > 0)
                resolveLossNumberText.text = "+" + amount.ToString();
            else
                resolveLossNumberText.text = "0";
            resolveLossText.text = amount > 0 ? "Resolve Gained" : "Resolve Lost";

            // Hide the resolveLoss object after a short delay
            Invoke(nameof(HideResolveLoss), 2f);
        }
    }

    private void HideHealthLoss()
    {
        if (healthLoss != null)
        {
            healthLoss.SetActive(false);
        }
    }

    private void HideResolveLoss()
    {
        if (resolveLoss != null)
        {
            resolveLoss.SetActive(false);
        }
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

    private void UpdateMoneyText()
    {
        if (moneyText != null)
        {
            moneyText.text = "£" + playerCash.ToString("F2"); // Format to 2 decimal places
        }
    }
}
