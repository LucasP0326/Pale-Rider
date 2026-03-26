using Articy.Unity;
using Articy.Unity.Interfaces;
using UnityEngine;
using UnityEngine.UI;
using Articy.Pale_Rider;
using Articy.Pale_Rider.GlobalVariables;
using StarterAssets;
using TMPro;

public class SkillSelectInterface : MonoBehaviour
{
    // Important References
    private GameObject playerController;
    private ThirdPersonController controller;
    private PlayerStats playerStats;

    // Game State
    public bool firstTime = true;
    public bool signatureSkillSelected = false;
    private bool initialSkillsAssigned = false;

    // Colors
    private Color purple = new Color(0.5f, 0f, 0.5f);

    public GameObject selectedSkill;

    // Values
    [Header("Skill Points")]
    public int startingAvailableSkillPoints = 8;
    public int availableSkillPoints = 0;
    public int availableAttributePoints = 0;

    [Header("UI Elements")]
    public GameObject rowIncreasePanel;
    public GameObject hintPanel;
    public GameObject startingPointsPanel;
    public GameObject availableSkillPointsPanel;
    //Portrait
    public GameObject selectedSkillPanel;
    public GameObject selectedSkillPortrait;
    public TextMeshProUGUI selectedSkillName;
    public TextMeshProUGUI selectedSkillInfo;
    public TextMeshProUGUI selectedSkillStats1;
    public TextMeshProUGUI selectedSkillStats2;
    public TextMeshProUGUI selectedSkillStats3;
    public TextMeshProUGUI selectedSkillDescription;

    //Experience and Point Tracker
    public GameObject experiencePanel;
    public TextMeshProUGUI experienceText;
    public GameObject skillPointPanel;
    public GameObject whiteStar;

    //Skill Info
    public GameObject skillInfoPanel;
    public GameObject skillDescriptionPanel;
    public GameObject setSignaturePanel;
    public GameObject levelUpPanel;

    [Header("Skill Row Assignments")]
    //Points
    public TextMeshProUGUI availableSkillPointsText;
    public TextMeshProUGUI startingAvailableSkillPointsText;
    public TextMeshProUGUI reptilianScore;
    public TextMeshProUGUI paleoScore;
    public TextMeshProUGUI neoScore;
    public TextMeshProUGUI paleScore;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        firstTime = !ArticyGlobalVariables.Default.GlobalVariables.AssignedSkills;

        playerController = GameObject.FindGameObjectWithTag("Player");
        controller = playerController.GetComponent<ThirdPersonController>();
        playerStats = playerController.GetComponent<PlayerStats>();
        rowIncreasePanel.SetActive(firstTime);
        startingPointsPanel.SetActive(firstTime);
        hintPanel.SetActive(firstTime);
        selectedSkillPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (ArticyGlobalVariables.Default.PlayerStats.SignatureSkill != "")
        {
            signatureSkillSelected = true;
        }
        else
        {
            signatureSkillSelected = false;
        }

        availableSkillPoints = ArticyGlobalVariables.Default.PlayerStats.AvailableSkillPoints;
        
        if (playerController != null)
        {
            if (controller != null)
            {
                controller.inMenu = gameObject.activeSelf;
                controller.paused = gameObject.activeSelf;
            }
        }

        if (startingAvailableSkillPoints == 0 && signatureSkillSelected)
        {
            initialSkillsAssigned = true;
        }
        else if (startingAvailableSkillPoints > 0 && !signatureSkillSelected)
        {
            initialSkillsAssigned = false;
        }

        //Update UI
        rowIncreasePanel.SetActive(firstTime);
        startingPointsPanel.SetActive(firstTime);
        availableSkillPointsPanel.SetActive(!firstTime);
        experiencePanel.SetActive(!firstTime);
        if (firstTime)
        {
            if (signatureSkillSelected == false)
                setSignaturePanel.SetActive(true);
            else if (signatureSkillSelected == true)
                setSignaturePanel.SetActive(false);
        }
        else if (!firstTime)
        {
            setSignaturePanel.SetActive(false);
        }
        
        if (firstTime)
        {
            hintPanel.SetActive(!selectedSkillPanel.activeSelf);
        }
        else if (!firstTime)
        {
            hintPanel.SetActive(false);
        }

        if (firstTime)
            levelUpPanel.SetActive(false);
        else if (!firstTime && availableSkillPoints > 0)
            levelUpPanel.SetActive(true);
        else if (!firstTime && availableSkillPoints <= 0)
            levelUpPanel.SetActive(false);
        
        //Display Values
        startingAvailableSkillPointsText.text = startingAvailableSkillPoints + " Available Points";
        availableSkillPointsText.text = availableSkillPoints + " Available Points";
        reptilianScore.text = playerStats.reptilianBaseScore.ToString();
        paleoScore.text = playerStats.paleoBaseScore.ToString();
        neoScore.text = playerStats.neoBaseScore.ToString();
        paleScore.text = playerStats.paleBaseScore.ToString();
        experienceText.text = "Experience: " + playerStats.experience.ToString() + " / 100";

        if (gameObject.activeSelf)
        {
            UpdateAvailableSkillPoints();
        }

        //Manage Number of SKill Points Available
        if (!firstTime)
        {
            if (ArticyGlobalVariables.Default.PlayerStats.Experience >= 100)
            {
                ArticyGlobalVariables.Default.PlayerStats.AvailableSkillPoints++;
                ArticyGlobalVariables.Default.PlayerStats.Experience -= 100;
                playerStats.experience = ArticyGlobalVariables.Default.PlayerStats.Experience;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Close();
        }

        //Disable Level Up
        if (selectedSkill.GetComponent<SkillPortraitInfo>().skillLevel >= selectedSkill.GetComponent<SkillPortraitInfo>().categoryLevel)
        {
            levelUpPanel.SetActive(false);
        }
    }

    public void SelectSkill()
    {
        selectedSkillPanel.SetActive(true);
        selectedSkill = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        if (selectedSkill != null && selectedSkillPortrait != null)
        {
            Sprite skillImage = selectedSkill.GetComponent<SkillPortraitInfo>().fullPortrait;
            selectedSkillPortrait.GetComponent<Image>().sprite = skillImage;
        }
        if (selectedSkillName != null && selectedSkill != null)
        {
            selectedSkillName.text = selectedSkill.name;
        }

        selectedSkillInfo.text = selectedSkill.GetComponent<SkillPortraitInfo>().skillInfo;
        selectedSkillStats1.text = selectedSkill.GetComponent<SkillPortraitInfo>().skillStats1 + " " + selectedSkill.GetComponent<SkillPortraitInfo>().categoryLevel;
        selectedSkillStats2.text = selectedSkill.GetComponent<SkillPortraitInfo>().skillStats2 + " " + selectedSkill.GetComponent<SkillPortraitInfo>().skillLevel;
        selectedSkillStats3.text = selectedSkill.GetComponent<SkillPortraitInfo>().skillStats3 + " " + selectedSkill.GetComponent<SkillPortraitInfo>().itemLevel;
        selectedSkillDescription.text = selectedSkill.GetComponent<SkillPortraitInfo>().skillDescription;

        if (firstTime)
        {
            hintPanel.SetActive(false);
        }
    }

    public void Close()
    {
        if (initialSkillsAssigned || !firstTime)
        {
            firstTime = false;
            ArticyGlobalVariables.Default.GlobalVariables.AssignedSkills = true;
            gameObject.SetActive(false);
            if (controller != null)
            {
                controller.inMenu = false;
                controller.paused = false;
            }
        }
        else
        {
            Debug.Log("You must assign your skills before closing the menu.");
        }

        //Initialize Player Stats in Articy
        playerStats.currentHealth = playerStats.maxHealth;
        playerStats.currentResolve = playerStats.maxResolve;
        ArticyGlobalVariables.Default.PlayerStats.Health = playerStats.maxHealth;
        ArticyGlobalVariables.Default.PlayerStats.Resolve = playerStats.maxResolve;
        //ArticyGlobalVariables.Default.PlayerStats.ReptilianBaseScore++;
        //ArticyGlobalVariables.Default.PlayerStats.PaleoBaseScore++;
        //ArticyGlobalVariables.Default.PlayerStats.NeoBaseScore++;
        //ArticyGlobalVariables.Default.PlayerStats.PaleBaseScore++;
    }

    public void ShowSkillInfo()
    {
        if (selectedSkill != null)
        {
            skillInfoPanel.SetActive(true);
            skillDescriptionPanel.SetActive(false);
        }
    }

    public void ShowSkillDescription()
    {
        if (selectedSkill != null)
        {
            skillInfoPanel.SetActive(false);
            skillDescriptionPanel.SetActive(true);
        }
    }

    //Skill Row Assignments

    public void AssignSignatureSkill()
    {
        ArticyGlobalVariables.Default.PlayerStats.SignatureSkill = selectedSkill.GetComponent<SkillPortraitInfo>().skillName;
        playerStats.signatureSkill = ArticyGlobalVariables.Default.PlayerStats.SignatureSkill;
        selectedSkill.GetComponent<SkillPortraitInfo>().LevelUpSkill();
        signatureSkillSelected = true;
    }

    public void LevelUpSkill()
    {
        ArticyGlobalVariables.Default.PlayerStats.AvailableSkillPoints--;
        availableSkillPoints--;
        selectedSkill.GetComponent<SkillPortraitInfo>().LevelUpSkill();
    }

    public void IncreaseReptilian()
    {
        if (startingAvailableSkillPoints > 0 && playerStats.reptilianBaseScore < 6)
        {
            playerStats.reptilianBaseScore++;
            ArticyGlobalVariables.Default.PlayerStats.MaxHealth++;
            ArticyGlobalVariables.Default.PlayerStats.ReptilianBaseScore++;
            playerStats.maxHealth++;
            playerStats.InitializeHealthBar();
            startingAvailableSkillPoints--;
        }
    }
    public void IncreasePaleo()
    {
        if (startingAvailableSkillPoints > 0 && playerStats.paleoBaseScore < 6)
        {
            playerStats.paleoBaseScore++;
            ArticyGlobalVariables.Default.PlayerStats.PaleoBaseScore++;
            startingAvailableSkillPoints--;
        }
    }
    public void IncreaseNeo()
    {
        if (startingAvailableSkillPoints > 0 && playerStats.neoBaseScore < 6)
        {
            playerStats.neoBaseScore++;
            ArticyGlobalVariables.Default.PlayerStats.NeoBaseScore++;
            startingAvailableSkillPoints--;
        }
    }
    public void IncreasePale()
    {
        if (startingAvailableSkillPoints > 0 && playerStats.paleBaseScore < 6)
        {
            playerStats.paleBaseScore++;
            ArticyGlobalVariables.Default.PlayerStats.MaxResolve++;
            ArticyGlobalVariables.Default.PlayerStats.PaleBaseScore++;
            playerStats.maxResolve++;
            playerStats.InitializeResolveBar();
            startingAvailableSkillPoints--;
        }
    }

    public void DecreaseReptilian()
    {
        if (playerStats.reptilianBaseScore > 1)
        {
            playerStats.reptilianBaseScore--;
            ArticyGlobalVariables.Default.PlayerStats.MaxHealth--;
            ArticyGlobalVariables.Default.PlayerStats.ReptilianBaseScore--;
            playerStats.maxHealth--;
            playerStats.InitializeHealthBar();
            startingAvailableSkillPoints++;
        }
    }

    public void DecreasePaleo()
    {
        if (playerStats.paleoBaseScore > 1)
        {
            playerStats.paleoBaseScore--;
            ArticyGlobalVariables.Default.PlayerStats.PaleoBaseScore--;
            startingAvailableSkillPoints++;
        }
    }

    public void DecreaseNeo()
    {
        if (playerStats.neoBaseScore > 1)
        {
            playerStats.neoBaseScore--;
            ArticyGlobalVariables.Default.PlayerStats.NeoBaseScore--;
            startingAvailableSkillPoints++;
        }
    }

    public void DecreasePale()
    {
        if (playerStats.paleBaseScore > 1)
        {
            playerStats.paleBaseScore--;
            ArticyGlobalVariables.Default.PlayerStats.MaxResolve--;
            ArticyGlobalVariables.Default.PlayerStats.PaleBaseScore--;
            playerStats.maxResolve--;
            playerStats.InitializeResolveBar();
            startingAvailableSkillPoints++;
        }
    }

    public void UpdateAvailableSkillPoints()
    {
        if (skillPointPanel == null || whiteStar == null)
            return;

        var parent = skillPointPanel.transform;
        int current = parent.childCount;
        int target = availableSkillPoints;

        // If there are too many, remove the extras (from end to keep ordering)
        if (current > target)
        {
            for (int i = current - 1; i >= target; i--)
            {
                var child = parent.GetChild(i);
#if UNITY_EDITOR
                // In editor use DestroyImmediate to remove immediately (keeps UI in sync)
                if (!Application.isPlaying) UnityEngine.Object.DestroyImmediate(child.gameObject);
                else UnityEngine.Object.Destroy(child.gameObject);
#else
                UnityEngine.Object.Destroy(child.gameObject);
#endif
            }
        }
        // If there are too few, instantiate the difference
        else if (current < target)
        {
            for (int i = current; i < target; i++)
            {
                var star = UnityEngine.Object.Instantiate(whiteStar, parent);
                // Normalize transform for UI prefabs
                var rt = star.GetComponent<RectTransform>();
                if (rt != null)
                {
                    rt.localScale = Vector3.one;
                    rt.anchoredPosition = Vector2.zero;
                }
                else
                {
                    star.transform.localScale = Vector3.one;
                    star.transform.localPosition = Vector3.zero;
                }
                star.name = "WhiteStar_" + i;
            }
        }
    }
}
