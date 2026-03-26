using UnityEngine;
using UnityEngine.UI;
using Articy.Unity;
using Articy.Unity.Interfaces;
using Articy.Pale_Rider;
using Articy.Pale_Rider.GlobalVariables;

public class InventoryItem : MonoBehaviour
{
    //Important References
    public InventoryInterface inventoryInterface; // Reference to the InventoryInterface script

    public string itemName; // Name of the item
    public string technicalName; // Unique identifier for the item
    public string itemType; // Type of the item (Tools, Clothes, Items, Interact)
    public string itemClothingCategory; // Clothing category (Face, Head, Eyes, Body, Legs, Feet, Hands, Neck)
    public string itemDescription; // Description of the item
    public Sprite itemIcon; // Icon of the item
    public int itemQuantity; // Quantity of the item
    public int itemPrice; // Price of the item
    public string itemBonuses; // Bonuses provided by the item (e.g., "+2 Endurance, +1 Perception")
    public bool isEquipped; // Whether the item is equipped or not
    //Bonuses
    public string[] bonuses;
    public string[] parts;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inventoryInterface = GameObject.FindFirstObjectByType<InventoryInterface>();
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

    public void FindBonuses()
    {
        // Logic to parse itemBonuses string and apply bonuses to player stats
        // This is a placeholder implementation and should be expanded based on the actual format of itemBonuses
        Debug.Log("Finding Bonuses for: " + itemName);
        bonuses = itemBonuses.Split(',');
        //Bonus written as "Endurance: +2" or "Perception: -1" or etc.
        foreach (string bonus in bonuses)
        {
            string[] parts = bonus.Split(':');
            if (parts.Length != 2)
            {
                Debug.LogWarning("Invalid bonus format: " + bonus);
                continue;
            }
            string stat = parts[0].Trim();
            int value;
            if (!int.TryParse(parts[1].Trim(), out value))
            {
                Debug.LogWarning("Invalid bonus value: " + parts[1].Trim());
                continue;
            }
            Debug.Log("Applying Bonus: " + value + " to " + stat);
            ApplyBonus(stat, value);
        }
    }

    public void ApplyBonus(string stat, int value)
    {
        // Logic to apply the bonus to the player's stats
        // This is a placeholder implementation and should be expanded based on the actual player stats structure
        ArticyGlobalVariables globalVariables = Resources.Load<ArticyGlobalVariables>("ArticyGlobalVariables");
        if (isEquipped)
        {
            switch (stat)
            {
                case "Endurance":
                    ArticyGlobalVariables.Default.PlayerStats.Endurance += value;
                    ArticyGlobalVariables.Default.ItemStatVariables.EnduranceItem += value;
                    break;
                case "Perception":
                    ArticyGlobalVariables.Default.PlayerStats.Perception += value;
                    ArticyGlobalVariables.Default.ItemStatVariables.PerceptionItem += value;
                    break;
                case "Authority":
                    ArticyGlobalVariables.Default.PlayerStats.Authority += value;
                    ArticyGlobalVariables.Default.ItemStatVariables.AuthorityItem += value;
                    break;
                case "Conceptualization":
                    ArticyGlobalVariables.Default.PlayerStats.Conceptualization += value;
                    ArticyGlobalVariables.Default.ItemStatVariables.ConceptualizationItem += value;
                    break;
                case "Encyclopedia":
                    ArticyGlobalVariables.Default.PlayerStats.Encyclopedia += value;
                    ArticyGlobalVariables.Default.ItemStatVariables.EncyclopediaItem += value;
                    break;
                case "Empathy":
                    ArticyGlobalVariables.Default.PlayerStats.Empathy += value;
                    ArticyGlobalVariables.Default.ItemStatVariables.EmpathyItem += value;
                    break;
                case "Logic":
                    ArticyGlobalVariables.Default.PlayerStats.Logic += value;
                    ArticyGlobalVariables.Default.ItemStatVariables.LogicItem += value;
                    break;
                case "Perspicacity":
                    ArticyGlobalVariables.Default.PlayerStats.Perspicacity += value;
                    ArticyGlobalVariables.Default.ItemStatVariables.PerspicacityItem += value;
                    break; 
                case "Physicality":
                    ArticyGlobalVariables.Default.PlayerStats.Physicality += value;
                    ArticyGlobalVariables.Default.ItemStatVariables.PhysicalityItem += value;
                    break;
                case "Reflexivity":
                    ArticyGlobalVariables.Default.PlayerStats.Reflexivity += value;
                    ArticyGlobalVariables.Default.ItemStatVariables.ReflexivityItem += value;
                    break; 
                case "Rhetoric":
                    ArticyGlobalVariables.Default.PlayerStats.Rhetoric += value;
                    ArticyGlobalVariables.Default.ItemStatVariables.RhetoricItem += value;
                    break; 
                case "SavoirFaire":
                    ArticyGlobalVariables.Default.PlayerStats.SavoirFaire += value;
                    ArticyGlobalVariables.Default.ItemStatVariables.SavoirFaireItem += value;
                    break; 
                case "SelfActualization":
                    ArticyGlobalVariables.Default.PlayerStats.SelfActualization += value;
                    ArticyGlobalVariables.Default.ItemStatVariables.SelfActualizationItem += value;
                    break; 
                case "Suggestion":
                    ArticyGlobalVariables.Default.PlayerStats.Suggestion += value;
                    ArticyGlobalVariables.Default.ItemStatVariables.SuggestionItem += value;
                    break; 
                case "Tenebrality":
                    ArticyGlobalVariables.Default.PlayerStats.Tenebrality += value;
                    ArticyGlobalVariables.Default.ItemStatVariables.TenebralityItem += value;
                    break; 
                case "Volition":
                    ArticyGlobalVariables.Default.PlayerStats.Volition += value;
                    ArticyGlobalVariables.Default.ItemStatVariables.VolitionItem += value;
                    break; 
                default:
                    Debug.LogWarning("Unknown stat: " + stat);
                    break;
            }
            Debug.Log("Applied Bonus: " + value + " to " + stat);
        }
        else if (!isEquipped)
        {
            switch (stat)
            {
                case "Endurance":
                    ArticyGlobalVariables.Default.PlayerStats.Endurance -= value;
                    ArticyGlobalVariables.Default.ItemStatVariables.EnduranceItem -= value;
                    break;
                case "Perception":
                    ArticyGlobalVariables.Default.PlayerStats.Perception -= value;
                    ArticyGlobalVariables.Default.ItemStatVariables.PerceptionItem -= value;
                    break;
                case "Authority":
                    ArticyGlobalVariables.Default.PlayerStats.Authority -= value;
                    ArticyGlobalVariables.Default.ItemStatVariables.AuthorityItem -= value;
                    break;
                case "Conceptualization":
                    ArticyGlobalVariables.Default.PlayerStats.Conceptualization -= value;
                    ArticyGlobalVariables.Default.ItemStatVariables.ConceptualizationItem -= value;
                    break;
                case "Encyclopedia":
                    ArticyGlobalVariables.Default.PlayerStats.Encyclopedia -= value;
                    ArticyGlobalVariables.Default.ItemStatVariables.EncyclopediaItem -= value;
                    break;
                case "Empathy":
                    ArticyGlobalVariables.Default.PlayerStats.Empathy -= value;
                    ArticyGlobalVariables.Default.ItemStatVariables.EmpathyItem -= value;
                    break;
                case "Logic":
                    ArticyGlobalVariables.Default.PlayerStats.Logic -= value;
                    ArticyGlobalVariables.Default.ItemStatVariables.LogicItem -= value;
                    break;
                case "Perspicacity":
                    ArticyGlobalVariables.Default.PlayerStats.Perspicacity -= value;
                    ArticyGlobalVariables.Default.ItemStatVariables.PerspicacityItem -= value;
                    break; 
                case "Physicality":
                    ArticyGlobalVariables.Default.PlayerStats.Physicality -= value;
                    ArticyGlobalVariables.Default.ItemStatVariables.PhysicalityItem -= value;
                    break;
                case "Reflexivity":
                    ArticyGlobalVariables.Default.PlayerStats.Reflexivity -= value;
                    ArticyGlobalVariables.Default.ItemStatVariables.ReflexivityItem -= value;
                    break; 
                case "Rhetoric":
                    ArticyGlobalVariables.Default.PlayerStats.Rhetoric -= value;
                    ArticyGlobalVariables.Default.ItemStatVariables.RhetoricItem -= value;
                    break; 
                case "SavoirFaire":
                    ArticyGlobalVariables.Default.PlayerStats.SavoirFaire -= value;
                    ArticyGlobalVariables.Default.ItemStatVariables.SavoirFaireItem -= value;
                    break; 
                case "SelfActualization":
                    ArticyGlobalVariables.Default.PlayerStats.SelfActualization -= value;
                    ArticyGlobalVariables.Default.ItemStatVariables.SelfActualizationItem -= value;
                    break; 
                case "Suggestion":
                    ArticyGlobalVariables.Default.PlayerStats.Suggestion -= value;
                    ArticyGlobalVariables.Default.ItemStatVariables.SuggestionItem -= value;
                    break; 
                case "Tenebrality":
                    ArticyGlobalVariables.Default.PlayerStats.Tenebrality -= value;
                    ArticyGlobalVariables.Default.ItemStatVariables.TenebralityItem -= value;
                    break; 
                case "Volition":
                    ArticyGlobalVariables.Default.PlayerStats.Volition -= value;
                    ArticyGlobalVariables.Default.ItemStatVariables.VolitionItem -= value;
                    break; 
                default:
                    Debug.LogWarning("Unknown stat: " + stat);
                    break;
            }
            Debug.Log("Removed Bonus: " + value + " from " + stat);
        }
    }
}
