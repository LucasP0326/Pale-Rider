using UnityEngine;

public class Thoughts : MonoBehaviour
{
    public float rotationSpeed = 10f; // Speed of rotation

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        {
            // Rotate the sprite around its Z-axis
            transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
        }
    }
}
