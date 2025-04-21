using UnityEngine;

public class ThoughtInteractionManager : MonoBehaviour
{
    public GameObject thoughtTrigger; // The current trigger the player is inside

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object has the Interactable script
        if (other.GetComponent<Interactable>() != null)
        {
            thoughtTrigger = other.gameObject; // Assign the GameObject to thoughtTrigger
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the object has the Interactable script
        if (other.GetComponent<Interactable>() != null && thoughtTrigger == other.gameObject)
        {
            thoughtTrigger = null; // Clear the thoughtTrigger when exiting the trigger
        }
    }
}
