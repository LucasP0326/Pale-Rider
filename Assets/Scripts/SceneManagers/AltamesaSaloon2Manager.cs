using UnityEngine;
using Articy.Pale_Rider.GlobalVariables;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Reflection;
using System;
using StarterAssets;
using UnityEngine.Events;

public class AltamesaSaloon2Manager : MonoBehaviour
{
    //In Scene Variables
    public GameObject dialogueStarter;
    public GameObject roomDoor;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitializeScene();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitializeScene()
    {
        if (ArticyGlobalVariables.Default.GlobalVariables.WokeUp == true)
        {
            if (dialogueStarter != null)
                dialogueStarter.SetActive(false);
        }
    }
}