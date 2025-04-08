using UnityEngine;

public class ScreenFaceRotate : MonoBehaviour
{
    private Quaternion desiredRotation = Quaternion.Euler(30f, 45f, 0f);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = desiredRotation; // Set the rotation to the desired rotation
    }
}
