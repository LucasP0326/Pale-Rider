using UnityEngine;
using StarterAssets;

public class ThoughtTrigger : MonoBehaviour
{
    public bool hasBeenInteracted = false; // Tracks if the object has been interacted with
    [Header("Thought Type")]
    public bool reptilianThought = false;
    public bool paleomammalianThought = false;
    public bool neomammalianThought = false;
    public bool paleThought = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // This method is called when another collider enters the trigger collider attached to this object
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasBeenInteracted)
        {
            // Show the thought bubble or perform any other action here
            Debug.Log("Thought Trigger Activated!");
            // Example: Show a thought bubble or play a sound
            other.GetComponent<ThirdPersonController>().hasThought = true; // Set the thought state to true
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<ThirdPersonController>().hasThought = false;
        }
    }
}
