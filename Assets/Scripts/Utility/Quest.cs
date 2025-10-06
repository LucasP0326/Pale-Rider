using UnityEngine;
using UnityEngine.UI;
using Articy.Unity;
using Articy.Unity.Interfaces;
using Articy.Pale_Rider;
using Articy.Pale_Rider.GlobalVariables;

public class Quest : MonoBehaviour
{
    public QuestInterface questInterface;
    public string technicalName;
    public string questName;
    public string questDescription;
    public string questStages;
    public int questStage;
    public bool isComplete;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        questInterface = GameObject.FindFirstObjectByType<QuestInterface>();
    }

    // Update is called once per frame
    void Update()
    {
        if (questStage == 1000)
        {
            isComplete = true;
        }
    }

    public void OnQuestSelected()
    {
        // Handle quest selection logic here
        Debug.Log("Selected Quest: " + questName);
        questInterface.selectedQuest = this.gameObject;
        questInterface.SelectQuest();
    }
}
