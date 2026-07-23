using Articy.Unity;
using Articy.Unity.Interfaces;
using UnityEngine;
using UnityEngine.UI;
using Articy.Pale_Rider;
using Articy.Pale_Rider.GlobalVariables;
using StarterAssets;
using TMPro;

public class InventoryInterface : MonoBehaviour
{
    //Important References
    private GameObject playerController;
    public GameObject inventorySpace; // The parent GameObject that holds all inventory UI elements
    private ThirdPersonController controller;
    private InventoryManager inventoryManager; // Reference to the InventoryManager script
    private OxygenHandler oxygenHandler; // Reference to the OxygenHandler script
    private PlayerStats playerStats;
    public DialogueManager dialogueManager;
    public ArticyReference aObject; // Reference to the Articy object for dialogue

    [Header("UI Elements")]
    //Stats
    public GameObject repNumber;
    public GameObject paleoNumber;
    public GameObject neoNumber;
    public GameObject paleNumber;

    //Health And Resolve UI
    public GameObject healthBar;
    public GameObject resolveBar;
    public GameObject healthBoxPrefab; // Prefab for a single health box
    public GameObject resolveBoxPrefab; // Prefab for a single resolve box
    private GameObject[] healthBoxes; // Array to store health box instances
    private GameObject[] resolveBoxes; // Array to store resolve box instances
    public GameObject healthText;
    public GameObject resolveText;

    public GameObject selectedItem;

    //Inventory Item UI
    public GameObject ToolsinventoryGrid; // The grid where inventory items will be displayed
    public GameObject ClothesinventoryGrid; // The grid for clothes items
    public GameObject ItemsinventoryGrid; // The grid for other items
    public GameObject InteractinventoryGrid; // The grid for interactable items

    //Item Display
    public GameObject selectedItemPicture;
    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemDescription;
    public TextMeshProUGUI selectedItemPrice;
    public TextMeshProUGUI selectedItemBonuses;
    public GameObject equipButton;
    public GameObject interactButton;
    public ArticyObject availableDialogue;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player");
        inventorySpace = GameObject.FindGameObjectWithTag("InventorySpace");
        controller = playerController.GetComponent<ThirdPersonController>();
        inventoryManager = playerController.GetComponent<InventoryManager>();
        playerStats = playerController.GetComponent<PlayerStats>();
        oxygenHandler = playerController.GetComponent<OxygenHandler>();
        dialogueManager = FindObjectOfType<DialogueManager>();
        //aObject = gameObject.GetComponent<ArticyReference>();
        UpdateInventory();
        UpdateOxygen();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerController != null)
        {
            if (controller != null)
            {
                controller.inMenu = gameObject.activeSelf;
                controller.paused = gameObject.activeSelf;
            }
        }

        UpdateNumbers();
        UpdateHealth();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Close();
        }

        if (selectedItem != null && selectedItem.GetComponent<InventoryItem>().itemType == "Clothing" || selectedItem.GetComponent<InventoryItem>().itemType == "Tool")
        {
            equipButton.SetActive(true);
            if (selectedItem.GetComponent<InventoryItem>().isEquipped)
            {
                equipButton.GetComponentInChildren<TextMeshProUGUI>().text = "Unequip";
            }
            else
            {
                equipButton.GetComponentInChildren<TextMeshProUGUI>().text = "Equip";
            }
        }
        else if (selectedItem != null && selectedItem.GetComponent<InventoryItem>().itemType == "Interactable")
        {
            interactButton.SetActive(true);
        }
        else
        {
            equipButton.SetActive(false);
            interactButton.SetActive(false);
        }
    }

    public void UpdateInventory()
    {
        // Clear all inventory grids
        foreach (Transform child in ToolsinventoryGrid.transform)
            Destroy(child.gameObject);
        foreach (Transform child in ClothesinventoryGrid.transform)
            Destroy(child.gameObject);
        foreach (Transform child in ItemsinventoryGrid.transform)
            Destroy(child.gameObject);
        foreach (Transform child in InteractinventoryGrid.transform)
            Destroy(child.gameObject);

        // Repopulate grids based on itemType
        foreach (var item in inventoryManager.inventoryItems)
        {
            if (item == null) continue;

            // Instantiate the UI element for the item (assuming itemPrefab is a UI prefab)
            InventoryItem itemUI = Instantiate(inventoryManager.itemPrefab);

            // Set parent based on itemType
            switch (item.itemType)
            {
                case "Tool":
                    itemUI.transform.SetParent(ToolsinventoryGrid.transform, false);
                    break;
                case "Clothing":
                    itemUI.transform.SetParent(ClothesinventoryGrid.transform, false);
                    break;
                case "Item":
                    itemUI.transform.SetParent(ItemsinventoryGrid.transform, false);
                    break;
                case "Interactable":
                    itemUI.transform.SetParent(InteractinventoryGrid.transform, false);
                    break;
                default:
                    itemUI.transform.SetParent(ItemsinventoryGrid.transform, false); // fallback
                    break;
            }

            // Copy data from the inventory item to the UI instance
            itemUI.technicalName = item.technicalName;
            itemUI.itemName = item.itemName;
            itemUI.itemType = item.itemType;
            itemUI.itemDescription = item.itemDescription;
            itemUI.itemIcon = item.itemIcon;
            itemUI.itemPrice = item.itemPrice;
            itemUI.itemBonuses = item.itemBonuses;
            // ...copy any other fields as needed...

            // Optionally update UI visuals (icon, text, etc.)
            var imageComponent = itemUI.GetComponent<UnityEngine.UI.Image>();
            if (imageComponent != null && item.itemIcon != null)
                imageComponent.sprite = item.itemIcon;
        }
    }

    public void UpdateNumbers()
    {
        repNumber.GetComponent<TextMeshProUGUI>().text = ArticyGlobalVariables.Default.PlayerStats.ReptilianBaseScore.ToString();
        paleoNumber.GetComponent<TextMeshProUGUI>().text = ArticyGlobalVariables.Default.PlayerStats.PaleoBaseScore.ToString();
        neoNumber.GetComponent<TextMeshProUGUI>().text = ArticyGlobalVariables.Default.PlayerStats.NeoBaseScore.ToString();
        paleNumber.GetComponent<TextMeshProUGUI>().text = ArticyGlobalVariables.Default.PlayerStats.PaleBaseScore.ToString();
    }

    public void UpdateHealth()
    {
        // Ensure healthBoxes array is initialized and matches maxHealth
        if (healthBoxes == null || healthBoxes.Length != playerStats.maxHealth)
        {
            // Clear old boxes
            foreach (Transform child in healthBar.transform)
                Destroy(child.gameObject);

            // Create new boxes
            healthBoxes = new GameObject[playerStats.maxHealth];
            for (int i = 0; i < playerStats.maxHealth; i++)
            {
                healthBoxes[i] = Instantiate(healthBoxPrefab, healthBar.transform);
            }
        }

        // Enable boxes up to currentHealth, disable the rest
        for (int i = 0; i < playerStats.maxHealth; i++)
        {
            healthBoxes[i].SetActive(i < playerStats.currentHealth);
        }

        // Repeat for resolve
        if (resolveBoxes == null || resolveBoxes.Length != playerStats.maxResolve)
        {
            foreach (Transform child in resolveBar.transform)
                Destroy(child.gameObject);

            resolveBoxes = new GameObject[playerStats.maxResolve];
            for (int i = 0; i < playerStats.maxResolve; i++)
            {
                resolveBoxes[i] = Instantiate(resolveBoxPrefab, resolveBar.transform);
            }
        }

        for (int i = 0; i < playerStats.maxResolve; i++)
        {
            resolveBoxes[i].SetActive(i < playerStats.currentResolve);
        }

        // Update health and resolve text
        healthText.GetComponent<TextMeshProUGUI>().text = $"{playerStats.currentHealth}/{playerStats.maxHealth}";
        resolveText.GetComponent<TextMeshProUGUI>().text = $"{playerStats.currentResolve}/{playerStats.maxResolve}";
    }

    public void UpdateOxygen()
    {
        if (oxygenHandler != null)
        {
            oxygenHandler.instantAssignUIElements(); // Ensure UI elements are assigned
        }
    }

    public void Close()
    {
        // Close the inventory interface
        gameObject.SetActive(false);

        UpdateInventory();

        // Resume player control
        if (playerController != null && controller != null)
        {
            controller.inMenu = false;
            controller.paused = false;
        }
    }

    public void SelectItem()
    {
        if (selectedItem != null)
        {
            InventoryItem[] items = inventorySpace.GetComponentsInChildren<InventoryItem>(true);
            string itemName = selectedItem.GetComponent<InventoryItem>().itemName;
            foreach (var inv in items)
            {
                if (inv != null && inv.itemName == itemName)
                {
                    selectedItem = inv.gameObject;
                    //SelectItem();
                    //return;
                }
            }
            InventoryItem item = selectedItem.GetComponent<InventoryItem>();
            if (item != null)
            {
                selectedItemPicture.GetComponent<Image>().sprite = item.itemIcon;
                selectedItemName.text = item.GetComponent<InventoryItem>().itemName;
                selectedItemDescription.text = item.GetComponent<InventoryItem>().itemDescription;
                // Format price as pounds and pence (e.g., £12.34)
                int price = item.GetComponent<InventoryItem>().itemPrice;
                selectedItemPrice.text = "£" + (price / 100f).ToString("0.00");
                selectedItemBonuses.text = "Item Bonuses: " + item.GetComponent<InventoryItem>().itemBonuses;
                Debug.Log("Populating UI");
                //aObject.reference = ArticyDatabase.GetObject(item.technicalName);
            }
            else
            {
                Debug.LogWarning("Selected item does not have an InventoryItem component.");
            }
        }
        else
        {
            Debug.LogWarning("No item selected.");
        }
    }

    public void EquipSelectedItem()
    {
        if (selectedItem != null)
        {
            InventoryItem item = selectedItem.GetComponent<InventoryItem>();
            if (item != null)
            {
                // Implement equip logic here, e.g., update player stats, change appearance, etc.
                Debug.Log("Equipped Item: " + item.itemName);
                selectedItem.GetComponent<InventoryItem>().isEquipped = !selectedItem.GetComponent<InventoryItem>().isEquipped; // Toggle equip state
                selectedItem.GetComponent<InventoryItem>().FindBonuses(); // Apply bonuses when equipping
            }
            else
            {
                Debug.LogWarning("Selected item does not have an InventoryItem component.");
            }
        }
        else
        {
            Debug.LogWarning("No item selected to equip.");
        }
        if (selectedItem.GetComponent<InventoryItem>().isEquipped)
        {
            switch (selectedItem.GetComponent<InventoryItem>().itemType)
            {
                case "Tool":
                    if (selectedItem.GetComponent<InventoryItem>().itemClothingCategory == "HeldItem")
                    {
                        ArticyGlobalVariables.Default.EquippedItems.HeldItem = selectedItem.GetComponent<InventoryItem>().technicalName;
                    }
                    else
                        ArticyGlobalVariables.Default.EquippedItems.EquippedTool = selectedItem.GetComponent<InventoryItem>().technicalName;
                    break;
                case "Clothing":
                    switch (selectedItem.GetComponent<InventoryItem>().itemClothingCategory)
                    {
                        case "Head":
                            ArticyGlobalVariables.Default.EquippedItems.EquippedHead = selectedItem.GetComponent<InventoryItem>().technicalName;
                            break;
                        case "Face":
                            ArticyGlobalVariables.Default.EquippedItems.EquippedFace = selectedItem.GetComponent<InventoryItem>().technicalName;
                            break;
                        case "Neck":
                            ArticyGlobalVariables.Default.EquippedItems.EquippedNeck = selectedItem.GetComponent<InventoryItem>().technicalName;
                            break;
                        case "Body":
                            ArticyGlobalVariables.Default.EquippedItems.EquippedBody = selectedItem.GetComponent<InventoryItem>().technicalName;
                            break;
                        case "Legs":
                            ArticyGlobalVariables.Default.EquippedItems.EquippedLegs = selectedItem.GetComponent<InventoryItem>().technicalName;
                            break;
                        case "Feet":
                            ArticyGlobalVariables.Default.EquippedItems.EquippedFeet = selectedItem.GetComponent<InventoryItem>().technicalName;
                            break;
                        case "Hands":
                            ArticyGlobalVariables.Default.EquippedItems.EquippedHands = selectedItem.GetComponent<InventoryItem>().technicalName;
                            break;
                    }
                    break;
            }
        }
        else if (selectedItem.GetComponent<InventoryItem>().isEquipped == false)
        {
            switch (selectedItem.GetComponent<InventoryItem>().itemType)
            {
                case "Tool":
                    if (selectedItem.GetComponent<InventoryItem>().itemClothingCategory == "HeldItem")
                    {
                        ArticyGlobalVariables.Default.EquippedItems.HeldItem = "";
                    }
                    else
                        ArticyGlobalVariables.Default.EquippedItems.EquippedTool = "";
                    break;
                case "Clothing":
                    switch (selectedItem.GetComponent<InventoryItem>().itemClothingCategory)
                    {
                        case "Head":
                            ArticyGlobalVariables.Default.EquippedItems.EquippedHead = "";
                            break;
                        case "Face":
                            ArticyGlobalVariables.Default.EquippedItems.EquippedFace = "";
                            break;
                        case "Neck":
                            ArticyGlobalVariables.Default.EquippedItems.EquippedNeck = "";
                            break;
                        case "Body":
                            ArticyGlobalVariables.Default.EquippedItems.EquippedBody = "";
                            break;
                        case "Legs":
                            ArticyGlobalVariables.Default.EquippedItems.EquippedLegs = "";
                            break;
                        case "Feet":
                            ArticyGlobalVariables.Default.EquippedItems.EquippedFeet = "";
                            break;
                        case "Hands":
                            ArticyGlobalVariables.Default.EquippedItems.EquippedHands = "";
                            break;
                    }
                    break;
            }
        }
    }

    public void InteractWithSelectedItem()
    {
        if (selectedItem == null)
        {
            Debug.LogWarning("No item selected.");
            return;
        }

        var item = selectedItem.GetComponent<InventoryItem>();
        if (item == null)
        {
            Debug.LogWarning("Selected item does not have an InventoryItem component.");
            return;
        }

        if (item.availableDialogue == null || string.IsNullOrEmpty(item.availableDialogue.TechnicalName))
        {
            Debug.LogWarning("Selected item has no available dialogue reference.");
            return;
        }

        var dialogueObj = ArticyDatabase.GetObject(item.availableDialogue.TechnicalName) as IArticyObject;
        if (dialogueObj == null)
        {
            Debug.LogWarning("Dialogue object not found for technical name: " + item.availableDialogue.TechnicalName);
            return;
        }

        dialogueManager.StartDialogue(dialogueObj);
        Close();
    }
}


