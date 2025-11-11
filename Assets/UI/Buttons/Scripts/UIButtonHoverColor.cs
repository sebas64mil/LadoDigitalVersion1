using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class UIButtonHoverColor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Colors")]
    public Color normalColor = Color.white;
    public Color hoverColor = Color.yellow;

    private TMP_Text uiText;

    private void Awake()
    {
        uiText = GetComponentInChildren<TMP_Text>();
        if (uiText != null)
            uiText.color = normalColor;

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (uiText == null) return;
        uiText.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (uiText == null) return;
        uiText.color = normalColor;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (uiText == null) return;
        uiText.color = normalColor;
    }
}
