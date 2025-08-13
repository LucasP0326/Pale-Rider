using UnityEngine;
using UnityEngine.UI;
using StarterAssets;
using System.Collections;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject player;
    public string mainMenuName;

    //Important References
    private InventoryManager inventoryManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        inventoryManager = FindObjectOfType<InventoryManager>();
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
        inventoryManager.SaveInventory();
    }

    public void LoadGame()
    {
        //Insert More
    }
}
