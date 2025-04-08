using UnityEngine;
using UnityEngine.Events;

public class EventTrigger : MonoBehaviour
{
    [Header("Event Settings")]
    public UnityEvent onTriggerEnter; // Event to trigger when the player enters
    private bool hasTriggered = false; // Tracks if the event has already been triggered

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger is the player
        if (!hasTriggered && other.CompareTag("Player"))
        {
            hasTriggered = true; // Mark the event as triggered
            onTriggerEnter?.Invoke(); // Invoke the assigned event
        }
    }
}
