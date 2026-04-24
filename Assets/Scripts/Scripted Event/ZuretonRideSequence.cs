using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;
using Articy.Unity.Interfaces;
using Articy.Pale_Rider;
using Articy.Pale_Rider.GlobalVariables;
using UnityEngine.UI;
using StarterAssets;

public class ZuretonRideSequence : MonoBehaviour
{

    public Transform cutsceneRideCoordinate;
    public Transform playerFallCoordinate;
    public LeavePaleOpeningSequence leavePaleOpeningScript;
    public GameObject horse;
    public GameObject fullHorse;
    public GameObject player;
    public GameObject deadplayer;
    public ThirdPersonController playerController;
    public HorseManager horseController;
    public GameObject fadeToBlackPanel; //Black UI image for fade effect
    public string nextSceneName;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        leavePaleOpeningScript = FindObjectOfType<LeavePaleOpeningSequence>();
        playerController = FindObjectOfType<ThirdPersonController>();
        horseController = FindObjectOfType<HorseManager>();
        if (leavePaleOpeningScript != null)
        {
            if (ArticyGlobalVariables.Default.GlobalVariables.KeptHorse == true)
                StartCoroutine(WalkInSequence()); //Change once I figure out how to fix ride sequence
            else if (ArticyGlobalVariables.Default.GlobalVariables.KeptHorse == false)
                StartCoroutine(WalkInSequence());
            else if (ArticyGlobalVariables.Default.GlobalVariables.KeptHorse == null)
            {
                Debug.Log("hasHorse variable is not set in LeavePaleOpeningSequence script.");
                StartCoroutine(WalkInSequence());
            }
        }
        else
        {
            Debug.Log("LeavePaleOpeningSequence script not found in the scene.");
            StartCoroutine(WalkInSequence());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator RideInSequence()
    {
        playerController.movementEnabled = false;
        playerController.tempInteractableObject = horse;
        horse.GetComponent<Interactable>().OnMouseDown();
        yield return new WaitForSeconds(3f);
        cutsceneRideCoordinate.GetComponent<Interactable>().OnMouseDown();
        horseController.MoveToClick(cutsceneRideCoordinate.position);
        yield return new WaitForSeconds(5f);
        // Fade to black
        yield return StartCoroutine(FadeToBlack(2f));
        yield return new WaitForSeconds(2f);
        horseController.currentSpeed = 0f;
        horseController.horseAnimator.SetBool("IsIdling", true);
        horse.GetComponent<Interactable>().OnMouseDown();
        yield return new WaitForSeconds(1f);
        playerController.movementEnabled = true;
        playerController.MoveToClick(playerFallCoordinate.position);
        playerController._isMovingToClick = false;
        player.SetActive(false);
        deadplayer.SetActive(true);
        yield return new WaitForSeconds(2f);
        // Fade from black
        fullHorse.SetActive(false);
        yield return StartCoroutine(FadeFromBlack(4f));
        yield return new WaitForSeconds(3f);
        // Fade to black indefinitely
        yield return StartCoroutine(FadeToBlack(5f));
        SceneManager.LoadScene(nextSceneName);
    }

    private IEnumerator WalkInSequence()
    {
        fullHorse.SetActive(false);
        playerController.movementEnabled = false;
        yield return new WaitForSeconds(1f);
        playerController.movementEnabled = true;
        playerController.MoveToClick(playerFallCoordinate.position);
        yield return new WaitForSeconds(5f);
        yield return StartCoroutine(FadeToBlack(2f));
        yield return new WaitForSeconds(2f);
        playerController._isMovingToClick = false;
        player.SetActive(false);
        deadplayer.SetActive(true);
        yield return new WaitForSeconds(2f);
        // Fade from black
        yield return StartCoroutine(FadeFromBlack(4f));
        yield return new WaitForSeconds(3f);
        // Fade to black indefinitely
        yield return StartCoroutine(FadeToBlack(5f));
        SceneManager.LoadScene(nextSceneName);
    }

    private IEnumerator FadeToBlack(float duration)
    {
        Image fadeImage = fadeToBlackPanel.GetComponent<Image>();
        Color color = fadeImage.color;
        float elapsedTime = 0f;
        
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsedTime / duration);
            fadeImage.color = color;
            yield return null;
        }
        
        color.a = 1f;
        fadeImage.color = color;
    }

    private IEnumerator FadeFromBlack(float duration)
    {
        Image fadeImage = fadeToBlackPanel.GetComponent<Image>();
        Color color = fadeImage.color;
        float elapsedTime = 0f;
        
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Clamp01(1f - (elapsedTime / duration));
            fadeImage.color = color;
            yield return null;
        }
        
        color.a = 0f;
        fadeImage.color = color;
    }
}
