using TMPro;
using UnityEngine;

public class MissionUI : MonoBehaviour
{
    public static MissionUI Instance;

    public TMP_Text missionTextUI;


    [Header("Sonidos Config")]
    public AudioClip openSound;
    private SFXAudioController audioCtrl;

    private void OnEnable()
    {
        PlayerProgressManager.OnMissionChanged += UpdateMission;
    }

    private void OnDisable()
    {
        PlayerProgressManager.OnMissionChanged -= UpdateMission;
    }

    private void Start()
    {
        missionTextUI.text = PlayerProgressManager.CurrentMissionText;



        if (audioCtrl == null)
            audioCtrl = gameObject.AddComponent<SFXAudioController>();
    }

    private void UpdateMission(string newMission)
    {
        if (missionTextUI != null)
            missionTextUI.text = newMission;

        if (audioCtrl != null && openSound != null)
            audioCtrl.Play(openSound);
    }


    public void SetText(string text)
    {
        missionTextUI.text = text;
    }
}
