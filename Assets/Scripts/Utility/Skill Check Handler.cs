using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;
using Articy.Unity.Interfaces;
using Articy.Pale_Rider;
using Articy.Pale_Rider.GlobalVariables;
using UnityEngine.UI;
using StarterAssets;

public class SkillCheckHandler : MonoBehaviour
{
    public int playerStatValue;
    public int difficulty;
    public int randomRoll;
    public int finalValue;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if  (ArticyGlobalVariables.Default.SkillCheckStats.PerformingSkillCheck == true)
        {
            PerformSkillCheck(ArticyGlobalVariables.Default.SkillCheckStats.CheckedSkill);
        }
    }

    public void PerformSkillCheck(string skill)
    {
        playerStatValue = 0;
        difficulty = ArticyGlobalVariables.Default.SkillCheckStats.Difficulty;
        switch (skill)
        {
            case "Authority":
                playerStatValue = ArticyGlobalVariables.Default.PlayerStats.Authority;
                break;
            case "Conceptualization":
                playerStatValue = ArticyGlobalVariables.Default.PlayerStats.Conceptualization;
                break;
            case "Encyclopedia":
                playerStatValue = ArticyGlobalVariables.Default.PlayerStats.Encyclopedia;
                break;
            case "Empathy":
                playerStatValue = ArticyGlobalVariables.Default.PlayerStats.Empathy;
                break;
            case "Endurance":
                playerStatValue = ArticyGlobalVariables.Default.PlayerStats.Endurance;
                break;
            case "Logic":
                playerStatValue = ArticyGlobalVariables.Default.PlayerStats.Logic;
                break;
            case "Perception":
                playerStatValue = ArticyGlobalVariables.Default.PlayerStats.Perception;
                break;
            case "Perspicacity":
                playerStatValue = ArticyGlobalVariables.Default.PlayerStats.Perspicacity;
                break;
            case "Physicality":
                playerStatValue = ArticyGlobalVariables.Default.PlayerStats.Physicality;
                break;
            case "Reflexivity":
                playerStatValue = ArticyGlobalVariables.Default.PlayerStats.Reflexivity;
                break;
            case "Rhetoric":
                playerStatValue = ArticyGlobalVariables.Default.PlayerStats.Rhetoric;
                break;
            case "Savoir Faire":
                playerStatValue = ArticyGlobalVariables.Default.PlayerStats.SavoirFaire;
                break;
            case "Self Actualization":
                playerStatValue = ArticyGlobalVariables.Default.PlayerStats.SelfActualization;
                break;
            case "Suggestion":
                playerStatValue = ArticyGlobalVariables.Default.PlayerStats.Suggestion;
                break;
            case "Tenebrality":
                playerStatValue = ArticyGlobalVariables.Default.PlayerStats.Tenebrality;
                break;
            case "Volition":
                playerStatValue = ArticyGlobalVariables.Default.PlayerStats.Volition;
                break;
            default:
                Debug.LogError("Invalid skill name for skill check: " + skill);
                return;
        }

        randomRoll = Random.Range(1, 12); // Simulate a d20 roll

        finalValue = playerStatValue + randomRoll;

        if (finalValue >= ArticyGlobalVariables.Default.SkillCheckStats.Difficulty)
        {
            Debug.Log("Skill Check Passed!");
            ArticyGlobalVariables.Default.SkillCheckStats.SkillCheckResult = 1; // Indicate success
            // Handle success logic here
        }
        else
        {
            Debug.Log("Skill Check Failed!");
            ArticyGlobalVariables.Default.SkillCheckStats.SkillCheckResult = 0; // Indicate failure
            // Handle failure logic here
        }

        // Reset skill check state
        //ArticyGlobalVariables.Default.SkillCheckStats.PerformingSkillCheck = false;
        //ArticyGlobalVariables.Default.SkillCheckStats.CheckedSkill = "";
        //ArticyGlobalVariables.Default.SkillCheckStats.Difficulty = 0;
    }
}
