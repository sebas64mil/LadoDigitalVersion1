using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonSound : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    [Header("Audio Clips")]
    public AudioClip hoverClip;
    public AudioClip clickClip;

    public void OnPointerEnter(PointerEventData eventData)
    {
        UIAudioManager.Instance.PlaySound(hoverClip);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        UIAudioManager.Instance.PlaySound(clickClip);
    }
}
