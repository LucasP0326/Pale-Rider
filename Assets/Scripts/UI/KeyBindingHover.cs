using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class KeyBindingHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Color normalColor = Color.white;
    public Color hoverColor = Color.yellow;
    public Image targetImage; // Assign the Image component in Inspector
    public TMP_Text hoverText; // Assign the TMP_Text in Inspector
    public GameObject hoverBackdrop; // Assign the background GameObject in Inspector
    public string textToShow = "Key Info";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (targetImage != null)
            targetImage.color = normalColor;
        if (hoverText != null)
            hoverText.gameObject.SetActive(false);
        if (hoverBackdrop != null)
            hoverBackdrop.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (targetImage != null)
            targetImage.color = hoverColor;
        if (hoverText != null)
        {
            hoverText.text = textToShow;
            hoverText.gameObject.SetActive(true);
            if (hoverBackdrop != null)
                hoverBackdrop.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (targetImage != null)
            targetImage.color = normalColor;
        if (hoverText != null)
        {
            hoverText.gameObject.SetActive(false);
            if (hoverBackdrop != null)
                hoverBackdrop.SetActive(false);
        }
    }
}
