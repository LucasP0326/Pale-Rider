using UnityEngine;

public class ClickIndicator : MonoBehaviour
{
    public float duration = 0.5f;  // Time before disappearing
    private float timer;
    private SpriteRenderer spriteRenderer;
    private Vector3 initialScale;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        initialScale = transform.localScale;
        timer = duration;
    }

    // Update is called once per frame
    void Update()
    {
       // Reduce timer
        timer -= Time.deltaTime;
        
        // Expand the circle
        float scaleFactor = 1 + (1 - (timer / duration)); // Expands from 1x to 2x size
        transform.localScale = initialScale * scaleFactor;
        
        // Fade out effect
        float alpha = timer / duration; 
        spriteRenderer.color = new Color(1f, 1f, 1f, alpha);
        
        // Destroy once time is up
        if (timer <= 0)
        {
            Destroy(gameObject);
        } 
    }
}
