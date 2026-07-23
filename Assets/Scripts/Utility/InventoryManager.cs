using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Articy.Unity;
using Articy.Unity.Interfaces;
using Articy.Pale_Rider;
using Articy.Pale_Rider.GlobalVariables;
using System.Collections.Generic;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public InventoryItem[] inventoryItems; // Array to hold all inventory items
    public InventoryItem itemPrefab; // Prefab for creating new inventory items
    public GameObject inventorySpace;

    [Header("Item Added Popup")]
    public GameObject itemPopup;
    public TextMeshProUGUI popupItemName;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //PlayerPrefs.DeleteKey("PlayerInventory"); // Remove saved inventory on game start
        // Optionally: PlayerPrefs.DeleteAll(); // (removes all PlayerPrefs, use with caution)
        //DontDestroyOnLoad(gameObject);
        InitializeEquipment();
    }

    // Update is called once per frame
    void Update()
    {
        InventoryItemChecker();
    }

    public void AddItem(string technicalName)
    {
        // Get the Articy object by ID
        var articyObj = ArticyDatabase.GetObject(technicalName) as Articy.Pale_Rider.Item;
        if (articyObj == null)
        {
            Debug.LogWarning("Articy object not found for technical name: " + technicalName);
            return;
        }

        // Instantiate the prefab
        InventoryItem newItem = Instantiate(itemPrefab, inventorySpace.transform);

        // Populate fields
        newItem.technicalName = technicalName;
        newItem.itemName = articyObj.DisplayName;
        newItem.itemType = articyObj.Template.ItemCategory.SmallTextValue;
        newItem.itemClothingCategory = articyObj.Template.ClothingSlot.SmallTextValue;
        newItem.itemDescription = articyObj.Template.Description.MediumTextValue;
        newItem.itemPrice = (int)articyObj.Template.Price.NumberValue;
        newItem.itemBonuses = articyObj.Template.ItemBonuses.MediumTextValue;
        newItem.availableDialogue = articyObj.Template.DialogueConnector.ReferenceSlot;

        StartCoroutine(PopupCoroutine(newItem.itemName));

        // If your Articy object stores an image as an Asset reference:
        var itemAsset = ((articyObj as IObjectWithPreviewImage).PreviewImage.Asset as Asset);
        newItem.itemIcon = itemAsset != null ? itemAsset.LoadAssetAsSprite() : null;
        if (newItem.itemIcon != null)
        {
            var imageComponent = newItem.GetComponent<Image>();
            if (imageComponent != null)
            {
                imageComponent.sprite = newItem.itemIcon;
            }
        }

        newItem.itemQuantity = 1; // Default quantity for new items
        newItem.isEquipped = false; // Default state for new items

        // Add the new item to the inventory
        var itemsList = new System.Collections.Generic.List<InventoryItem>(inventoryItems ?? new InventoryItem[0]);
        itemsList.Add(newItem);
        inventoryItems = itemsList.ToArray();
    }

    public void SaveInventory()
    {
        var dataList = new List<InventoryItemData>();
        foreach (var item in inventoryItems)
        {
            if (item == null) continue;
            dataList.Add(new InventoryItemData
            {
                technicalName = item.technicalName,
                itemName = item.itemName,
                itemType = item.itemType,
                itemClothingCategory = item.itemClothingCategory,
                itemDescription = item.itemDescription,
                itemPrice = item.itemPrice,
                itemBonuses = item.itemBonuses,
                itemQuantity = item.itemQuantity,
                isEquipped = item.isEquipped,
                availableDialogueTechnicalName = item.availableDialogue != null ? item.availableDialogue.TechnicalName : string.Empty
            });
        }
        string json = JsonUtility.ToJson(new SerializationWrapper<InventoryItemData>(dataList));
        PlayerPrefs.SetString("PlayerInventory", json);
        PlayerPrefs.Save();
    }

    public void LoadInventory()
    {
        string json = PlayerPrefs.GetString("PlayerInventory", "");
        if (!string.IsNullOrEmpty(json))
        {
            var wrapper = JsonUtility.FromJson<SerializationWrapper<InventoryItemData>>(json);
            if (wrapper != null && wrapper.list != null)
            {
                // Clear existing inventory UI
                foreach (Transform child in inventorySpace.transform)
                {
                    Destroy(child.gameObject);
                }

                // Rebuild inventoryItems array and instantiate UI prefabs
                var itemsList = new List<InventoryItem>();
                foreach (var data in wrapper.list)
                {
                    // Instantiate the prefab
                    InventoryItem newItem = Instantiate(itemPrefab, inventorySpace.transform);

                    // Populate fields
                    newItem.technicalName = data.technicalName;
                    newItem.itemName = data.itemName;
                    newItem.itemType = data.itemType;
                    newItem.itemClothingCategory = data.itemClothingCategory;
                    newItem.itemDescription = data.itemDescription;
                    newItem.itemPrice = data.itemPrice;
                    newItem.itemBonuses = data.itemBonuses;
                    newItem.itemQuantity = data.itemQuantity;
                    newItem.isEquipped = data.isEquipped;
                    newItem.availableDialogue = string.IsNullOrEmpty(data.availableDialogueTechnicalName)
                        ? null
                        : ArticyDatabase.GetObject(data.availableDialogueTechnicalName) as ArticyObject;

                    // Optionally, reload icon from Articy if needed
                    var articyObj = ArticyDatabase.GetObject(data.technicalName) as Articy.Pale_Rider.Item;
                    if (articyObj != null)
                    {
                        var itemAsset = ((articyObj as IObjectWithPreviewImage).PreviewImage.Asset as Asset);
                        newItem.itemIcon = itemAsset != null ? itemAsset.LoadAssetAsSprite() : null;
                        if (newItem.itemIcon != null)
                        {
                            var imageComponent = newItem.GetComponent<Image>();
                            if (imageComponent != null)
                            {
                                imageComponent.sprite = newItem.itemIcon;
                            }
                        }
                    }

                    itemsList.Add(newItem);
                }
                inventoryItems = itemsList.ToArray();
            }
        }
    }

    public void InventoryItemChecker()
    {
        //Inventory Add Items
        if (ArticyGlobalVariables.Default.InventoryAddingStats.Revolver)
        {
            AddItem("Tool_KonstanzRevolver");
            ArticyGlobalVariables.Default.InventoryAddingStats.Revolver = false;
        }
        if (ArticyGlobalVariables.Default.InventoryAddingStats.RancherHat)
        {
            AddItem("Clothing_HisperianRancherHat");
            ArticyGlobalVariables.Default.InventoryAddingStats.RancherHat = false;
        }
        if (ArticyGlobalVariables.Default.InventoryAddingStats.Canister)
        {
            AddItem("Item_OxygenCanister");
            ArticyGlobalVariables.Default.InventoryAddingStats.Canister = false;
        }
        if (ArticyGlobalVariables.Default.InventoryAddingStats.GasMask)
        {
            AddItem("Clothing_GasMask");
            ArticyGlobalVariables.Default.InventoryAddingStats.GasMask = false;
        }
        if (ArticyGlobalVariables.Default.InventoryAddingStats.Rifle)
        {
            AddItem("Tool_KonstanzRifleBroken");
            ArticyGlobalVariables.Default.InventoryAddingStats.Rifle = false;
        }
        if (ArticyGlobalVariables.Default.InventoryAddingStats.Patch)
        {
            AddItem("Item_Patch");
            ArticyGlobalVariables.Default.InventoryAddingStats.Patch = false;
        }
        if (ArticyGlobalVariables.Default.InventoryAddingStats.RaikLockerKey)
        {
            AddItem("Item_FatherLockerKey");
            ArticyGlobalVariables.Default.InventoryAddingStats.RaikLockerKey = false;
        }
        if (ArticyGlobalVariables.Default.InventoryAddingStats.SafetyLamp)
        {
            AddItem("Tool_SafetyLamp");
            ArticyGlobalVariables.Default.InventoryAddingStats.SafetyLamp = false;
        }
        if (ArticyGlobalVariables.Default.InventoryAddingStats.Crowbar)
        {
            AddItem("Tool_Crowbar");
            ArticyGlobalVariables.Default.InventoryAddingStats.Crowbar = false;
        }
        if (ArticyGlobalVariables.Default.InventoryAddingStats.RaikFatherEnvelope)
        {
            AddItem("Interactable_FatherEnvelope");
            ArticyGlobalVariables.Default.InventoryAddingStats.RaikFatherEnvelope = false;
        }
    }

    public IEnumerator PopupCoroutine(string itemName)
    {
        itemPopup.SetActive(true);
        popupItemName.text = itemName;
        yield return new WaitForSeconds(5f);
        itemPopup.SetActive(false);
    }
    
    public void ClearInventory()
    {
        // Clear the inventory array
        inventoryItems = new InventoryItem[0];

        // Clear the inventory UI
        foreach (Transform child in inventorySpace.transform)
        {
            Destroy(child.gameObject);
        }

        // Remove saved inventory from PlayerPrefs
        PlayerPrefs.DeleteKey("PlayerInventory");
        PlayerPrefs.Save();
    }

    public void InitializeEquipment()
    {
        if (inventorySpace == null)
            inventorySpace = GameObject.FindGameObjectWithTag("InventorySpace");

        if (inventorySpace == null) return;

        var items = inventorySpace.GetComponentsInChildren<InventoryItem>(true);
        var itemsList = new List<InventoryItem>();

        foreach (var item in items)
        {
            if (item == null) continue;

            string tech = item.technicalName ?? "";
            bool equipped = false;

            if (!string.IsNullOrEmpty(ArticyGlobalVariables.Default.EquippedItems.EquippedTool) && ArticyGlobalVariables.Default.EquippedItems.EquippedTool == tech)
                equipped = true;
            if (!string.IsNullOrEmpty(ArticyGlobalVariables.Default.EquippedItems.EquippedHead) && ArticyGlobalVariables.Default.EquippedItems.EquippedHead == tech)
                equipped = true;
            if (!string.IsNullOrEmpty(ArticyGlobalVariables.Default.EquippedItems.EquippedFace) && ArticyGlobalVariables.Default.EquippedItems.EquippedFace == tech)
                equipped = true;
            if (!string.IsNullOrEmpty(ArticyGlobalVariables.Default.EquippedItems.EquippedNeck) && ArticyGlobalVariables.Default.EquippedItems.EquippedNeck == tech)
                equipped = true;
            if (!string.IsNullOrEmpty(ArticyGlobalVariables.Default.EquippedItems.EquippedBody) && ArticyGlobalVariables.Default.EquippedItems.EquippedBody == tech)
                equipped = true;
            if (!string.IsNullOrEmpty(ArticyGlobalVariables.Default.EquippedItems.EquippedHands) && ArticyGlobalVariables.Default.EquippedItems.EquippedHands == tech)
                equipped = true;
            if (!string.IsNullOrEmpty(ArticyGlobalVariables.Default.EquippedItems.EquippedLegs) && ArticyGlobalVariables.Default.EquippedItems.EquippedLegs == tech)
                equipped = true;
            if (!string.IsNullOrEmpty(ArticyGlobalVariables.Default.EquippedItems.EquippedFeet) && ArticyGlobalVariables.Default.EquippedItems.EquippedFeet == tech)
                equipped = true;
            if (!string.IsNullOrEmpty(ArticyGlobalVariables.Default.EquippedItems.HeldItem) && ArticyGlobalVariables.Default.EquippedItems.HeldItem == tech)
                equipped = true;

            item.isEquipped = equipped;
            itemsList.Add(item);
        }

        inventoryItems = itemsList.ToArray();
    }
}



// Helper for serializing lists
[System.Serializable]
public class SerializationWrapper<T>
{
    public List<T> list;
    public SerializationWrapper(List<T> list) { this.list = list; }
}

[System.Serializable]
public class InventoryItemData
{
    public string technicalName;
    public string itemName;
    public string itemType;
    public string itemClothingCategory;
    public string itemDescription;
    public int itemPrice;
    public string itemBonuses;
    public int itemQuantity;
    public bool isEquipped;
    public string availableDialogueTechnicalName;
    // Add other fields as needed (e.g., icon reference as a string)
}
