using Articy.Unity;
using Articy.Unity.Interfaces;
using UnityEngine;
using UnityEngine.UI;
using Articy.Pale_Rider;
using Articy.Pale_Rider.GlobalVariables;
using StarterAssets;
using TMPro;

public class QuestInterface : MonoBehaviour
{
    public GameObject selectedQuest;

    [Header("Panels")]
    public GameObject activeQuestsPanel;
    public GameObject activeQuestContent;
    public GameObject completedQuestsPanel;
    public GameObject completedQuestContent;
    public int questPanelViewState; // 0 = None, 1 = Active, 2 = Completed

    [Header("Prefabs")]
    public GameObject questLabelPrefab;

    [Header("References")]
    private GameObject playerController;
    private ThirdPersonController controller;
    private QuestManager questManager;

    [Header("UI Elements")]
    public TextMeshProUGUI questNameText;
    public TextMeshProUGUI questDescriptionText;
    public GameObject questStageContent;
    public GameObject questStagePrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player");
        controller = playerController.GetComponent<ThirdPersonController>();
        questManager = playerController.GetComponent<QuestManager>();
        questPanelViewState = 0;
        UpdateQuests();
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
        
        if (!activeQuestsPanel.activeSelf && !completedQuestsPanel.activeSelf)
        {
            questPanelViewState = 0;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Close();
        }
    }

    public void OpenActiveQuests()
    {
        Debug.Log("Open Active Quests");
        if (questPanelViewState == 0)
        {
            activeQuestsPanel.SetActive(true);
            questPanelViewState = 1;
        }
        else if (questPanelViewState == 1)
        {
            activeQuestsPanel.SetActive(false);
            questPanelViewState = 0;
        }
        else if (questPanelViewState == 2)
        {
            completedQuestsPanel.SetActive(false);
            activeQuestsPanel.SetActive(true);
            questPanelViewState = 1;
        }
    }

    public void OpenCompletedQuests()
    {
        Debug.Log("Open Completed Quests");
        if (questPanelViewState == 0)
        {
            completedQuestsPanel.SetActive(true);
            questPanelViewState = 2;
        }
        else if (questPanelViewState == 2)
        {
            completedQuestsPanel.SetActive(false);
            questPanelViewState = 0;
        }
        else if (questPanelViewState == 1)
        {
            activeQuestsPanel.SetActive(false);
            completedQuestsPanel.SetActive(true);
            questPanelViewState = 2;
        }
    }

    public void UpdateQuests()
    {
        // Clear all quest grids
        foreach (Transform child in activeQuestContent.transform)
            Destroy(child.gameObject);
        foreach (Transform child in completedQuestContent.transform)
            Destroy(child.gameObject);

        // Repopulate grids
        foreach (var quest in questManager.activeQuests)
        {
            if (quest == null) continue;

            // Instantiate the UI element for the quest (assuming questLabelPrefab is a UI prefab)
            Quest questUI = Instantiate(questLabelPrefab).GetComponent<Quest>();
            questUI.GetComponentInChildren<TextMeshProUGUI>().text = quest.questName;
            questUI.questInterface = this;
            questUI.technicalName = quest.technicalName;
            questUI.questName = quest.questName;
            questUI.questDescription = quest.questDescription;
            questUI.questStages = quest.questStages;
            questUI.questStage = quest.questStage;
            questUI.questExperienceReward = quest.questExperienceReward;
            questUI.questExperienceRewardInt = quest.questExperienceRewardInt;
            questUI.isComplete = quest.isComplete;

            // Set parent to active quests panel
            questUI.transform.SetParent(activeQuestContent.transform, false);
        }
        foreach (var quest in questManager.completedQuests)
        {
            if (quest == null) continue;

            // Instantiate the UI element for the quest (assuming questLabelPrefab is a UI prefab)
            Quest questUI = Instantiate(questLabelPrefab).GetComponent<Quest>();
            questUI.GetComponentInChildren<TextMeshProUGUI>().text = quest.questName;
            questUI.questInterface = this;
            questUI.technicalName = quest.technicalName;
            questUI.questName = quest.questName;
            questUI.questDescription = quest.questDescription;
            questUI.questStages = quest.questStages;
            questUI.questStage = quest.questStage;
            questUI.questExperienceReward = quest.questExperienceReward;
            questUI.questExperienceRewardInt = quest.questExperienceRewardInt;
            questUI.isComplete = quest.isComplete;

            // Set parent to completed quests panel
            questUI.transform.SetParent(completedQuestContent.transform, false);
        }
    }

    public void SelectQuest()
    {
        Debug.Log("Quest Selected");
        if (selectedQuest != null)
        {
            Quest quest = selectedQuest.GetComponent<Quest>();
            if (quest != null)
            {
                questNameText.text = quest.questName;
                questDescriptionText.text = quest.questDescription;

                // Clear existing stages
                foreach (Transform child in questStageContent.transform)
                {
                    Destroy(child.gameObject);
                }

                // Create the orange color (FF7400)
                Color orangeColor;
                ColorUtility.TryParseHtmlString("#FF7400", out orangeColor);

                // Populate stages as children of questStageContent
                string[] stages = quest.questStages.Split(new string[] { "\n", "\r\n" }, System.StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < stages.Length; i++)
                {
                    GameObject stageObj = Instantiate(questStagePrefab, questStageContent.transform);
                    stageObj.name = "Stage_" + (i + 1);
                    TextMeshProUGUI stageText = stageObj.GetComponent<TextMeshProUGUI>();
                    if (stageText != null)
                    {
                        // Remove the stage number from the beginning of the text
                        string cleanText = stages[i];
                        int dotIndex = cleanText.IndexOf('.');
                        if (dotIndex != -1 && dotIndex < 5) // Assuming stage numbers are 1-3 digits
                        {
                            cleanText = cleanText.Substring(dotIndex + 1).Trim();
                        }
                        
                        stageText.text = cleanText;
                        if (i + 1 <= quest.questStage) // Current and previous stages
                        {
                            stageText.color = orangeColor;
                            stageText.gameObject.SetActive(true);
                        }
                        else // Future stages
                        {
                            stageText.gameObject.SetActive(false);
                        }
                    }
                }
            }
        }
    }

    public void Close()
    {
        // Close the Quest interface
        gameObject.SetActive(false);

        UpdateQuests();

        // Resume player control
        if (playerController != null && controller != null)
        {
            controller.inMenu = false;
            controller.paused = false;
        }
    }
}
