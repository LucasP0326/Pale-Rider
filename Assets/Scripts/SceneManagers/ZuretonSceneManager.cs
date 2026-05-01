using UnityEngine;
using Articy.Pale_Rider.GlobalVariables;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Reflection;
using System;
using StarterAssets;

public class ZuretonSceneManager : MonoBehaviour
{
    //Important References
    public ThirdPersonController playerController;
    public GameObject horse;
    public GameObject beer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Assign References
        playerController = FindObjectOfType<ThirdPersonController>();

        if (horse != null)
            horse.SetActive(ArticyGlobalVariables.Default.GlobalVariables.KeptHorse);
        if (beer != null && ArticyGlobalVariables.Default.Quests.FindBeerNewHome > 0)
            beer.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReloadScene()
    {
        Start();
        if (!ArticyGlobalVariables.Default.GlobalVariables.ZuretonInnFirstTime)
            playerController.Dismount();
    }
}
