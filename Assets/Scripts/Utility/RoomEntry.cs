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
            if (blackBox != null)
                blackBox.SetActive(true);
        if (inRoom)
        {
            if (roomWalls != null)
                roomWalls.SetActive(false);
            if (roomInvisibleWalls != null)
                roomInvisibleWalls.SetActive(true);
        }
        else if (!inRoom)
        {
            if (roomWalls != null)
                roomWalls.SetActive(true);
            if (roomInvisibleWalls != null)
                roomInvisibleWalls.SetActive(false); 
        }
        else
        {
            Debug.Log("Man, I don't fucking know.");
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
}
