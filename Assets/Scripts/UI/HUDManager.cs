using Articy.Unity;
using Articy.Unity.Interfaces;
using UnityEngine;
using UnityEngine.UI;
using Articy.Pale_Rider;
using Articy.Pale_Rider.GlobalVariables;

public class HUDManager : MonoBehaviour
{
    public GameObject InventoryPanel;
    public GameObject SkillPanel;
    public GameObject QuestPanel;
    public GameObject MapPanel;
    public GameObject OxygenPanel;

    public GameObject SkillMenu;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (ArticyGlobalVariables.Default.GlobalVariables.UnlockedInventory)
        {
            InventoryPanel.SetActive(true);
        }
        else
        {
            InventoryPanel.SetActive(false);
        }

        if (ArticyGlobalVariables.Default.GlobalVariables.UnlockedSkills)
        {
            SkillPanel.SetActive(true);
        }
        else
        {
            SkillPanel.SetActive(false);
        }

        if (ArticyGlobalVariables.Default.GlobalVariables.UnlockedQuests)
        {
            QuestPanel.SetActive(true);
        }
        else
        {
            QuestPanel.SetActive(false);
        }

        if (ArticyGlobalVariables.Default.GlobalVariables.UnlockedMap)
        {
            MapPanel.SetActive(true);
        }
        else
        {
            MapPanel.SetActive(false);
        }

        if (ArticyGlobalVariables.Default.GlobalVariables.UnlockedOxygen)
        {
            OxygenPanel.SetActive(true);
        }
        else
        {
            OxygenPanel.SetActive(false);
        }

        //Open skill menu once
        if (ArticyGlobalVariables.Default.GlobalVariables.AssigningSkills)
        {
            SkillMenu.SetActive(true);
            ArticyGlobalVariables.Default.GlobalVariables.AssigningSkills = false;
        }
    }
}
