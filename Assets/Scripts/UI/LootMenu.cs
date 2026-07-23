using UnityEngine;

public class LootMenu : MonoBehaviour
{
    private Interactable parentInteractable;
    private Quaternion desiredRotation = Quaternion.Euler(45f, 30f, 0f);
    private GameObject player;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player"); // Find the player object by tag
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = desiredRotation; // Set the rotation to the desired rotation
    }

    public void Setup(Interactable interactable)
    {
        parentInteractable = interactable;
    }
}
