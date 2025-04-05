using UnityEngine;

public class InteractionBubble : MonoBehaviour
{
    private Interactable parentInteractable;

    // Desired rotation for the bubble to appear as a perfect circle
    private Quaternion desiredRotation = Quaternion.Euler(45f, 30f, 0f);

    public void Setup(Interactable interactable)
    {
        parentInteractable = interactable;
    }

    private void Update()
    {
        transform.rotation = desiredRotation; // Set the rotation to the desired rotation
    }

    private void OnMouseDown()
    {
        if (parentInteractable != null)
        {
            parentInteractable.OnInteract();
            parentInteractable.hasBeenInteracted = true; // Set the flag to true after interaction
        }
    }

    private void OnMouseEnter()
    {
        transform.localScale = new Vector3(0.013f, 0.013f, 0.013f); // Scale up the bubble when hovered over
    }

    private void OnMouseExit()
    {
        transform.localScale = new Vector3(0.012f, 0.012f, 0.012f); // Scale down the bubble when not hovered over
    }
}