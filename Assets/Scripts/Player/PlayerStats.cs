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
    public string signatureSkill = ArticyGlobalVariables.Default.PlayerStats.SignatureSkill;
    public int reptilianBaseScore = ArticyGlobalVariables.Default.PlayerStats.ReptilianBaseScore;
    public int paleoBaseScore = ArticyGlobalVariables.Default.PlayerStats.PaleoBaseScore;
    public int neoBaseScore = ArticyGlobalVariables.Default.PlayerStats.NeoBaseScore;
    public int paleBaseScore = ArticyGlobalVariables.Default.PlayerStats.PaleBaseScore;

    public int endurance = ArticyGlobalVariables.Default.PlayerStats.Endurance;
    public int physicality = ArticyGlobalVariables.Default.PlayerStats.Physicality;
    public int reflexivity = ArticyGlobalVariables.Default.PlayerStats.Reflexivity;
    public int volition = ArticyGlobalVariables.Default.PlayerStats.Volition;
    public int authority = ArticyGlobalVariables.Default.PlayerStats.Authority;
    public int conceptualization = ArticyGlobalVariables.Default.PlayerStats.Conceptualization;
    public int encyclopedia = ArticyGlobalVariables.Default.PlayerStats.Encyclopedia;
    public int empathy = ArticyGlobalVariables.Default.PlayerStats.Empathy;
    public int logic = ArticyGlobalVariables.Default.PlayerStats.Logic;
    public int perception = ArticyGlobalVariables.Default.PlayerStats.Perception;
    public int perspicacity = ArticyGlobalVariables.Default.PlayerStats.Perspicacity;
    public int rhetoric = ArticyGlobalVariables.Default.PlayerStats.Rhetoric;
    public int savoirFaire = ArticyGlobalVariables.Default.PlayerStats.SavoirFaire;
    public int selfActualization = ArticyGlobalVariables.Default.PlayerStats.SelfActualization;
    public int suggestion = ArticyGlobalVariables.Default.PlayerStats.Suggestion;
    public int tenebrality = ArticyGlobalVariables.Default.PlayerStats.Tenebrality;

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

        signatureSkill = ArticyGlobalVariables.Default.PlayerStats.SignatureSkill;
        reptilianBaseScore = ArticyGlobalVariables.Default.PlayerStats.ReptilianBaseScore;
        paleoBaseScore = ArticyGlobalVariables.Default.PlayerStats.PaleoBaseScore;
        neoBaseScore = ArticyGlobalVariables.Default.PlayerStats.NeoBaseScore;
        paleBaseScore = ArticyGlobalVariables.Default.PlayerStats.PaleBaseScore;

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

        ArticyGlobalVariables.Default.PlayerStats.Endurance = endurance;
        ArticyGlobalVariables.Default.PlayerStats.Physicality = physicality;
        ArticyGlobalVariables.Default.PlayerStats.Reflexivity = reflexivity;
        ArticyGlobalVariables.Default.PlayerStats.Volition = volition;
        ArticyGlobalVariables.Default.PlayerStats.Authority = authority;
        ArticyGlobalVariables.Default.PlayerStats.Conceptualization = conceptualization;
        ArticyGlobalVariables.Default.PlayerStats.Encyclopedia = encyclopedia;
        ArticyGlobalVariables.Default.PlayerStats.Empathy = empathy;
        ArticyGlobalVariables.Default.PlayerStats.Logic = logic;
        ArticyGlobalVariables.Default.PlayerStats.Perception = perception;
        ArticyGlobalVariables.Default.PlayerStats.Perspicacity = perspicacity;
        ArticyGlobalVariables.Default.PlayerStats.Rhetoric = rhetoric;
        ArticyGlobalVariables.Default.PlayerStats.SavoirFaire = savoirFaire;
        ArticyGlobalVariables.Default.PlayerStats.SelfActualization = selfActualization;
        ArticyGlobalVariables.Default.PlayerStats.Suggestion = suggestion;
        ArticyGlobalVariables.Default.PlayerStats.Tenebrality = tenebrality;

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
