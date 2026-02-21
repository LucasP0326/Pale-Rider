using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;
using Articy.Unity.Interfaces;
using Articy.Pale_Rider;
using Articy.Pale_Rider.GlobalVariables;
using UnityEngine.UI;
using StarterAssets;

public class ZuretonInn : MonoBehaviour
{
    public ThirdPersonController playerController;
    public GameObject pubTender;
    public GameObject sitLocation;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = FindObjectOfType<ThirdPersonController>();
        if (ArticyGlobalVariables.Default.GlobalVariables.ZuretonInnFirstTime == true)
        {
            ArticyGlobalVariables.Default.GlobalVariables.SpawnPoint = "InnBarStool";
            playerController.transform.Rotate(0, 180, 0);
            StartCoroutine(FirstTimeSequence());
        }
        else
        return;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator FirstTimeSequence()
    {
        yield return new WaitForSeconds(0.25f);
        playerController.transform.position = sitLocation.transform.position;
        pubTender.GetComponent<Interactable>().OnInteract();
        playerController.isMounted = true;
    }
}
