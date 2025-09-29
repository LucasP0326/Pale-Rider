using UnityEngine;

public class ThoughtInteractionManager : MonoBehaviour
{
    public GameObject thoughtTrigger; // The current trigger the player is inside

    // Call this from the thought bubble's OnClick event

    private void OnClick()
    {
        OnThoughtBubbleClicked();
    }

    private void OnMouseDown()
    {
        OnThoughtBubbleClicked();
    }

    public void OnThoughtBubbleClicked()
    {
        if (thoughtTrigger != null)
        {
            var interactable = thoughtTrigger.GetComponent<Interactable>();
            if (interactable != null)
            {
                interactable.OnInteract(); // Replace with your actual interaction method
            }
        }

        if (thoughtTrigger.GetComponent<ThoughtTrigger>().reptilianThought)
        {
            thoughtTrigger.GetComponent<ThoughtTrigger>().reptilianThought = false; // Set to false after interaction
        }
        if (thoughtTrigger.GetComponent<ThoughtTrigger>().paleomammalianThought)
        {
            thoughtTrigger.GetComponent<ThoughtTrigger>().paleomammalianThought = false; // Set to false after interaction
        }
        if (thoughtTrigger.GetComponent<ThoughtTrigger>().neomammalianThought)
        {
            thoughtTrigger.GetComponent<ThoughtTrigger>().neomammalianThought = false; // Set to false after interaction
        }
        if (thoughtTrigger.GetComponent<ThoughtTrigger>().paleThought)
        {
            thoughtTrigger.GetComponent<ThoughtTrigger>().paleThought = false; // Set to false after interaction
        }
    }
}
