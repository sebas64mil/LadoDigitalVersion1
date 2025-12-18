using UnityEngine;
using UnityEngine.Localization;

public class MissionTrigger : MonoBehaviour
{
    [Header("Misión localizada que se activará al entrar")]
    public LocalizedString missionKey;

    private bool hasTriggered = false;

    private string savedKey; // clave real usada para PlayerPrefs

    private void Start()
    {
        // Obtener el ID de la entry
        long id = missionKey.TableEntryReference.KeyId;

        // Convertirlo a la key real ("I018")
        savedKey = PlayerProgressManager.Instance.GetKeyStringFromId("Tabla1", id);

        // Si ya estaba activado antes → desactivar el trigger
        if (PlayerPrefs.GetInt(savedKey, 0) == 1)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        TryTrigger(other.CompareTag("Player"));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        TryTrigger(other.CompareTag("Player"));
    }

    private void TryTrigger(bool isPlayer)
    {
        if (!isPlayer || hasTriggered) return;

        hasTriggered = true;

        // Asignar misión usando la key STRING real
        PlayerProgressManager.Instance.SetCurrentMission(savedKey);

        // Guardar que ya fue activada una vez
        PlayerPrefs.SetInt(savedKey, 1);
        SaveMissionKey(savedKey);
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
