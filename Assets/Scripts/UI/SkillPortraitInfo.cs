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
    private bool isSignature = false;

    //Important References
    public GameObject levelPanel;
    public TMP_Text skillLevelText;
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
    public int itemLevel;

    void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player");
        playerStats = playerController.GetComponent<PlayerStats>();
        starLayout.SetActive(false);
        skillName = gameObject.name;
    }

    void Update()
    {
        if (ArticyGlobalVariables.Default.PlayerStats.SignatureSkill == skillName)
        {
            isSignature = true;
        }
        else
        {
            isSignature = false;
        }

        // Set levelPanel color based on isSignature
        if (levelPanel != null)
        {
            var image = levelPanel.GetComponent<Image>();
            if (image != null)
            {
                image.color = isSignature ? new Color(0.6f, 0.2f, 0.8f, 1f) : Color.white; // Purple or white
            }
        }

        skillLevelText.text = skillLevel.ToString();

        if (skillCategory == "Reptilian Complex")
        {
            categoryLevel = playerStats.reptilianBaseScore;
            if (skillName == "Endurance")
            {
                skillLevel = playerStats.endurance;
                itemLevel = ArticyGlobalVariables.Default.ItemStatVariables.EnduranceItem;
            }
            else if (skillName == "Physicality")
            {
                skillLevel = playerStats.physicality;
                itemLevel = ArticyGlobalVariables.Default.ItemStatVariables.PhysicalityItem;

            }
            else if (skillName == "Reflexivity")
            {
                skillLevel = playerStats.reflexivity;
                itemLevel = ArticyGlobalVariables.Default.ItemStatVariables.ReflexivityItem;
            }
            else if (skillName == "Volition")
            {
                skillLevel = playerStats.volition;
                itemLevel = ArticyGlobalVariables.Default.ItemStatVariables.VolitionItem;
            }
        }
        else if (skillCategory == "PaleoMammalian Complex")
        {
            categoryLevel = playerStats.paleoBaseScore;
            if (skillName == "Empathy")
            {
                skillLevel = playerStats.empathy;
                itemLevel = ArticyGlobalVariables.Default.ItemStatVariables.EmpathyItem;
            }
            else if (skillName == "Suggestion")
            {
                skillLevel = playerStats.suggestion;
                itemLevel = ArticyGlobalVariables.Default.ItemStatVariables.SuggestionItem;
            }
            else if (skillName == "Authority")
            {
                skillLevel = playerStats.authority;
                itemLevel = ArticyGlobalVariables.Default.ItemStatVariables.AuthorityItem;
            }
            else if (skillName == "Rhetoric")
            {
                skillLevel = playerStats.rhetoric;
                itemLevel = ArticyGlobalVariables.Default.ItemStatVariables.RhetoricItem;
            }
        }
        else if (skillCategory == "NeoMammalian Complex")
        {
            categoryLevel = playerStats.neoBaseScore;
            if (skillName == "Encyclopedia")
            {
                skillLevel = playerStats.encyclopedia;
                itemLevel = ArticyGlobalVariables.Default.ItemStatVariables.EncyclopediaItem;
            }
            else if (skillName == "Logic")
            {
                skillLevel = playerStats.logic;
                itemLevel = ArticyGlobalVariables.Default.ItemStatVariables.LogicItem;
            }
            else if (skillName == "Perception")
            {
                skillLevel = playerStats.perception;
                itemLevel = ArticyGlobalVariables.Default.ItemStatVariables.PerceptionItem;
            }
            else if (skillName == "Conceptualization")
            {
                skillLevel = playerStats.conceptualization;
                itemLevel = ArticyGlobalVariables.Default.ItemStatVariables.ConceptualizationItem;
            }
        }
        else if (skillCategory == "The Pale")
        {
            categoryLevel = playerStats.paleBaseScore;
            if (skillName == "Savoir-Faire")
            {
                skillLevel = playerStats.savoirFaire;
                itemLevel = ArticyGlobalVariables.Default.ItemStatVariables.SavoirFaireItem;
            }
            else if (skillName == "Perspicacity")
            {
                skillLevel = playerStats.perspicacity;
                itemLevel = ArticyGlobalVariables.Default.ItemStatVariables.PerspicacityItem;
            }
            else if (skillName == "Tenebrality")
            {
                skillLevel = playerStats.tenebrality;
                itemLevel = ArticyGlobalVariables.Default.ItemStatVariables.TenebralityItem;
            }
            else if (skillName == "SelfActualization")
            {
                skillLevel = playerStats.selfActualization;
                itemLevel = ArticyGlobalVariables.Default.ItemStatVariables.SelfActualizationItem;
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
                fill.SetActive(i < (skillLevel - itemLevel));
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

    public void LevelUpSkill()
    {
        if (skillName == "Endurance")
        {
            if (isSignature)
            {
                ArticyGlobalVariables.Default.PlayerStats.Endurance += 2;
                playerStats.endurance += 2;
            }
            else if (!isSignature)
            {
                ArticyGlobalVariables.Default.PlayerStats.Endurance += 1;
                playerStats.endurance += 1;
            }
        }
        else if (skillName == "Physicality")
        {
            if (isSignature)
            {
                ArticyGlobalVariables.Default.PlayerStats.Physicality += 2;
                playerStats.physicality += 2;
            }
            else if (!isSignature)
            {
                ArticyGlobalVariables.Default.PlayerStats.Physicality += 1;
                playerStats.physicality += 1;
            }
        }
        else if (skillName == "Reflexivity")
        {
            if (isSignature)
            {
                ArticyGlobalVariables.Default.PlayerStats.Reflexivity += 2;
                playerStats.reflexivity += 2;
            }
            else if (!isSignature)
            {
                ArticyGlobalVariables.Default.PlayerStats.Reflexivity += 1;
                playerStats.reflexivity += 1;
            }
        }
        else if (skillName == "Volition")
        {
            if (isSignature)
            {
                ArticyGlobalVariables.Default.PlayerStats.Volition += 2;
                playerStats.volition += 2;
            }
            else if (!isSignature)
            {
                ArticyGlobalVariables.Default.PlayerStats.Volition += 1;
                playerStats.volition += 1;
            }
        }
        else if (skillName == "Empathy")
        {
            if (isSignature)
            {
                ArticyGlobalVariables.Default.PlayerStats.Empathy += 2;
                playerStats.empathy += 2;
            }
            else if (!isSignature)
            {
                ArticyGlobalVariables.Default.PlayerStats.Empathy += 1;
                playerStats.empathy += 1;
            }
        }
        else if (skillName == "Suggestion")
        {
            if (isSignature)
            {
                ArticyGlobalVariables.Default.PlayerStats.Suggestion += 2;
                playerStats.suggestion += 2;
            }
            else if (!isSignature)
            {
                ArticyGlobalVariables.Default.PlayerStats.Suggestion += 1;
                playerStats.suggestion += 1;
            }
        }
        else if (skillName == "Authority")
        {
            if (isSignature)
            {
                ArticyGlobalVariables.Default.PlayerStats.Authority += 2;
                playerStats.authority += 2;
            }
            else if (!isSignature)
            {
                ArticyGlobalVariables.Default.PlayerStats.Authority += 1;
                playerStats.authority += 1;
            }
        }
        else if (skillName == "Rhetoric")
        {
            if (isSignature)
            {
                ArticyGlobalVariables.Default.PlayerStats.Rhetoric += 2;
                playerStats.rhetoric += 2;
            }
            else if (!isSignature)
            {
                ArticyGlobalVariables.Default.PlayerStats.Rhetoric += 1;
                playerStats.rhetoric += 1;
            }
        }
        else if (skillName == "Encyclopedia")
        {
            if (isSignature)
            {
                ArticyGlobalVariables.Default.PlayerStats.Encyclopedia += 2;
                playerStats.encyclopedia += 2;
            }
            else if (!isSignature)
            {
                ArticyGlobalVariables.Default.PlayerStats.Encyclopedia += 1;
                playerStats.encyclopedia += 1;
            }
        }
        else if (skillName == "Logic")
        {
            if (isSignature)
            {
                ArticyGlobalVariables.Default.PlayerStats.Logic += 2;
                playerStats.logic += 2;
            }
            else if (!isSignature)
            {
                ArticyGlobalVariables.Default.PlayerStats.Logic += 1;
                playerStats.logic += 1;
            }
        }
        else if (skillName == "Perception")
        {
            if (isSignature)
            {
                ArticyGlobalVariables.Default.PlayerStats.Perception += 2;
                playerStats.perception += 2;
            }
            else if (!isSignature)
            {
                ArticyGlobalVariables.Default.PlayerStats.Perception += 1;
                playerStats.perception += 1;
            }
        }
        else if (skillName == "Conceptualization")
        {
            if (isSignature)
            {
                ArticyGlobalVariables.Default.PlayerStats.Conceptualization += 2;
                playerStats.conceptualization += 2;
            }
            else if (!isSignature)
            {
                ArticyGlobalVariables.Default.PlayerStats.Conceptualization += 1;
                playerStats.conceptualization += 1;
            }
        }
        else if (skillName == "Savoir-Faire")
        {
            if (isSignature)
            {
                ArticyGlobalVariables.Default.PlayerStats.SavoirFaire += 2;
                playerStats.savoirFaire += 2;
            }
            else if (!isSignature)
            {
                ArticyGlobalVariables.Default.PlayerStats.SavoirFaire += 1;
                playerStats.savoirFaire += 1;
            }
        }
        else if (skillName == "Perspicacity")
        {
            if (isSignature)
            {
                ArticyGlobalVariables.Default.PlayerStats.Perspicacity += 2;
                playerStats.perspicacity += 2;
            }
            else if (!isSignature)
            {
                ArticyGlobalVariables.Default.PlayerStats.Perspicacity += 1;
                playerStats.perspicacity += 1;
            }
        }
        else if (skillName == "Tenebrality")
        {
            if (isSignature)
            {
                ArticyGlobalVariables.Default.PlayerStats.Tenebrality += 2;
                playerStats.tenebrality += 2;
            }
            else if (!isSignature)
            {
                ArticyGlobalVariables.Default.PlayerStats.Tenebrality += 1;
                playerStats.tenebrality += 1;
            }
        }
        else if (skillName == "Self-Actualization")
        {
            if (isSignature)
            {
                ArticyGlobalVariables.Default.PlayerStats.SelfActualization += 2;
                playerStats.selfActualization += 2;
            }
            else if (!isSignature)
            {
                ArticyGlobalVariables.Default.PlayerStats.SelfActualization += 1;
                playerStats.selfActualization += 1;
            }
        }
    }
}
