using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using Articy.Unity; // Import Articy namespace
using Articy.Unity.Interfaces;
using Articy.Pale_Rider;
using Articy.Pale_Rider.GlobalVariables;

public class SkillPortraitInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //Important References
    public GameObject skillSelectInterface;
    private GameObject playerController;
    private PlayerStats playerStats;

    // UI
    public GameObject starLayout;
    public Sprite fullPortrait;

    [Header("Skill Info")]
    public string skillCategory;
    public string skillName;
    public string skillInfo;
    public string skillStats1;
    public string skillStats2;
    public string skillStats3;
    public string skillDescription;
    public int categoryLevel;
    public int skillLevel;

    void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player");
        playerStats = playerController.GetComponent<PlayerStats>();
        starLayout.SetActive(false);
        skillName = gameObject.name;
    }

    void Update()
    {
        if (skillCategory == "Reptilian Complex")
        {
            categoryLevel = playerStats.reptilianBaseScore;
            if (skillName == "Endurance")
            {
                skillLevel = playerStats.endurance;
            }
            else if (skillName == "Physicality")
            {
                skillLevel = playerStats.physicality;
            }
            else if (skillName == "Reflexivity")
            {
                skillLevel = playerStats.reflexivity;
            }
            else if (skillName == "Volition")
            {
                skillLevel = playerStats.volition;
            }
        }
        else if (skillCategory == "PaleoMammalian Complex")
        {
            categoryLevel = playerStats.paleoBaseScore;
            if (skillName == "Empathy")
            {
                skillLevel = playerStats.empathy;
            }
            else if (skillName == "Suggestion")
            {
                skillLevel = playerStats.suggestion;
            }
            else if (skillName == "Authority")
            {
                skillLevel = playerStats.authority;
            }
            else if (skillName == "Rhetoric")
            {
                skillLevel = playerStats.rhetoric;
            }
        }
        else if (skillCategory == "NeoMammalian Complex")
        {
            categoryLevel = playerStats.neoBaseScore;
            if (skillName == "Encyclopedia")
            {
                skillLevel = playerStats.encyclopedia;
            }
            else if (skillName == "Logic")
            {
                skillLevel = playerStats.logic;
            }
            else if (skillName == "Perception")
            {
                skillLevel = playerStats.perception;
            }
            else if (skillName == "Conceptualization")
            {
                skillLevel = playerStats.conceptualization;
            }
        }
        else if (skillCategory == "The Pale")
        {
            categoryLevel = playerStats.paleBaseScore;
            if (skillName == "Savoir-Faire")
            {
                skillLevel = playerStats.savoirFaire;
            }
            else if (skillName == "Perspicacity")
            {
                skillLevel = playerStats.perspicacity;
            }
            else if (skillName == "Tenebrality")
            {
                skillLevel = playerStats.tenebrality;
            }
            else if (skillName == "SelfActualization")
            {
                skillLevel = playerStats.selfActualization;
            }
        }

        //Update star layout with appropriate number of stars
        for (int i = 0; i < starLayout.transform.childCount; i++)
        {
            var star = starLayout.transform.GetChild(i).gameObject;
            star.SetActive(i < categoryLevel);

            // Enable fill child if within skillLevel
            if (star.transform.childCount > 0)
            {
            var fill = star.transform.GetChild(0).gameObject;
            fill.SetActive(i < skillLevel);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        starLayout.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        starLayout.SetActive(false);
    }
}
