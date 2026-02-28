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
    //UI Elements
    public GameObject skillCheckPanel;
    public GameObject skillCheckResultBar;
    public TextMeshProUGUI skillCheckResultTMP;
    public Sprite[] diceImages; // Array to hold the dice images
    public GameObject diceLocation1;
    public GameObject diceLocation2;

    //SFX Elements
    public AudioSource audioSource;
    public AudioClip diceRollSFX;
    public AudioClip successSFX;
    public AudioClip failureSFX;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ArticyGlobalVariables.Default.SkillCheckStats.PerformingSkillCheck == true)
        {
            StartCoroutine(DiceRoll());
            ArticyGlobalVariables.Default.SkillCheckStats.PerformingSkillCheck = false;
        }
    }
    
    private IEnumerator DiceRoll()
    {
        skillCheckPanel.SetActive(true);
        audioSource.PlayOneShot(diceRollSFX);
        StartCoroutine(AnimateDiceRoll());
        yield return new WaitForSeconds(0.5f);
        skillCheckResultBar.SetActive(true);
        if (ArticyGlobalVariables.Default.SkillCheckStats.FinalDice >= ArticyGlobalVariables.Default.SkillCheckStats.Difficulty)
        {
            skillCheckResultTMP.text = "Success!";
            skillCheckResultBar.GetComponent<Image>().color = Color.green;
            audioSource.PlayOneShot(successSFX);
        }
        else
        {
            skillCheckResultTMP.text = "Failure!";
            skillCheckResultBar.GetComponent<Image>().color = Color.red;
            audioSource.PlayOneShot(failureSFX);
        }
        yield return new WaitForSeconds(4f);
        skillCheckResultBar.SetActive(false);
        skillCheckPanel.SetActive(false);
    }
    
    private IEnumerator AnimateDiceRoll()
    {
        float animationDuration = 0.75f; // Duration of the dice roll animation
        float elapsedTime = 0f;

        int displayedFinalDice1 = ArticyGlobalVariables.Default.SkillCheckStats.Dice1;
        int displayedFinalDice2 = ArticyGlobalVariables.Default.SkillCheckStats.Dice2;

        while (elapsedTime < animationDuration)
        {
            int randomDice1 = Random.Range(0, diceImages.Length);
            int randomDice2 = Random.Range(0, diceImages.Length);

            // Update the dice images
            diceLocation1.GetComponent<Image>().sprite = diceImages[randomDice1];
            diceLocation2.GetComponent<Image>().sprite = diceImages[randomDice2];

            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        if (elapsedTime >= animationDuration)
        {
            audioSource.Stop();
        }

        // Set the final dice images based on the randomRoll value
        displayedFinalDice1 = Mathf.Clamp(displayedFinalDice1 - 1, 0, diceImages.Length - 1);
        displayedFinalDice2 = Mathf.Clamp(displayedFinalDice2 - 1, 0, diceImages.Length - 1);
        diceLocation1.GetComponent<Image>().sprite = diceImages[displayedFinalDice1];
        diceLocation2.GetComponent<Image>().sprite = diceImages[displayedFinalDice2];

        //rollingDice = false;
    }
}
