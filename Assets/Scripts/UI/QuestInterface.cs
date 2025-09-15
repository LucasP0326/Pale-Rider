using UnityEngine;

public class QuestInterface : MonoBehaviour
{
    public GameObject activeQuestsPanel;
    public GameObject completedQuestsPanel;
    public int questPanelViewState; // 0 = None, 1 = Active, 2 = Completed
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!activeQuestsPanel.activeSelf && !completedQuestsPanel.activeSelf)
        {
            questPanelViewState = 0;
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
}
