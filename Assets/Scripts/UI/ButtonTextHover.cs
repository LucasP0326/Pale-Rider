using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ButtonTextHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TMP_Text buttonText;
    public Color normalColor = Color.white;
    public Color highlightColor = Color.yellow;

    void Start()
    {
        buttonText.color = normalColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonText.color = highlightColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonText.color = normalColor;
    }
}