using UnityEngine;
using UnityEngine.UI;
using StarterAssets;
using System.Collections;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject player;
    public string mainMenuName;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
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
}
