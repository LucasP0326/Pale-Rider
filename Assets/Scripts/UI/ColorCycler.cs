using UnityEngine;
using UnityEngine.UI;

public class ColorCycler : MonoBehaviour
{
    public Image targetImage;
    public float cycleSpeed = 1f; // Speed of transition

    private float hue = 0f; 

    void Update()
    {
        // Use unscaled time so it still updates while paused
        hue += cycleSpeed * Time.unscaledDeltaTime;
        if (hue > 1f) hue -= 1f; 

        // Get current alpha value
        Color currentColor = targetImage.color;
        float alpha = currentColor.a; // Preserve alpha

        // Apply new color while keeping original alpha
        targetImage.color = new Color(Color.HSVToRGB(hue, 1f, 1f).r, 
                                    Color.HSVToRGB(hue, 1f, 1f).g, 
                                    Color.HSVToRGB(hue, 1f, 1f).b, 
                                    alpha);
    }
}
