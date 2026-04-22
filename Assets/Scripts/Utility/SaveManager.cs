using UnityEngine;
using Articy.Pale_Rider.GlobalVariables;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Reflection;
using System;

public class SaveManager : MonoBehaviour
{
    //Important References
    private InventoryManager inventoryManager;
    private QuestManager questManager;
    private PlayerStats playerStats;
    private DialogueManager dialogueManager;
    private TimeManager TimeManager;
    public string sceneName;
    public Vector3 playerPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inventoryManager = FindFirstObjectByType<InventoryManager>();
        questManager = FindFirstObjectByType<QuestManager>();
        playerStats = FindFirstObjectByType<PlayerStats>();
        dialogueManager = FindFirstObjectByType<DialogueManager>();
        if (ArticyGlobalVariables.Default.GlobalVariables.LoadingGame)
            LoadGame();
        TimeManager = FindObjectOfType<TimeManager>();
    }

    // Update is called once per frame
    void Update()
    {
        sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        if (GameObject.FindGameObjectWithTag("Player") != null)
            playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
    }

    // Call this to save
    public void SaveGame()
    {
        //Misc Scripts
        inventoryManager.SaveInventory();

        //Player Location
        PlayerPrefs.SetString("SceneName", sceneName);
        PlayerPrefs.SetFloat("PlayerPosX", playerPosition.x);
        PlayerPrefs.SetFloat("PlayerPosY", playerPosition.y);
        PlayerPrefs.SetFloat("PlayerPosZ", playerPosition.z);

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
        PlayerPrefs.SetInt("Experience", ArticyGlobalVariables.Default.PlayerStats.Experience);

        //Player Moral Stats
        PlayerPrefs.SetInt("Apocalypse_Rider", ArticyGlobalVariables.Default.PlayerStats.Apocalypse_Rider);
        PlayerPrefs.SetInt("Hope_Rider", ArticyGlobalVariables.Default.PlayerStats.Hope_Rider);
        PlayerPrefs.SetInt("Order_Rider", ArticyGlobalVariables.Default.PlayerStats.Order_Rider);
        PlayerPrefs.SetInt("Anarchist_Rider", ArticyGlobalVariables.Default.PlayerStats.Anarchist_Rider);
        PlayerPrefs.SetInt("Kindness_Rider", ArticyGlobalVariables.Default.PlayerStats.Kindness_Rider);
        PlayerPrefs.SetInt("Ambiguity_Rider", ArticyGlobalVariables.Default.PlayerStats.Ambiguity_Rider);

        //Player Faith Stats
        PlayerPrefs.SetInt("Faith_Apostolic", ArticyGlobalVariables.Default.PlayerStats.Faith_Apostolic);
        PlayerPrefs.SetInt("Faith_Adventist", ArticyGlobalVariables.Default.PlayerStats.Faith_Adventist);
        PlayerPrefs.SetInt("Faith_Iconoclast", ArticyGlobalVariables.Default.PlayerStats.Faith_Iconoclast);
        PlayerPrefs.SetInt("Faith_Atheist", ArticyGlobalVariables.Default.PlayerStats.Faith_Atheist);

        //Player Item Stats
        PlayerPrefs.SetInt("AuthorityItem", ArticyGlobalVariables.Default.ItemStatVariables.AuthorityItem);
        PlayerPrefs.SetInt("ConceptualizationItem", ArticyGlobalVariables.Default.ItemStatVariables.ConceptualizationItem);
        PlayerPrefs.SetInt("EncyclopediaItem", ArticyGlobalVariables.Default.ItemStatVariables.EncyclopediaItem);
        PlayerPrefs.SetInt("EmpathyItem", ArticyGlobalVariables.Default.ItemStatVariables.EmpathyItem);
        PlayerPrefs.SetInt("EnduranceItem", ArticyGlobalVariables.Default.ItemStatVariables.EnduranceItem);
        PlayerPrefs.SetInt("LogicItem", ArticyGlobalVariables.Default.ItemStatVariables.LogicItem);
        PlayerPrefs.SetInt("PerceptionItem", ArticyGlobalVariables.Default.ItemStatVariables.PerceptionItem);
        PlayerPrefs.SetInt("PerspicacityItem", ArticyGlobalVariables.Default.ItemStatVariables.PerspicacityItem);
        PlayerPrefs.SetInt("PhysicalityItem", ArticyGlobalVariables.Default.ItemStatVariables.PhysicalityItem);
        PlayerPrefs.SetInt("ReflexivityItem", ArticyGlobalVariables.Default.ItemStatVariables.ReflexivityItem);
        PlayerPrefs.SetInt("RhetoricItem", ArticyGlobalVariables.Default.ItemStatVariables.RhetoricItem);
        PlayerPrefs.SetInt("SavoirFaireItem", ArticyGlobalVariables.Default.ItemStatVariables.SavoirFaireItem);
        PlayerPrefs.SetInt("SelfActualizationItem", ArticyGlobalVariables.Default.ItemStatVariables.SelfActualizationItem);
        PlayerPrefs.SetInt("SuggestionItem", ArticyGlobalVariables.Default.ItemStatVariables.SuggestionItem);
        PlayerPrefs.SetInt("TenebralityItem", ArticyGlobalVariables.Default.ItemStatVariables.TenebralityItem);
        PlayerPrefs.SetInt("VolitionItem", ArticyGlobalVariables.Default.ItemStatVariables.VolitionItem);

        //Player State Stats
        PlayerPrefs.SetInt("Health", ArticyGlobalVariables.Default.PlayerStats.Health);
        PlayerPrefs.SetInt("MaxHealth", ArticyGlobalVariables.Default.PlayerStats.MaxHealth);
        PlayerPrefs.SetInt("Resolve", ArticyGlobalVariables.Default.PlayerStats.Resolve);
        PlayerPrefs.SetInt("MaxResolve", ArticyGlobalVariables.Default.PlayerStats.MaxResolve);
        PlayerPrefs.SetInt("Money", ArticyGlobalVariables.Default.PlayerStats.Money);

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
        PlayerPrefs.SetInt("SucumbingToPale", ArticyGlobalVariables.Default.PlayerVariables.SucumbingToPale ? 1 : 0);
        PlayerPrefs.SetInt("PhysicalDeath", ArticyGlobalVariables.Default.PlayerVariables.PhysicalDeath ? 1 : 0);
        PlayerPrefs.SetInt("ResolveDeath", ArticyGlobalVariables.Default.PlayerVariables.ResolveDeath ? 1 : 0);
        PlayerPrefs.SetInt("JesusComplex", ArticyGlobalVariables.Default.PlayerVariables.JesusComplex);
        PlayerPrefs.SetInt("KnowsTheConcord", ArticyGlobalVariables.Default.PlayerVariables.KnowsTheConcord ? 1 : 0);
        PlayerPrefs.SetInt("KnowWhoYouAre", ArticyGlobalVariables.Default.PlayerVariables.KnowWhoYouAre ? 1 : 0);
        PlayerPrefs.SetInt("MistakenIdentity", ArticyGlobalVariables.Default.PlayerVariables.MistakenIdentity);
        PlayerPrefs.SetInt("AlcoholConsumed", ArticyGlobalVariables.Default.PlayerVariables.AlcoholConsumed);
        PlayerPrefs.SetInt("KnowsAboutJesus", ArticyGlobalVariables.Default.PlayerVariables.KnowsAboutJesus ? 1 : 0);

        //Equipped Items
        PlayerPrefs.SetString("EquippedHead", ArticyGlobalVariables.Default.EquippedItems.EquippedHead);
        PlayerPrefs.SetString("EquippedFace", ArticyGlobalVariables.Default.EquippedItems.EquippedFace);
        PlayerPrefs.SetString("EquippedNeck", ArticyGlobalVariables.Default.EquippedItems.EquippedNeck);
        PlayerPrefs.SetString("EquippedBody", ArticyGlobalVariables.Default.EquippedItems.EquippedBody);
        PlayerPrefs.SetString("EquippedLegs", ArticyGlobalVariables.Default.EquippedItems.EquippedLegs);
        PlayerPrefs.SetString("EquippedFeet", ArticyGlobalVariables.Default.EquippedItems.EquippedFeet);
        PlayerPrefs.SetString("EquippedHands", ArticyGlobalVariables.Default.EquippedItems.EquippedHands);
        PlayerPrefs.SetString("EquippedTool", ArticyGlobalVariables.Default.EquippedItems.EquippedTool);

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
        PlayerPrefs.SetInt("IngoPaleRealization", ArticyGlobalVariables.Default.GlobalVariables.IngoPaleRealization ? 1 : 0);
        PlayerPrefs.SetInt("Time", ArticyGlobalVariables.Default.GlobalVariables.Time);
        PlayerPrefs.SetInt("KeptHorse", ArticyGlobalVariables.Default.GlobalVariables.KeptHorse ? 1 : 0);
        PlayerPrefs.SetInt("ZuretonInnFirstTime", ArticyGlobalVariables.Default.GlobalVariables.ZuretonInnFirstTime ? 1 : 0);
        PlayerPrefs.SetInt("InDialogue", ArticyGlobalVariables.Default.GlobalVariables.InDialogue ? 1 : 0);
        PlayerPrefs.SetString("CurrentDialogue", ArticyGlobalVariables.Default.GlobalVariables.CurrentDialogueTechnicalName);

        //Raik Variables
        PlayerPrefs.SetInt("RaikOpinion", ArticyGlobalVariables.Default.RaikVariables.RaikOpinion);
        PlayerPrefs.SetInt("AskedAboutAnger", ArticyGlobalVariables.Default.RaikVariables.AskedAboutAnger ? 1 : 0);
        PlayerPrefs.SetInt("JobDislikeRevealed", ArticyGlobalVariables.Default.RaikVariables.JobDislikeRevealed ? 1 : 0);
        PlayerPrefs.SetInt("OpeningQuestionsAsked", ArticyGlobalVariables.Default.RaikVariables.OpeningQuestionsAsked);
        PlayerPrefs.SetInt("NameKnown", ArticyGlobalVariables.Default.RaikVariables.NameKnown ? 1 : 0);
        PlayerPrefs.SetInt("FinishedAlcohol", ArticyGlobalVariables.Default.RaikVariables.FinishedAlcohol ? 1 : 0);
        PlayerPrefs.SetInt("TabPaid", ArticyGlobalVariables.Default.RaikVariables.TabPaid ? 1 : 0);
        PlayerPrefs.SetInt("RoomDiscount", ArticyGlobalVariables.Default.RaikVariables.RoomDiscount);
        PlayerPrefs.SetInt("KnowsFatherSexuality", ArticyGlobalVariables.Default.RaikVariables.KnowsFatherSexuality ? 1 : 0);
        PlayerPrefs.SetInt("HatesFather", ArticyGlobalVariables.Default.RaikVariables.HatesFather ? 1 : 0);
        PlayerPrefs.SetInt("KnowsSchwarstein", ArticyGlobalVariables.Default.RaikVariables.KnowsSchwarstein ? 1 : 0);

        //Alina Variables
        PlayerPrefs.SetInt("RaikAunt", ArticyGlobalVariables.Default.AlinaVariables.RaikAunt ? 1 : 0);
        PlayerPrefs.SetInt("LavenderKnown", ArticyGlobalVariables.Default.AlinaVariables.LavenderKnown ? 1 : 0);
        PlayerPrefs.SetInt("Known", ArticyGlobalVariables.Default.AlinaVariables.Known ? 1 : 0);

        //Zureton Variables
        PlayerPrefs.SetInt("MineKnown", ArticyGlobalVariables.Default.ZuretonVariables.MineKnown ? 1 : 0);
        PlayerPrefs.SetInt("PaleDanger", ArticyGlobalVariables.Default.ZuretonVariables.PaleDanger);

        //Quests
        PlayerPrefs.SetInt("LeaveThePale", ArticyGlobalVariables.Default.Quests.LeaveThePale);
        PlayerPrefs.SetInt("PayInnTab", ArticyGlobalVariables.Default.Quests.PayInnTab);
        PlayerPrefs.SetInt("GetNewOutfit", ArticyGlobalVariables.Default.Quests.GetNewOutfit);
        PlayerPrefs.SetInt("FindBeerNewHome", ArticyGlobalVariables.Default.Quests.FindBeerNewHome);
        PlayerPrefs.SetInt("UncoverFatherRelationship", ArticyGlobalVariables.Default.Quests.UncoverFatherRelationship);
        PlayerPrefs.SetInt("LearnAboutJesus", ArticyGlobalVariables.Default.Quests.LearnAboutJesus);

        // Save all Articy-generated variables (ints, bools, strings) reflectively
        SaveArticyVariables();
        PlayerPrefs.Save();
        Debug.Log("Game Saved!");
    }

    // Call this to load
    public void LoadGame()
    {
        ArticyGlobalVariables.Default.GlobalVariables.LoadingGame = true;
        // Load Misc Scripts
        inventoryManager.LoadInventory();

        //Player Position
        sceneName = PlayerPrefs.GetString("SceneName", "SampleScene");
        float posX = PlayerPrefs.GetFloat("PlayerPosX", 0f);
        float posY = PlayerPrefs.GetFloat("PlayerPosY", 0f);
        float posZ = PlayerPrefs.GetFloat("PlayerPosZ", 0f);

        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != sceneName)
        {
            StartCoroutine(LoadSceneAndTeleport(sceneName, new Vector3(posX, posY, posZ)));
            return; // Stop further loading until scene is loaded
        }
        else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == sceneName)
        {
            TeleportToLoadLocation(new Vector3(posX, posY, posZ));
        }

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
        ArticyGlobalVariables.Default.PlayerStats.SignatureSkill = PlayerPrefs.GetString("SignatureSkill", "");
        ArticyGlobalVariables.Default.PlayerStats.Experience = PlayerPrefs.GetInt("Experience", 0);

        // Load Player Moral Stats
        ArticyGlobalVariables.Default.PlayerStats.Apocalypse_Rider = PlayerPrefs.GetInt("Apocalypse_Rider", 0);
        ArticyGlobalVariables.Default.PlayerStats.Hope_Rider = PlayerPrefs.GetInt("Hope_Rider", 0);
        ArticyGlobalVariables.Default.PlayerStats.Order_Rider = PlayerPrefs.GetInt("Order_Rider", 0);
        ArticyGlobalVariables.Default.PlayerStats.Anarchist_Rider = PlayerPrefs.GetInt("Anarchist_Rider", 0);
        ArticyGlobalVariables.Default.PlayerStats.Kindness_Rider = PlayerPrefs.GetInt("Kindness_Rider", 0);
        ArticyGlobalVariables.Default.PlayerStats.Ambiguity_Rider = PlayerPrefs.GetInt("Ambiguity_Rider", 0);

        // Load Player Faith Stats
        ArticyGlobalVariables.Default.PlayerStats.Faith_Apostolic = PlayerPrefs.GetInt("Faith_Apostolic", 0);
        ArticyGlobalVariables.Default.PlayerStats.Faith_Adventist = PlayerPrefs.GetInt("Faith_Adventist", 0);
        ArticyGlobalVariables.Default.PlayerStats.Faith_Iconoclast = PlayerPrefs.GetInt("Faith_Iconoclast", 0);
        ArticyGlobalVariables.Default.PlayerStats.Faith_Atheist = PlayerPrefs.GetInt("Faith_Atheist", 0);

        // Load Player Item Stats
        ArticyGlobalVariables.Default.ItemStatVariables.AuthorityItem = PlayerPrefs.GetInt("AuthorityItem", 0);
        ArticyGlobalVariables.Default.ItemStatVariables.ConceptualizationItem = PlayerPrefs.GetInt("ConceptualizationItem", 0);
        ArticyGlobalVariables.Default.ItemStatVariables.EncyclopediaItem = PlayerPrefs.GetInt("EncyclopediaItem", 0);
        ArticyGlobalVariables.Default.ItemStatVariables.EmpathyItem = PlayerPrefs.GetInt("EmpathyItem", 0);
        ArticyGlobalVariables.Default.ItemStatVariables.EnduranceItem = PlayerPrefs.GetInt("EnduranceItem", 0);
        ArticyGlobalVariables.Default.ItemStatVariables.LogicItem = PlayerPrefs.GetInt("LogicItem", 0);
        ArticyGlobalVariables.Default.ItemStatVariables.PerceptionItem = PlayerPrefs.GetInt("PerceptionItem", 0);
        ArticyGlobalVariables.Default.ItemStatVariables.PerspicacityItem = PlayerPrefs.GetInt("PerspicacityItem", 0);
        ArticyGlobalVariables.Default.ItemStatVariables.PhysicalityItem = PlayerPrefs.GetInt("PhysicalityItem", 0);
        ArticyGlobalVariables.Default.ItemStatVariables.ReflexivityItem = PlayerPrefs.GetInt("ReflexivityItem", 0);
        ArticyGlobalVariables.Default.ItemStatVariables.RhetoricItem = PlayerPrefs.GetInt("RhetoricItem", 0);
        ArticyGlobalVariables.Default.ItemStatVariables.SavoirFaireItem = PlayerPrefs.GetInt("SavoirFaireItem", 0);
        ArticyGlobalVariables.Default.ItemStatVariables.SelfActualizationItem = PlayerPrefs.GetInt("SelfActualizationItem", 0);
        ArticyGlobalVariables.Default.ItemStatVariables.SuggestionItem = PlayerPrefs.GetInt("SuggestionItem", 0);
        ArticyGlobalVariables.Default.ItemStatVariables.TenebralityItem = PlayerPrefs.GetInt("TenebralityItem", 0);
        ArticyGlobalVariables.Default.ItemStatVariables.VolitionItem = PlayerPrefs.GetInt("VolitionItem", 0);

        // Player State Stats
        ArticyGlobalVariables.Default.PlayerStats.Health = PlayerPrefs.GetInt("Health", 1);
        ArticyGlobalVariables.Default.PlayerStats.MaxHealth = PlayerPrefs.GetInt("MaxHealth", 1);
        ArticyGlobalVariables.Default.PlayerStats.Resolve = PlayerPrefs.GetInt("Resolve", 1);
        ArticyGlobalVariables.Default.PlayerStats.MaxResolve = PlayerPrefs.GetInt("MaxResolve", 1);
        ArticyGlobalVariables.Default.PlayerStats.Money = PlayerPrefs.GetInt("Money", 0);

        // Skill Base Scores
        ArticyGlobalVariables.Default.PlayerStats.ReptilianBaseScore = PlayerPrefs.GetInt("ReptilianBaseScore", 1);
        ArticyGlobalVariables.Default.PlayerStats.PaleoBaseScore = PlayerPrefs.GetInt("PaleoBaseScore", 1);
        ArticyGlobalVariables.Default.PlayerStats.NeoBaseScore = PlayerPrefs.GetInt("NeoBaseScore", 1);
        ArticyGlobalVariables.Default.PlayerStats.PaleBaseScore = PlayerPrefs.GetInt("PaleBaseScore", 1);

        // Player Variables (bools)
        ArticyGlobalVariables.Default.PlayerVariables.FoundGasMask = PlayerPrefs.GetInt("FoundGasMask", 0) == 1;
        ArticyGlobalVariables.Default.PlayerVariables.HasEquipment = PlayerPrefs.GetInt("HasEquipment", 0) == 1;
        ArticyGlobalVariables.Default.PlayerVariables.IdentityCrisis = PlayerPrefs.GetInt("IdentityCrisis", 0) == 1;
        ArticyGlobalVariables.Default.PlayerVariables.SucumbingToPale = PlayerPrefs.GetInt("SucumbingToPale", 0) == 1;
        ArticyGlobalVariables.Default.PlayerVariables.PhysicalDeath = PlayerPrefs.GetInt("PhysicalDeath", 0) == 1;
        ArticyGlobalVariables.Default.PlayerVariables.ResolveDeath = PlayerPrefs.GetInt("ResolveDeath", 0) == 1;
        ArticyGlobalVariables.Default.PlayerVariables.JesusComplex = PlayerPrefs.GetInt("JesusComplex", 1);
        ArticyGlobalVariables.Default.PlayerVariables.KnowsTheConcord = PlayerPrefs.GetInt("KnowsTheConcord", 0) == 1;
        ArticyGlobalVariables.Default.PlayerVariables.KnowWhoYouAre = PlayerPrefs.GetInt("KnowWhoYouAre", 0) == 1;
        ArticyGlobalVariables.Default.PlayerVariables.MistakenIdentity = PlayerPrefs.GetInt("MistakenIdentity", 1);
        ArticyGlobalVariables.Default.PlayerVariables.AlcoholConsumed = PlayerPrefs.GetInt("AlcoholConsumed", 0);
        ArticyGlobalVariables.Default.PlayerVariables.KnowsAboutJesus = PlayerPrefs.GetInt("KnowsAboutJesus", 0) == 1;

        //Equipped Items
        ArticyGlobalVariables.Default.EquippedItems.EquippedHead = PlayerPrefs.GetString("EquippedHead", "");
        ArticyGlobalVariables.Default.EquippedItems.EquippedFace = PlayerPrefs.GetString("EquippedFace", "");
        ArticyGlobalVariables.Default.EquippedItems.EquippedNeck = PlayerPrefs.GetString("EquippedNeck", "");
        ArticyGlobalVariables.Default.EquippedItems.EquippedBody = PlayerPrefs.GetString("EquippedBody", "");
        ArticyGlobalVariables.Default.EquippedItems.EquippedLegs = PlayerPrefs.GetString("EquippedLegs", "");
        ArticyGlobalVariables.Default.EquippedItems.EquippedFeet = PlayerPrefs.GetString("EquippedFeet", "");
        ArticyGlobalVariables.Default.EquippedItems.EquippedHands = PlayerPrefs.GetString("EquippedHands", "");
        ArticyGlobalVariables.Default.EquippedItems.EquippedTool = PlayerPrefs.GetString("EquippedTool", "");

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
        ArticyGlobalVariables.Default.GlobalVariables.IngoPaleRealization = PlayerPrefs.GetInt("IngoPaleRealization", 0) == 1;
        ArticyGlobalVariables.Default.GlobalVariables.KeptHorse = PlayerPrefs.GetInt("KeptHorse", 0) == 1;
        ArticyGlobalVariables.Default.GlobalVariables.ZuretonInnFirstTime = PlayerPrefs.GetInt("ZuretonInnFirstTime", 0) == 1;
        ArticyGlobalVariables.Default.GlobalVariables.InDialogue = PlayerPrefs.GetInt("InDialogue", 0) == 1;
        ArticyGlobalVariables.Default.GlobalVariables.CurrentDialogueTechnicalName = PlayerPrefs.GetString("CurrentDialogue", "");

        //Raik Variables
        ArticyGlobalVariables.Default.RaikVariables.RaikOpinion = PlayerPrefs.GetInt("RaikOpinion", 0);
        ArticyGlobalVariables.Default.RaikVariables.AskedAboutAnger = PlayerPrefs.GetInt("AskedAboutAnger", 0) == 1;
        ArticyGlobalVariables.Default.RaikVariables.JobDislikeRevealed = PlayerPrefs.GetInt("JobDislikeRevealed", 0) == 1;
        ArticyGlobalVariables.Default.RaikVariables.OpeningQuestionsAsked = PlayerPrefs.GetInt("OpeningQuestionsAsked", 0);
        ArticyGlobalVariables.Default.RaikVariables.NameKnown = PlayerPrefs.GetInt("NameKnown", 0) == 1;
        ArticyGlobalVariables.Default.RaikVariables.FinishedAlcohol = PlayerPrefs.GetInt("FinishedAlcohol", 0) == 1;
        ArticyGlobalVariables.Default.RaikVariables.TabPaid = PlayerPrefs.GetInt("TabPaid", 0) == 1;
        ArticyGlobalVariables.Default.RaikVariables.RoomDiscount = PlayerPrefs.GetInt("RoomDiscount", 0);
        ArticyGlobalVariables.Default.RaikVariables.KnowsFatherSexuality = PlayerPrefs.GetInt("KnowsFatherSexuality", 0) == 1;
        ArticyGlobalVariables.Default.RaikVariables.HatesFather = PlayerPrefs.GetInt("HatesFather", 0) == 1;
        ArticyGlobalVariables.Default.RaikVariables.KnowsSchwarstein = PlayerPrefs.GetInt("KnowsSchwarstein", 0) == 1;

        //Alina Variables
        ArticyGlobalVariables.Default.AlinaVariables.RaikAunt = PlayerPrefs.GetInt("RaikAunt", 0) == 1;
        ArticyGlobalVariables.Default.AlinaVariables.LavenderKnown = PlayerPrefs.GetInt("LavenderKnown", 0) == 1;
        ArticyGlobalVariables.Default.AlinaVariables.Known = PlayerPrefs.GetInt("Known", 0) == 1;

        //Zureton Variables
        ArticyGlobalVariables.Default.ZuretonVariables.MineKnown = PlayerPrefs.GetInt("MineKnown", 0) == 1;
        ArticyGlobalVariables.Default.ZuretonVariables.PaleDanger = PlayerPrefs.GetInt("PaleDanger", 0);

        //Quests
        ArticyGlobalVariables.Default.Quests.LeaveThePale = PlayerPrefs.GetInt("LeaveThePale", 0);
        ArticyGlobalVariables.Default.Quests.PayInnTab = PlayerPrefs.GetInt("PayInnTab", 0);
        ArticyGlobalVariables.Default.Quests.GetNewOutfit = PlayerPrefs.GetInt("GetNewOutfit", 0);
        ArticyGlobalVariables.Default.Quests.FindBeerNewHome = PlayerPrefs.GetInt("FindBeerNewHome", 0);
        ArticyGlobalVariables.Default.Quests.UncoverFatherRelationship = PlayerPrefs.GetInt("UncoverFatherRelationship", 0);
        ArticyGlobalVariables.Default.Quests.LearnAboutJesus = PlayerPrefs.GetInt("LearnAboutJesus", 0);

        // Time
        ArticyGlobalVariables.Default.GlobalVariables.Time = PlayerPrefs.GetInt("Time", 8 * 60);
        // Load any remaining Articy variables saved via reflective saver
        LoadArticyVariables();
        playerStats.UpdatePlayerStats();
        dialogueManager.LoadDialogue();  
        TimeManager.LoadTime();      
        Debug.Log("Game Loaded!");
    }

    // Reflectively save primitive fields (int, bool, string) from ArticyGlobalVariables.Default
    private void SaveArticyVariables()
    {
        var root = ArticyGlobalVariables.Default;
        if (root == null) return;
        SaveFieldsRecursive(root, "Articy");
    }

    private void SaveFieldsRecursive(object obj, string prefix)
    {
        if (obj == null) return;
        var type = obj.GetType();
        var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
        foreach (var f in fields)
        {
            var val = f.GetValue(obj);
            var key = prefix + "." + f.Name;
            if (val == null) continue;
            var fType = f.FieldType;
            if (fType == typeof(int))
                PlayerPrefs.SetInt(key, (int)val);
            else if (fType == typeof(bool))
                PlayerPrefs.SetInt(key, ((bool)val) ? 1 : 0);
            else if (fType == typeof(string))
                PlayerPrefs.SetString(key, (string)val);
            else if (!fType.IsPrimitive && !fType.IsEnum && !fType.IsValueType)
                SaveFieldsRecursive(val, key);
        }
    }

    // Reflectively load primitive fields (int, bool, string) into ArticyGlobalVariables.Default
    private void LoadArticyVariables()
    {
        var root = ArticyGlobalVariables.Default;
        if (root == null) return;
        LoadFieldsRecursive(root, "Articy");
    }

    private void LoadFieldsRecursive(object obj, string prefix)
    {
        if (obj == null) return;
        var type = obj.GetType();
        var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
        foreach (var f in fields)
        {
            var key = prefix + "." + f.Name;
            var fType = f.FieldType;
            var currentVal = f.GetValue(obj);
            if (fType == typeof(int))
            {
                int def = currentVal is int ci ? ci : 0;
                f.SetValue(obj, PlayerPrefs.GetInt(key, def));
            }
            else if (fType == typeof(bool))
            {
                int def = (currentVal is bool cb && cb) ? 1 : 0;
                f.SetValue(obj, PlayerPrefs.GetInt(key, def) == 1);
            }
            else if (fType == typeof(string))
            {
                string def = currentVal as string ?? string.Empty;
                f.SetValue(obj, PlayerPrefs.GetString(key, def));
            }
            else if (!fType.IsPrimitive && !fType.IsEnum && !fType.IsValueType)
            {
                var child = f.GetValue(obj);
                if (child != null)
                    LoadFieldsRecursive(child, key);
            }
        }
    }

    private IEnumerator LoadSceneAndTeleport(string targetScene, Vector3 targetPosition)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(targetScene);

        while (!asyncLoad.isDone)
            yield return null;

        // Wait one frame to ensure everything is initialized
        yield return null;

        // Teleport player
        TeleportToLoadLocation(targetPosition);

        // Continue loading other data if needed
        Debug.Log("Game Loaded and Teleported!");
    }

    public void TeleportToLoadLocation(Vector3 pos)
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            // Temporarily disable the CharacterController
            var controller = playerObj.GetComponent<CharacterController>();
            if (controller != null)
            {
                controller.enabled = false;
                playerObj.transform.position = pos;
                controller.enabled = true;
                Debug.Log($"Teleported to position: {pos} using CharacterController method");
            }
            else
            {
                // Fallback to direct transform if no CharacterController is found
                playerObj.transform.position = pos;
                Debug.Log($"Teleported to position: {pos} using direct transform");
            }
        }
        ArticyGlobalVariables.Default.GlobalVariables.LoadingGame = false;
    }
    
    public void ResetGame()
    {
        PlayerPrefs.DeleteAll();
        inventoryManager.ClearInventory();
        Debug.Log("Game Reset!");
    }
}
