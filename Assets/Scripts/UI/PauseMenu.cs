using UnityEngine;
using UnityEngine.UI;
using StarterAssets;
using System.Collections;
using UnityEngine.SceneManagement;
using Articy.Unity;
using Articy.Unity.Interfaces;
using Articy.Pale_Rider;
using Articy.Pale_Rider.GlobalVariables;

public class PauseMenu : MonoBehaviour
{
    public GameObject player;
    public string mainMenuName;
    public GameObject optionsPanel;
    public GameObject mainPanel;

    //Important References
    private InventoryManager inventoryManager;
    private SaveManager saveManager;
    public bool optionsOpen = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        inventoryManager = FindObjectOfType<InventoryManager>();
        saveManager = FindObjectOfType<SaveManager>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Intro()
    {
        Debug.Log("Glide in Here!");
    }

    public void Resume()
    {
        player.GetComponent<ThirdPersonController>().Pause2();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(mainMenuName);
    }

    public void SaveGame()
    {
        saveManager.SaveGame();
    }

    public void LoadGame()
    {
        saveManager.LoadGame();
    }

    public void ResetGame()
    {
        saveManager.ResetGame();
    }

    public void Options()
    {
        optionsPanel.SetActive(true);
        optionsOpen = true;
        mainPanel.SetActive(false);
    }

    public void CloseOptions()
    {
        optionsOpen = false;
        optionsPanel.SetActive(false);
        mainPanel.SetActive(true);
    }
}
