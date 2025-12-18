using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class UIButtonHoverColor : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Colors")]
    public Color normalColor = Color.white;
    public Color hoverColor = Color.yellow;
    public Color disabledColor = Color.gray;   // Nuevo color cuando el botón está desactivado

    private TMP_Text uiText;
    private Button button;

    private void Awake()
    {
        uiText = GetComponentInChildren<TMP_Text>();
        button = GetComponent<Button>();

        UpdateTextColor();
    }

    private void OnEnable()
    {
        UpdateTextColor();
    }


    private void UpdateTextColor()
    {
        if (uiText == null) return;

        if (button != null && !button.interactable)
            uiText.color = disabledColor;
        else
            uiText.color = normalColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (uiText == null || (button != null && !button.interactable)) return;

        uiText.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (uiText == null || (button != null && !button.interactable)) return;

        uiText.color = normalColor;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (uiText == null || (button != null && !button.interactable)) return;

        uiText.color = normalColor;
    }
}
