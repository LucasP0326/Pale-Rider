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

    [Header("Death States")]
    public bool sucumbingToPale;

    [Header("Inventory")]
    public InventoryManager inventoryManager; // Reference to the InventoryManager

    [Header("Player Skills")]
    public string signatureSkill;
    public int reptilianBaseScore;
    public int paleoBaseScore;
    public int neoBaseScore;
    public int paleBaseScore;

    public int endurance;
    public int physicality;
    public int reflexivity;
    public int volition;
    public int authority;
    public int conceptualization;
    public int encyclopedia;
    public int empathy;
    public int logic;
    public int perception;
    public int perspicacity;
    public int rhetoric;
    public int savoirFaire;
    public int selfActualization;
    public int suggestion;
    public int tenebrality;

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
        //Initialized Inventory Manager
        inventoryManager = FindFirstObjectByType<InventoryManager>();
        inventoryManager.LoadInventory(); // Load inventory data at the start

        // Initialize health and resolve from Articy variables
        maxHealth = ArticyGlobalVariables.Default.PlayerStats.MaxHealth;
        maxResolve = ArticyGlobalVariables.Default.PlayerStats.MaxResolve;

        signatureSkill = ArticyGlobalVariables.Default.PlayerStats.SignatureSkill;
        reptilianBaseScore = ArticyGlobalVariables.Default.PlayerStats.ReptilianBaseScore;
        paleoBaseScore = ArticyGlobalVariables.Default.PlayerStats.PaleoBaseScore;
        neoBaseScore = ArticyGlobalVariables.Default.PlayerStats.NeoBaseScore;
        paleBaseScore = ArticyGlobalVariables.Default.PlayerStats.PaleBaseScore;

        endurance = ArticyGlobalVariables.Default.PlayerStats.Endurance;
        physicality = ArticyGlobalVariables.Default.PlayerStats.Physicality;
        reflexivity = ArticyGlobalVariables.Default.PlayerStats.Reflexivity;
        volition = ArticyGlobalVariables.Default.PlayerStats.Volition;
        authority = ArticyGlobalVariables.Default.PlayerStats.Authority;
        conceptualization = ArticyGlobalVariables.Default.PlayerStats.Conceptualization;
        encyclopedia = ArticyGlobalVariables.Default.PlayerStats.Encyclopedia;
        empathy = ArticyGlobalVariables.Default.PlayerStats.Empathy;
        logic = ArticyGlobalVariables.Default.PlayerStats.Logic;
        perception = ArticyGlobalVariables.Default.PlayerStats.Perception;
        perspicacity = ArticyGlobalVariables.Default.PlayerStats.Perspicacity;
        rhetoric = ArticyGlobalVariables.Default.PlayerStats.Rhetoric;
        savoirFaire = ArticyGlobalVariables.Default.PlayerStats.SavoirFaire;
        selfActualization = ArticyGlobalVariables.Default.PlayerStats.SelfActualization;
        suggestion = ArticyGlobalVariables.Default.PlayerStats.Suggestion;
        tenebrality = ArticyGlobalVariables.Default.PlayerStats.Tenebrality;

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
        maxHealth = ArticyGlobalVariables.Default.PlayerStats.MaxHealth;
        maxResolve = ArticyGlobalVariables.Default.PlayerStats.MaxResolve;
        currentHealth = ArticyGlobalVariables.Default.PlayerStats.Health;
        currentResolve = ArticyGlobalVariables.Default.PlayerStats.Resolve;

        //Check Death States
        sucumbingToPale = ArticyGlobalVariables.Default.PlayerVariables.SucumbingToPale;

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
        //InitializeHealthBar();
        //InitializeResolveBar();
        UpdateHealthBar();
        UpdateResolveBar();
        UpdateMoneyText();
    }

    public void UpdatePlayerStats()
    {
        // Initialize health and resolve from Articy variables
        maxHealth = ArticyGlobalVariables.Default.PlayerStats.MaxHealth;
        maxResolve = ArticyGlobalVariables.Default.PlayerStats.MaxResolve;

        signatureSkill = ArticyGlobalVariables.Default.PlayerStats.SignatureSkill;
        reptilianBaseScore = ArticyGlobalVariables.Default.PlayerStats.ReptilianBaseScore;
        paleoBaseScore = ArticyGlobalVariables.Default.PlayerStats.PaleoBaseScore;
        neoBaseScore = ArticyGlobalVariables.Default.PlayerStats.NeoBaseScore;
        paleBaseScore = ArticyGlobalVariables.Default.PlayerStats.PaleBaseScore;

        endurance = ArticyGlobalVariables.Default.PlayerStats.Endurance;
        physicality = ArticyGlobalVariables.Default.PlayerStats.Physicality;
        reflexivity = ArticyGlobalVariables.Default.PlayerStats.Reflexivity;
        volition = ArticyGlobalVariables.Default.PlayerStats.Volition;
        authority = ArticyGlobalVariables.Default.PlayerStats.Authority;
        conceptualization = ArticyGlobalVariables.Default.PlayerStats.Conceptualization;
        encyclopedia = ArticyGlobalVariables.Default.PlayerStats.Encyclopedia;
        empathy = ArticyGlobalVariables.Default.PlayerStats.Empathy;
        logic = ArticyGlobalVariables.Default.PlayerStats.Logic;
        perception = ArticyGlobalVariables.Default.PlayerStats.Perception;
        perspicacity = ArticyGlobalVariables.Default.PlayerStats.Perspicacity;
        rhetoric = ArticyGlobalVariables.Default.PlayerStats.Rhetoric;
        savoirFaire = ArticyGlobalVariables.Default.PlayerStats.SavoirFaire;
        selfActualization = ArticyGlobalVariables.Default.PlayerStats.SelfActualization;
        suggestion = ArticyGlobalVariables.Default.PlayerStats.Suggestion;
        tenebrality = ArticyGlobalVariables.Default.PlayerStats.Tenebrality;

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

    public void InitializeHealthBar()
    {
        //maxHealth = reptilianBaseScore;
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

    public void InitializeResolveBar()
    {
        //maxResolve = paleBaseScore;
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
