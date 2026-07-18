using System.Collections;
using UnityEngine;
using UnityEngine.UI; // Required for Image components
using Articy.Unity; // Import Articy namespace
using Articy.Unity.Interfaces;
using Articy.Pale_Rider;
using Articy.Pale_Rider.GlobalVariables;
using TMPro; // Import TextMeshPro namespace
using UnityEngine.SceneManagement;
using StarterAssets;

public class PlayerStats : MonoBehaviour
{
    private int previousHealth;
    private int previousResolve;
    private int healthChange;
    private int resolveChange;
    public bool deathEnabled = true;
    private bool healthChanging = false;
    private bool resolveChanging = false;

    [Header("References")]
    public ThirdPersonController playerController; // Reference to the ThirdPersonController script
    public OxygenHandler oxygenHandler; // Reference to the OxygenHandler script

    [Header("Health Stats")]
    public int currentHealth;
    public int currentResolve;
    public int maxHealth;
    public int maxResolve;
    public int experience;

    [Header("Death States")]
    public bool alreadyDying;
    public bool sucumbingToPale;
    public Interactable healthDeathObject;
    public Interactable resolveDeathObject;

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
    public int playerCash;

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
    public GameObject fadeCanvas; // Fullscreen UI Image for fading

    private void Start()
    {
        playerController = GetComponent<ThirdPersonController>(); // Get the ThirdPersonController component from the Player GameObject
        oxygenHandler = GetComponent<OxygenHandler>(); // Get the OxygenHandler component from the Player GameObject

        //Reset Death
        ArticyGlobalVariables.Default.PlayerVariables.PhysicalDeath = false;
        ArticyGlobalVariables.Default.PlayerVariables.ResolveDeath = false;

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
        if (deathEnabled)
        {
            InitializeHealthBar();
            InitializeResolveBar();
            UpdateHealthBar();
            UpdateResolveBar();
        }
    }

    private void Update()
    {
        //Sync Experience
        experience = ArticyGlobalVariables.Default.PlayerStats.Experience;

        // Sync current health and resolve with Articy variables
        maxHealth = ArticyGlobalVariables.Default.PlayerStats.MaxHealth;
        maxResolve = ArticyGlobalVariables.Default.PlayerStats.MaxResolve;
        currentHealth = ArticyGlobalVariables.Default.PlayerStats.Health;
        currentResolve = ArticyGlobalVariables.Default.PlayerStats.Resolve;

        //Update Money
        playerCash = ArticyGlobalVariables.Default.PlayerStats.Money;

        //Check Death States
        sucumbingToPale = ArticyGlobalVariables.Default.PlayerVariables.SucumbingToPale;

        if (sucumbingToPale)
        {
            StartCoroutine(PaleDeathScene());
            sucumbingToPale = false; // Reset the flag to prevent repeated triggers
            ArticyGlobalVariables.Default.PlayerVariables.SucumbingToPale = false; // Also reset in Articy variables
            Debug.Log("Player is sucumbing to the Pale!");
            // You can trigger death animations, game over screens, etc.
        }

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
        if (deathEnabled)
        {
            UpdateHealthBar();
            UpdateResolveBar();
            UpdateMoneyText();
        }
        

        if (ArticyGlobalVariables.Default.PlayerVariables.PhysicalDeath)
        {
            SceneManager.LoadScene("End Scene");
        }
        if (ArticyGlobalVariables.Default.PlayerVariables.ResolveDeath)
        {
            SceneManager.LoadScene("End Scene");
        }
        if (ArticyGlobalVariables.Default.PlayerVariables.PaleDeath)
        {
            SceneManager.LoadScene("End Scene");
        }
    }

    private void LateUpdate()
    {
        if (deathEnabled)
        {
            CheckDeath();
        }
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

    public void CheckDeath()
    {
        if (currentHealth <= 0)
        {
            Debug.Log("Player has died due to health reaching zero.");
            StartCoroutine(HealthDeathScene());
        }
        if (currentResolve <= 0)
        {
            Debug.Log("Player has died due to resolve reaching zero.");
            if (!oxygenHandler.inPale)
            {
                StartCoroutine(ResolveDeathScene());
            }
            else
            {
                StartCoroutine(PaleDeathScene());
            }
        }
    }

    private void UpdateMoneyText()
    {
        if (moneyText != null)
        {
            decimal displayCash = playerCash / 100m; // Assuming playerCash is in pennies, convert to pounds
            moneyText.text = "£" + displayCash.ToString("F2"); // Format to 2 decimal places
        }
    }

    private IEnumerator PaleDeathScene()
    {
        Debug.Log("Starting Pale Death Scene Transition...");
        if (fadeCanvas == null)
        {
            Debug.LogWarning("FadeCanvas not found in scene.");
            yield break;
        }

        // Get the CanvasRenderer or Image component to control alpha
        var image = fadeCanvas.GetComponent<UnityEngine.UI.Image>();
        if (image == null)
        {
            Debug.LogWarning("FadeCanvas does not have an Image component.");
            yield break;
        }

        // Fade to black over 2 seconds
        float duration = 2f;
        float elapsed = 0f;
        Color color = image.color;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsed / duration);
            image.color = color;
            yield return null;
        }
        color.a = 1f;
        image.color = color;

        // Wait a moment on black
        yield return new WaitForSeconds(1f);

        // Load the "Sucumbing to Pale" scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("Sucumbing to Pale");
    }

    private IEnumerator HealthDeathScene()
    {
        healthDeathObject.OnInteract();
        yield return null;
    }
    
    private IEnumerator ResolveDeathScene()
    {
        resolveDeathObject.OnInteract();
        yield return null;
    }
}
