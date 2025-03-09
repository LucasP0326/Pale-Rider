using UnityEngine;

public class IsometricCameraZoom : MonoBehaviour
{
    public float zoomSpeed = 2f;  // Speed of zoom
    public float minZoom = 3f;    // Minimum zoom limit
    public float maxZoom = 15f;   // Maximum zoom limit

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");  // Get mouse scroll input
        if (scrollInput != 0)
        {
            cam.orthographicSize -= scrollInput * zoomSpeed;  // Adjust zoom
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);  // Clamp zoom limits
        }
    }
}