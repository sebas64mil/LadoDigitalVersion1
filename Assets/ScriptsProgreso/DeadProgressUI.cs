using UnityEngine;
using TMPro;

public class DeadProgressUI : MonoBehaviour
{
    public static DeadProgressUI Instance;

    public TMP_Text deathText;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        int x = DeadProgressPlayer.GetDeathsModulo();
        int y = DeadProgressPlayer.GetDeathsHundreds();
        deathText.text = $"{x}/{y}";
    }
}
