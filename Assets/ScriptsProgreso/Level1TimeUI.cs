using UnityEngine;
using TMPro;

public class Level1TimeUI : MonoBehaviour
{
    public TMP_Text timeText;

    void Start()
    {
        RefreshUI();
    }

    public void RefreshUI()
    {
        timeText.text = LevelTimer.GetFormattedBestTime();
    }

}
