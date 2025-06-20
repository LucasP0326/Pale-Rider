using UnityEngine;
using UnityEngine.UI;
using Articy.Unity;
using Articy.Unity.Interfaces;
using Articy.Pale_Rider;
using Articy.Pale_Rider.GlobalVariables;

public class InventoryManager : MonoBehaviour
{
    public InventoryItem[] inventoryItems; // Array to hold all inventory items
    public InventoryItem itemPrefab; // Prefab for creating new inventory items
    public GameObject inventorySpace;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
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
        // newItem.itemPrice = (int)articyObj.Template.Price;

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
}
