using UnityEngine;
using Articy.Unity; // Import Articy namespace
using Articy.Unity.Interfaces;
using Articy.Pale_Rider;
using Articy.Pale_Rider.GlobalVariables;

public class PlayerLeaveRoom : MonoBehaviour
{
    private bool leftRoom = false; // Flag to check if the player has left the room
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        leftRoom = ArticyGlobalVariables.Default.GlobalVariables.LeftRoom;
    }

    // Update is called once per frame
    void Update()
    {
        ArticyGlobalVariables.Default.GlobalVariables.LeftRoom = leftRoom; // Sync the Articy variable with the local flag
    }

    public void LeaveRoom()
    {
        leftRoom = true;
    }
}
