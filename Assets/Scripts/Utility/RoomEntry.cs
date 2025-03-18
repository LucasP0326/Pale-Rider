using UnityEngine;

public class RoomEntry : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public bool firstEntry = true;
    public bool inRoom;
    public GameObject blackBox;
    public GameObject roomWalls;
    public GameObject roomInvisibleWalls;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (firstEntry)
            blackBox.SetActive(true);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (firstEntry)
            {
                blackBox.SetActive(false);
                firstEntry = false;
            }
            roomWalls.SetActive(false);
            roomInvisibleWalls.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            roomWalls.SetActive(true);
            roomInvisibleWalls.SetActive(false);
        }
    }

    public void EnterExitRoom()
    {

    }
}
