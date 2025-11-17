using StarterAssets;
using UnityEngine;

public class IsometricCameraZoom : MonoBehaviour
{
    public float zoomSpeed = 2f;  // Speed of zoom
    public float minZoom = 3f;    // Minimum zoom limit
    public float maxZoom = 15f;   // Maximum zoom limit
    public float horseZoomModifier = 2f; // Zoom modifier when on horse
    public ThirdPersonController playerController;

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
        playerController = FindFirstObjectByType<ThirdPersonController>();
    }

    void Update()
    {
        // Check if we can zoom (not paused and not in dialogue)
        if (Time.timeScale == 0 || playerController.inDialogue)
            return;

        float scrollInput = Input.GetAxis("Mouse ScrollWheel");  // Get mouse scroll input
        
        if (scrollInput != 0)
        {
            // Apply double zoom speed if mounted
            float currentZoomSpeed = zoomSpeed * (playerController.isMounted ? horseZoomModifier : 1f);
            
            // Adjust zoom with modifier if mounted
            cam.orthographicSize -= scrollInput * currentZoomSpeed;
            
            // Clamp zoom limits
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);
        }
    }
}