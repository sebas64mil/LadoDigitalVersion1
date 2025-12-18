using UnityEngine;
using TMPro;
using System.Linq;

public class ProgressUiPlayer : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text progressText;
    public string sceneNameToLoad = "Level1";

    private DefaultSceneData data;

    void Start()
    {
        if (string.IsNullOrEmpty(sceneNameToLoad))
        {
            Debug.LogError("❌ Debes asignar un nombre de escena en sceneNameToLoad.");
            progressText.text = "0%";
            return;
        }

        data = SaveSystem.Load(sceneNameToLoad);

        if (data == null)
        {
            Debug.LogError("❌ SaveSystem.Load devolvió NULL para la escena: " + sceneNameToLoad);
            progressText.text = "0%";
            return;
        }

        UpdateProgressUI();
    }


    // 🔹 Equivalente a GetKeysForDoor, pero sin depender de otros scripts
    private bool[] GetKeysForDoor(int doorID)
    {
        DoorData door = data.doors.FirstOrDefault(d => d.doorID == doorID);
        return door != null ? door.keys : null;
    }

    public void UpdateProgressUI()
    {
        // ⭐ Si alguna vez se completó → siempre 100%
        if (PlayerPrefs.GetInt("Level1Completed", 0) == 1)
        {
            progressText.text = "100%";
            return;
        }

        if (data.doors == null)
        {
            progressText.text = "0%";
            Debug.Log("❌ No hay datos de puertas en DefaultSceneData.");
            return;
        }

        // ⭐ Si ya está completado en este save
        if (data.levelCompleted)
        {
            progressText.text = "100%";
            return;
        }

        int totalKeys = 0;
        int activatedKeys = 0;

        foreach (DoorData door in data.doors)
        {
            if (door == null || door.keys == null) continue;

            bool[] keys = GetKeysForDoor(door.doorID);
            if (keys == null) continue;

            totalKeys += keys.Length;
            activatedKeys += keys.Count(k => k);
        }

        float percent = (totalKeys == 0) ? 0 :
                        (float)activatedKeys / totalKeys * 100f;

        int percentInt = Mathf.RoundToInt(percent);

        // ⭐ Capar el 100% si no se ha completado el nivel
        if (percentInt >= 100)
            percentInt = 96;

        progressText.text = percentInt + "%";

    }

    public void RefreshUI()
    {
        data = SaveSystem.Load(sceneNameToLoad);
        UpdateProgressUI();
    }


}
