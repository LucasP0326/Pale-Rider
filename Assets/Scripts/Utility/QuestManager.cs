using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Articy.Unity;
using Articy.Unity.Interfaces;
using Articy.Pale_Rider;
using Articy.Pale_Rider.GlobalVariables;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;

public class QuestManager : MonoBehaviour
{
    public bool addingQuests;

    [Header("Quest List")]
    public Quest[] activeQuests;
    public Quest[] completedQuests;
    public Quest questPrefab;
    public TextMeshProUGUI questNameText;

    [Header("Quest Interface")]
    public GameObject questSpace;
    public GameObject activeQuestsPanel;
    public GameObject completedQuestsPanel;

    [Header("UI Elements")]
    public GameObject questPopup;
    public TMP_Text questAddedText;
    public TMP_Text questNamePopupText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(HideStartScenePopup());
    }

    // Update is called once per frame
    void Update()
    {
        QuestChecker();
        QuestUpdater();
    }

    public void QuestChecker()
    {
        if (ArticyGlobalVariables.Default.Quests.LeaveThePale != 0)
        {
            bool questExists = false;
            foreach (Transform child in questSpace.transform)
            {
                Quest quest = child.GetComponent<Quest>();
                if (quest != null && quest.technicalName == "Q_LeaveThePale")
                {
                    questExists = true;
                    break;
                }
            }

            if (!questExists)
            {
                AddQuest("Q_LeaveThePale");
            }
        }

        if (ArticyGlobalVariables.Default.Quests.PayInnTab != 0)
        {
            bool questExists = false;
            foreach (Transform child in questSpace.transform)
            {
                Quest quest = child.GetComponent<Quest>();
                if (quest != null && quest.technicalName == "Q_PayInnTab")
                {
                    questExists = true;
                    break;
                }
            }

            if (!questExists)
            {
                AddQuest("Q_PayInnTab");
            }
        }
    }

    public void QuestUpdater()
    {
        foreach (Transform child in questSpace.transform)
        {
            Quest quest = child.GetComponent<Quest>();
            if (quest != null && quest.technicalName == "Q_LeaveThePale")
            {
                quest.questStage = ArticyGlobalVariables.Default.Quests.LeaveThePale;
                if (quest.questStage == 1000 && !quest.isComplete)
                {
                    // Move to completed quests
                    var activeList = new List<Quest>(activeQuests ?? new Quest[0]);
                    activeList.Remove(quest);
                    activeQuests = activeList.ToArray();

                    var completedList = new List<Quest>(completedQuests ?? new Quest[0]);
                    completedList.Add(quest);
                    completedQuests = completedList.ToArray();

                    ArticyGlobalVariables.Default.PlayerStats.Experience += quest.questExperienceRewardInt;
                    RemoveQuest("Leave The Pale");
                    quest.isComplete = true;
                }
            }

            if (quest != null && quest.technicalName == "Q_PayInnTab")
            {
                quest.questStage = ArticyGlobalVariables.Default.Quests.PayInnTab;
                if (quest.questStage == 1000 && !quest.isComplete)
                {
                    // Move to completed quests
                    var activeList = new List<Quest>(activeQuests ?? new Quest[0]);
                    activeList.Remove(quest);
                    activeQuests = activeList.ToArray();

                    var completedList = new List<Quest>(completedQuests ?? new Quest[0]);
                    completedList.Add(quest);
                    completedQuests = completedList.ToArray();

                    ArticyGlobalVariables.Default.PlayerStats.Experience += quest.questExperienceRewardInt;
                    RemoveQuest("Pay Inn Tab");
                    quest.isComplete = true;
                }
            }
        }
    }

    public void AddQuest(string technicalName)
    {
        var articyObj = ArticyDatabase.GetObject(technicalName) as Articy.Pale_Rider.Quests;
        if (articyObj == null)
        {
            Debug.LogWarning("Articy object not found for technical name: " + technicalName);
            return;
        }

        //Instantiate Prefab
        Quest newQuest = Instantiate(questPrefab, questSpace.transform);

        //Populate Fields
        newQuest.technicalName = technicalName;
        newQuest.questName = articyObj.DisplayName;
        newQuest.questDescription = articyObj.Template.Description.MediumTextValue;
        newQuest.questStages = articyObj.Template.QuestStages.LargeTextValue;
        newQuest.questExperienceReward = articyObj.Template.ExperienceReward.NumberValue;

        //Add New Quest to Active Quests
        var questsList = new List<Quest>(activeQuests ?? new Quest[0]);
        questsList.Add(newQuest);
        activeQuests = questsList.ToArray();

        //Popup
        questAddedText.text = "Quest Added";
        questNamePopupText.text = newQuest.questName;
        StartCoroutine(ShowQuestPopup());
    }

    public void RemoveQuest(string questName)
    {
        questAddedText.text = "Quest Completed";
        questNamePopupText.text = questName;
        StartCoroutine(ShowQuestPopup());
    }

    public void SaveQuests()
    {
        // Implement saving logic if needed
    }

    private IEnumerator ShowQuestPopup()
    {
        questPopup.SetActive(true);
        yield return new WaitForSeconds(3f);
        questPopup.SetActive(false);
    }

    private IEnumerator HideStartScenePopup()
    {
        yield return new WaitForSeconds(0.01f);
        questPopup.SetActive(false);
    }
}
