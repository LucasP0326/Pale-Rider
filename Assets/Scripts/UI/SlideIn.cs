using UnityEngine;

public class SlideIn : MonoBehaviour
{
    [Header("Slide Settings")]
    [Tooltip("The starting position of the UI element (relative to its parent).")]
    public Vector3 startPosition = new Vector3(-1000, 0, 0); // Default: off-screen to the left
    [Tooltip("The target position of the UI element (relative to its parent).")]
    public Vector3 targetPosition = Vector3.zero; // Default: center of the parent
    [Tooltip("The speed of the slide-in animation.")]
    public float slideSpeed = 5f;

    public RectTransform rectTransform;
    public bool isSliding = false;

    void Start()
    {
        
    }
    
    void OnEnable()
    {
        // Initialize the RectTransform and set the starting position
        rectTransform = GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            rectTransform.anchoredPosition = startPosition;
            isSliding = true; // Start the slide-in animation
        }
    }

    void Update()
    {
        if (isSliding && rectTransform != null)
        {
            Vector3 previousPosition = rectTransform.anchoredPosition;

            // Smoothly move the UI element toward the target position
            rectTransform.anchoredPosition = Vector3.Lerp(
                rectTransform.anchoredPosition,
                targetPosition,
                Time.deltaTime * slideSpeed
            );

            // Check if the position is stuck (not moving)
            if (Vector3.Distance(previousPosition, rectTransform.anchoredPosition) < 0.001f)
            {
                rectTransform.anchoredPosition = targetPosition;
                isSliding = false; // Stop the animation
                return;
            }

            // Stop sliding when close enough to the target position
            if (Vector3.Distance(rectTransform.anchoredPosition, targetPosition) < 0.1f)
            {
                rectTransform.anchoredPosition = targetPosition;
                isSliding = false; // Stop the animation
            }
        }
    }
}
