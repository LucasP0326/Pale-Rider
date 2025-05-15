using Articy.Unity;
using Articy.Unity.Interfaces;
using UnityEngine;
using UnityEngine.UI;
using Articy.Pale_Rider;
using StarterAssets;
using TMPro;

public class SkillSelectInterface : MonoBehaviour
{
    // Important References
    private GameObject playerController;
    private ThirdPersonController controller;
    private PlayerStats playerStats;

    // Game State
    private bool firstTime = true;
    private bool signatureSkillSelected = false;
    private bool initialSkillsAssigned = false;

    public GameObject selectedSkill;

    // Values
    [Header("Skill Points")]
    public int startingAvailableSkillPoints = 8;
    public int availableSkillPoints;

    [Header("UI Elements")]
    public GameObject rowIncreasePanel;
    public GameObject startingPointsPanel;
    public GameObject selectedSkillPortrait;
    public TextMeshProUGUI availableSkillPointsText;
    public TextMeshProUGUI startingAvailableSkillPointsText;
    public TextMeshProUGUI reptilianScore;
    public TextMeshProUGUI paleoScore;
    public TextMeshProUGUI neoScore;
    public TextMeshProUGUI paleScore;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player");
        controller = playerController.GetComponent<ThirdPersonController>();
        playerStats = playerController.GetComponent<PlayerStats>();
        //rowIncreasePanel.SetActive(firstTime);
        //startingPointsPanel.SetActive(firstTime);
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

        rowIncreasePanel.SetActive(firstTime);
        startingPointsPanel.SetActive(firstTime);

        if (startingAvailableSkillPoints == 0)
        {
            initialSkillsAssigned = true;
        }
        else if (startingAvailableSkillPoints > 0)
        {
            initialSkillsAssigned = false;
        }

        //Display Values
        startingAvailableSkillPointsText.text = startingAvailableSkillPoints + " Available Points";
        reptilianScore.text = playerStats.reptilianBaseScore.ToString();
        paleoScore.text = playerStats.paleoBaseScore.ToString();
        neoScore.text = playerStats.neoBaseScore.ToString();
        paleScore.text = playerStats.paleBaseScore.ToString();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Close();
        }
    }

    public void Close()
    {
        if (initialSkillsAssigned)
        {
            firstTime = false;
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
    }

    //Skill Row Assignments
    public void IncreaseReptilian()
    {
        if (startingAvailableSkillPoints > 0 && playerStats.reptilianBaseScore < 6)
        {
            playerStats.reptilianBaseScore++;
            startingAvailableSkillPoints--;
        }
    }
    public void IncreasePaleo()
    {
        if (startingAvailableSkillPoints > 0 && playerStats.paleoBaseScore < 6)
        {
            playerStats.paleoBaseScore++;
            startingAvailableSkillPoints--;
        }
    }
    public void IncreaseNeo()
    {
        if (startingAvailableSkillPoints > 0 && playerStats.neoBaseScore < 6)
        {
            playerStats.neoBaseScore++;
            startingAvailableSkillPoints--;
        }
    }
    public void IncreasePale()
    {
        if (startingAvailableSkillPoints > 0 && playerStats.paleBaseScore < 6)
        {
            playerStats.paleBaseScore++;
            startingAvailableSkillPoints--;
        }
    }

    public void DecreaseReptilian()
    {
        if (playerStats.reptilianBaseScore > 1)
        {
            playerStats.reptilianBaseScore--;
            startingAvailableSkillPoints++;
        }
    }

    public void DecreasePaleo()
    {
        if (playerStats.paleoBaseScore > 1)
        {
            playerStats.paleoBaseScore--;
            startingAvailableSkillPoints++;
        }
    }

    public void DecreaseNeo()
    {
        if (playerStats.neoBaseScore > 1)
        {
            playerStats.neoBaseScore--;
            startingAvailableSkillPoints++;
        }
    }

    public void DecreasePale()
    {
        if (playerStats.paleBaseScore > 1)
        {
            playerStats.paleBaseScore--;
            startingAvailableSkillPoints++;
        }
    }
}
