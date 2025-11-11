using UnityEngine;

public class MissionTrigger : MonoBehaviour
{
    [SerializeField] private string missionText;
    private bool hasTriggered = false;

    private string missionKey;

    private void Start()
    {
        missionKey = "Mission_" + missionText;

        // Si ya estaba completada antes, no activamos el trigger
        if (PlayerPrefs.GetInt(missionKey, 0) == 1)
        {
            gameObject.SetActive(false);
        }
    }

    // Trigger 3D
    private void OnTriggerEnter(Collider other)
    {
        TryTrigger(other.CompareTag("Player"));
    }

    // Trigger 2D
    private void OnTriggerEnter2D(Collider2D other)
    {
        TryTrigger(other.CompareTag("Player"));
    }

    private void TryTrigger(bool isPlayer)
    {
        if (!isPlayer || hasTriggered) return;

        PlayerProgressManager.Instance.SetCurrentMission(missionText);
        hasTriggered = true;

        // Guardar como completada
        PlayerPrefs.SetInt(missionKey, 1);
        SaveMissionKey(missionKey);

        PlayerPrefs.Save();
        gameObject.SetActive(false);
    }

    private void SaveMissionKey(string key)
    {
        string keys = PlayerPrefs.GetString("Mission_Keys", "");

        if (!keys.Contains(key))
        {
            keys += key + "|";
            PlayerPrefs.SetString("Mission_Keys", keys);
        }
    }
}
