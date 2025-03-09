using UnityEngine;

public class IsometricCameraFollow : MonoBehaviour  // Inherits from MonoBehaviour
{
    public Transform target;  // Assign the player in the Inspector
    public float smoothSpeed = 5f;

    public Vector3 offset;

    void Start()
    {
        if (target == null)
        {
            Debug.LogError("IsometricCameraFollow: No target assigned!");
            return;
        }
    }

    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 targetPosition = target.position + offset;
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
        }
    }
}