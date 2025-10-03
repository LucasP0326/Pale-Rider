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
        newItem.itemDescription = articyObj.Template.Description.MediumTextValue;
        newItem.itemPrice = (int)articyObj.Template.Price.NumberValue;

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
                itemDescription = item.itemDescription,
                itemPrice = item.itemPrice,
                itemQuantity = item.itemQuantity,
                isEquipped = item.isEquipped
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
                    newItem.itemDescription = data.itemDescription;
                    newItem.itemPrice = data.itemPrice;
                    newItem.itemQuantity = data.itemQuantity;
                    newItem.isEquipped = data.isEquipped;

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
    public string itemDescription;
    public int itemPrice;
    public int itemQuantity;
    public bool isEquipped;
    // Add other fields as needed (e.g., icon reference as a string)
}
