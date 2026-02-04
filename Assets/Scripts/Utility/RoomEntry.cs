using UnityEngine;

public class RoomEntry : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public bool firstEntry = true;
    public bool inRoom;
    public GameObject blackBox;
    public GameObject[] roomWalls; // Changed to an array
    public GameObject roomInvisibleWalls;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (firstEntry)
            if (blackBox != null)
                blackBox.SetActive(true);
        if (inRoom)
        {
            foreach (var wall in roomWalls) // Loop through each wall
            {
                if (wall != null)
                    wall.SetActive(false);
            }
            if (roomInvisibleWalls != null)
                roomInvisibleWalls.SetActive(true);
        }
        else if (!inRoom)
        {
            // Only set walls to active if no other RoomEntry scripts are keeping them disabled
            if (!IsAnyOtherRoomActive())
            {
                Debug.Log("No other rooms active, enabling walls.");
                foreach (var wall in roomWalls) // Loop through each wall
                {
                    if (wall != null)
                        wall.SetActive(true);
                }
            }
            else
            {
                Debug.Log("Another room is still active, keeping walls disabled.");
                if (roomInvisibleWalls != null)
                    roomInvisibleWalls.SetActive(false);
            } 
        }
        else
        {
            Debug.Log("Man, I don't fucking know.  I was trying my best!  :,()");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRoom = true;
            if (firstEntry)
            {
                if (blackBox != null)
                    blackBox.SetActive(false);
                firstEntry = false;
            }
            /*if (roomWalls != null)
                roomWalls.SetActive(false);
            if (roomInvisibleWalls != null)
                roomInvisibleWalls.SetActive(true);*/
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRoom = false;
            /*if (roomWalls != null)
                roomWalls.SetActive(true);
            if (roomInvisibleWalls != null)
                roomInvisibleWalls.SetActive(false);*/
        }
    }

    public void EnterExitRoom()
    {

    }

    /// <summary>
    /// Checks if any other RoomEntry scripts in the scene have inRoom set to true
    /// </summary>
    /// <returns>True if any other RoomEntry is active in a room, false otherwise</returns>
    private bool IsAnyOtherRoomActive()
    {
        RoomEntry[] allRoomEntries = FindObjectsOfType<RoomEntry>();
        
        foreach (var roomEntry in allRoomEntries)
        {
            // Skip this RoomEntry instance
            if (roomEntry == this)
                continue;
            
            // If any other RoomEntry has inRoom == true, return true
            if (roomEntry.inRoom)
                return true;
        }
        
        return false;
    }
}
