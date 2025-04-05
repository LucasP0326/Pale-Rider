using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main; // Cache the main camera
    }

    void Update()
    {
        if (mainCamera != null)
        {
            // Make the object face the camera directly
            Vector3 cameraForward = mainCamera.transform.forward;
            transform.rotation = Quaternion.LookRotation(cameraForward, Vector3.up);
        }
    }
}