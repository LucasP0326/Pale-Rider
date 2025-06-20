using UnityEngine;
using UnityEngine.UI;
using Articy.Unity;
using Articy.Unity.Interfaces;
using Articy.Pale_Rider;
using Articy.Pale_Rider.GlobalVariables;

public class InventoryItem : MonoBehaviour
{
    //Important References
    private InventoryInterface inventoryInterface; // Reference to the InventoryInterface script

    public string itemName; // Name of the item
    public string technicalName; // Unique identifier for the item
    public string itemType; // Type of the item (Tools, Clothes, Items, Interact)
    public string itemDescription; // Description of the item
    public Sprite itemIcon; // Icon of the item
    public int itemQuantity; // Quantity of the item
    public int itemPrice; // Price of the item
    public bool isEquipped; // Whether the item is equipped or not

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inventoryInterface = GameObject.FindGameObjectWithTag("InventoryInterface").GetComponent<InventoryInterface>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnItemSelected()
    {
        // Handle item selection logic here
        inventoryInterface.selectedItem = this.gameObject;
        inventoryInterface.SelectItem();
        Debug.Log("Selected Item: " + itemName);
    }
}
