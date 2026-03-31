using UnityEngine;
using Articy.Unity;
using Articy.Unity.Interfaces;
using Articy.Pale_Rider;
using Articy.Pale_Rider.GlobalVariables;

public class PlayerEquipment : MonoBehaviour
{

    public GameObject rifle;
    //public GameObject pistol;
    public GameObject hat;
    public GameObject mask;
    public GameObject maskTube;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Weapons
        if (ArticyGlobalVariables.Default.EquippedItems.EquippedTool == "Tool_KonstanzRifleBroken" || ArticyGlobalVariables.Default.EquippedItems.EquippedTool == "Tool_KonstanzRifle")
        {
            rifle.SetActive(true);
            //pistol.SetActive(false);
        }
        else if (ArticyGlobalVariables.Default.EquippedItems.EquippedTool == "Tool_KonstanzRevolver")
        {
            rifle.SetActive(false);
            //pistol.SetActive(true);
        }
        else
        {
            rifle.SetActive(false);
            //pistol.SetActive(false);
        }

        //Hats
        if (ArticyGlobalVariables.Default.EquippedItems.EquippedHead == "Clothing_HisperianRancherHat")
        {
            hat.SetActive(true);
        }
        else
        {
            hat.SetActive(false);
        }

        //Masks
        if (ArticyGlobalVariables.Default.EquippedItems.EquippedFace == "Clothing_GasMask")
        {
            mask.SetActive(true);
            maskTube.SetActive(true);
        }
        else
        {
            mask.SetActive(false);
            maskTube.SetActive(false);
        }
    }  
}
