using UnityEngine;
using Articy.Pale_Rider.GlobalVariables;

public class SaveManager : MonoBehaviour
{
    //Important References
    private InventoryManager inventoryManager;
    private PlayerStats playerStats;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inventoryManager = FindObjectOfType<InventoryManager>();
        playerStats = FindObjectOfType<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Call this to save
    public void SaveGame()
    {
        //Misc Scripts
        inventoryManager.SaveInventory();

        //Player Skill Stats
        PlayerPrefs.SetInt("Authority", ArticyGlobalVariables.Default.PlayerStats.Authority);
        PlayerPrefs.SetInt("Conceptualization", ArticyGlobalVariables.Default.PlayerStats.Conceptualization);
        PlayerPrefs.SetInt("Encyclopedia", ArticyGlobalVariables.Default.PlayerStats.Encyclopedia);
        PlayerPrefs.SetInt("Empathy", ArticyGlobalVariables.Default.PlayerStats.Empathy);
        PlayerPrefs.SetInt("Endurance", ArticyGlobalVariables.Default.PlayerStats.Endurance);
        PlayerPrefs.SetInt("Logic", ArticyGlobalVariables.Default.PlayerStats.Logic);
        PlayerPrefs.SetInt("Perception", ArticyGlobalVariables.Default.PlayerStats.Perception);
        PlayerPrefs.SetInt("Perspicacity", ArticyGlobalVariables.Default.PlayerStats.Perspicacity);
        PlayerPrefs.SetInt("Physicality", ArticyGlobalVariables.Default.PlayerStats.Physicality);
        PlayerPrefs.SetInt("Reflexivity", ArticyGlobalVariables.Default.PlayerStats.Reflexivity);
        PlayerPrefs.SetInt("Rhetoric", ArticyGlobalVariables.Default.PlayerStats.Rhetoric);
        PlayerPrefs.SetInt("SavoirFaire", ArticyGlobalVariables.Default.PlayerStats.SavoirFaire);
        PlayerPrefs.SetInt("SelfActualization", ArticyGlobalVariables.Default.PlayerStats.SelfActualization);
        PlayerPrefs.SetInt("Suggestion", ArticyGlobalVariables.Default.PlayerStats.Suggestion);
        PlayerPrefs.SetInt("Tenebrality", ArticyGlobalVariables.Default.PlayerStats.Tenebrality);
        PlayerPrefs.SetInt("Volition", ArticyGlobalVariables.Default.PlayerStats.Volition);
        PlayerPrefs.SetString("SignatureSkill", ArticyGlobalVariables.Default.PlayerStats.SignatureSkill);

        //Player State Stats
        PlayerPrefs.SetInt("Health", ArticyGlobalVariables.Default.PlayerStats.Health);
        PlayerPrefs.SetInt("MaxHealth", ArticyGlobalVariables.Default.PlayerStats.MaxHealth);
        PlayerPrefs.SetInt("Resolve", ArticyGlobalVariables.Default.PlayerStats.Resolve);
        PlayerPrefs.SetInt("MaxResolve", ArticyGlobalVariables.Default.PlayerStats.MaxResolve);

        //Skill Base Scores
        PlayerPrefs.SetInt("ReptilianBaseScore", ArticyGlobalVariables.Default.PlayerStats.ReptilianBaseScore);
        PlayerPrefs.SetInt("PaleoBaseScore", ArticyGlobalVariables.Default.PlayerStats.PaleoBaseScore);
        PlayerPrefs.SetInt("NeoBaseScore", ArticyGlobalVariables.Default.PlayerStats.NeoBaseScore);
        PlayerPrefs.SetInt("PaleBaseScore", ArticyGlobalVariables.Default.PlayerStats.PaleBaseScore);

        //Player Variables
        PlayerPrefs.SetInt("FoundGasMask", ArticyGlobalVariables.Default.PlayerVariables.FoundGasMask ? 1 : 0);
        PlayerPrefs.SetInt("FoundGasMask", ArticyGlobalVariables.Default.PlayerVariables.FoundGasMask ? 1 : 0);
        PlayerPrefs.SetInt("HasEquipment", ArticyGlobalVariables.Default.PlayerVariables.HasEquipment ? 1 : 0);
        PlayerPrefs.SetInt("IdentityCrisis", ArticyGlobalVariables.Default.PlayerVariables.IdentityCrisis ? 1 : 0);

        //Global Variables
        PlayerPrefs.SetInt("MadeBed", ArticyGlobalVariables.Default.GlobalVariables.MadeBed ? 1 : 0);
        PlayerPrefs.SetInt("TalkedToIngo", ArticyGlobalVariables.Default.GlobalVariables.TalkedToIngo ? 1 : 0);
        PlayerPrefs.SetInt("LeftRoom", ArticyGlobalVariables.Default.GlobalVariables.LeftRoom ? 1 : 0);
        PlayerPrefs.SetInt("WokeUp", ArticyGlobalVariables.Default.GlobalVariables.WokeUp ? 1 : 0);
        PlayerPrefs.SetInt("AssigningSkills", ArticyGlobalVariables.Default.GlobalVariables.AssigningSkills ? 1 : 0);
        PlayerPrefs.SetInt("AssignedSkills", ArticyGlobalVariables.Default.GlobalVariables.AssignedSkills ? 1 : 0);
        PlayerPrefs.SetInt("UnlockedInventory", ArticyGlobalVariables.Default.GlobalVariables.UnlockedInventory ? 1 : 0);
        PlayerPrefs.SetInt("UnlockedSkills", ArticyGlobalVariables.Default.GlobalVariables.UnlockedSkills ? 1 : 0);
        PlayerPrefs.SetInt("UnlockedQuests", ArticyGlobalVariables.Default.GlobalVariables.UnlockedQuests ? 1 : 0);
        PlayerPrefs.SetInt("UnlockedMap", ArticyGlobalVariables.Default.GlobalVariables.UnlockedMap ? 1 : 0);
        PlayerPrefs.SetInt("Time", ArticyGlobalVariables.Default.GlobalVariables.Time);

        PlayerPrefs.Save();
        Debug.Log("Game Saved!");
    }

    // Call this to load
    public void LoadGame()
    {
        // Load Misc Scripts
        inventoryManager.LoadInventory();

        // Load Player Skill Stats
        ArticyGlobalVariables.Default.PlayerStats.Authority = PlayerPrefs.GetInt("Authority", 0);
        ArticyGlobalVariables.Default.PlayerStats.Conceptualization = PlayerPrefs.GetInt("Conceptualization", 0);
        ArticyGlobalVariables.Default.PlayerStats.Encyclopedia = PlayerPrefs.GetInt("Encyclopedia", 0);
        ArticyGlobalVariables.Default.PlayerStats.Empathy = PlayerPrefs.GetInt("Empathy", 0);
        ArticyGlobalVariables.Default.PlayerStats.Endurance = PlayerPrefs.GetInt("Endurance", 0);
        ArticyGlobalVariables.Default.PlayerStats.Logic = PlayerPrefs.GetInt("Logic", 0);
        ArticyGlobalVariables.Default.PlayerStats.Perception = PlayerPrefs.GetInt("Perception", 0);
        ArticyGlobalVariables.Default.PlayerStats.Perspicacity = PlayerPrefs.GetInt("Perspicacity", 0);
        ArticyGlobalVariables.Default.PlayerStats.Physicality = PlayerPrefs.GetInt("Physicality", 0);
        ArticyGlobalVariables.Default.PlayerStats.Reflexivity = PlayerPrefs.GetInt("Reflexivity", 0);
        ArticyGlobalVariables.Default.PlayerStats.Rhetoric = PlayerPrefs.GetInt("Rhetoric", 0);
        ArticyGlobalVariables.Default.PlayerStats.SavoirFaire = PlayerPrefs.GetInt("SavoirFaire", 0);
        ArticyGlobalVariables.Default.PlayerStats.SelfActualization = PlayerPrefs.GetInt("SelfActualization", 0);
        ArticyGlobalVariables.Default.PlayerStats.Suggestion = PlayerPrefs.GetInt("Suggestion", 0);
        ArticyGlobalVariables.Default.PlayerStats.Tenebrality = PlayerPrefs.GetInt("Tenebrality", 0);
        ArticyGlobalVariables.Default.PlayerStats.Volition = PlayerPrefs.GetInt("Volition", 0);
        ArticyGlobalVariables.Default.PlayerStats.SignatureSkill = PlayerPrefs.GetString("SignatureSkill", "DefaultSkill");

        // Player State Stats
        ArticyGlobalVariables.Default.PlayerStats.Health = PlayerPrefs.GetInt("Health", 0);
        ArticyGlobalVariables.Default.PlayerStats.MaxHealth = PlayerPrefs.GetInt("MaxHealth", 0);
        ArticyGlobalVariables.Default.PlayerStats.Resolve = PlayerPrefs.GetInt("Resolve", 0);
        ArticyGlobalVariables.Default.PlayerStats.MaxResolve = PlayerPrefs.GetInt("MaxResolve", 0);

        // Skill Base Scores
        ArticyGlobalVariables.Default.PlayerStats.ReptilianBaseScore = PlayerPrefs.GetInt("ReptilianBaseScore", 0);
        ArticyGlobalVariables.Default.PlayerStats.PaleoBaseScore = PlayerPrefs.GetInt("PaleoBaseScore", 0);
        ArticyGlobalVariables.Default.PlayerStats.NeoBaseScore = PlayerPrefs.GetInt("NeoBaseScore", 0);
        ArticyGlobalVariables.Default.PlayerStats.PaleBaseScore = PlayerPrefs.GetInt("PaleBaseScore", 0);

        // Player Variables (bools)
        ArticyGlobalVariables.Default.PlayerVariables.FoundGasMask = PlayerPrefs.GetInt("FoundGasMask", 0) == 1;
        ArticyGlobalVariables.Default.PlayerVariables.HasEquipment = PlayerPrefs.GetInt("HasEquipment", 0) == 1;
        ArticyGlobalVariables.Default.PlayerVariables.IdentityCrisis = PlayerPrefs.GetInt("IdentityCrisis", 0) == 1;

        // Global Variables (bools)
        ArticyGlobalVariables.Default.GlobalVariables.MadeBed = PlayerPrefs.GetInt("MadeBed", 0) == 1;
        ArticyGlobalVariables.Default.GlobalVariables.TalkedToIngo = PlayerPrefs.GetInt("TalkedToIngo", 0) == 1;
        ArticyGlobalVariables.Default.GlobalVariables.LeftRoom = PlayerPrefs.GetInt("LeftRoom", 0) == 1;
        ArticyGlobalVariables.Default.GlobalVariables.WokeUp = PlayerPrefs.GetInt("WokeUp", 0) == 1;
        ArticyGlobalVariables.Default.GlobalVariables.AssigningSkills = PlayerPrefs.GetInt("AssigningSkills", 0) == 1;
        ArticyGlobalVariables.Default.GlobalVariables.AssignedSkills = PlayerPrefs.GetInt("AssignedSkills", 0) == 1;
        ArticyGlobalVariables.Default.GlobalVariables.UnlockedInventory = PlayerPrefs.GetInt("UnlockedInventory", 0) == 1;
        ArticyGlobalVariables.Default.GlobalVariables.UnlockedSkills = PlayerPrefs.GetInt("UnlockedSkills", 0) == 1;
        ArticyGlobalVariables.Default.GlobalVariables.UnlockedQuests = PlayerPrefs.GetInt("UnlockedQuests", 0) == 1;
        ArticyGlobalVariables.Default.GlobalVariables.UnlockedMap = PlayerPrefs.GetInt("UnlockedMap", 0) == 1;

        // Time
        ArticyGlobalVariables.Default.GlobalVariables.Time = PlayerPrefs.GetInt("Time", 8 * 60);
        playerStats.UpdatePlayerStats();

        Debug.Log("Game Loaded!");
    }
    
    public void ResetGame()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("Game Reset!");
    }
}
