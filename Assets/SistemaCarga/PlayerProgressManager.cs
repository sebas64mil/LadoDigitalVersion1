using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class PlayerProgressManager : MonoBehaviour
{
    public ManagerTransition managerTransition;

    [HideInInspector] public DefaultSceneData currentData;
    [HideInInspector] public bool[] keysCollected;


    public static event Action<int, int> OnKeyCollected; // (doorID, keyIndex)
    public static event Action OnGameSaved;

    public static LocalizedString CurrentMission;
    public static event Action<string> OnMissionChanged; // <- evento


    public static PlayerProgressManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        currentData = SaveSystem.Load();

        if (!string.IsNullOrEmpty(currentData.currentMissionKey))
        {
            CurrentMission = new LocalizedString("Tabla1", currentData.currentMissionKey);
            OnMissionChanged?.Invoke(CurrentMission.GetLocalizedString());
        }


        if (currentData.doors == null || currentData.doors.Count == 0)
        {
            var defaultData = new DefaultSceneDataContainer().GetDefaultForScene(currentData.sceneName);
            currentData.doors = defaultData.doors;
        }



        // Colocar jugador según su modo guardado
        if (currentData.is3D)
            managerTransition.posicion3d(currentData.playerPosition3D, currentData.playerRotation3D);
        else
            managerTransition.posicion2d(currentData.playerPosition2D);


        LocalizationSettings.SelectedLocaleChanged += OnLanguageChanged;

    }

    private void OnLanguageChanged(UnityEngine.Localization.Locale locale)
    {
        RefreshMissionText();
    }

    private void RefreshMissionText()
    {
        if (CurrentMission == null) return;

        CurrentMission.GetLocalizedStringAsync().Completed += op =>
        {
            OnMissionChanged?.Invoke(op.Result);
        };
    }


    public void ReachCheckpoint(Vector3 newPos3D, Vector3 newRot3D, Vector3 newPos2D, bool is3D)
    {
        currentData.is3D = is3D;

        

        if (is3D)
        {
            currentData.playerPosition3D = newPos3D;
            currentData.playerRotation3D = newRot3D;
        }
        else
        {
            currentData.playerPosition2D = newPos2D;
        }

        SaveSystem.Save(currentData);
        OnGameSaved?.Invoke();
    }

    public void SetIs3D(bool is3D)
    {
        if (currentData != null)
        {
            currentData.is3D = is3D;
            SaveSystem.Save(currentData);
            OnGameSaved?.Invoke();
        }
    }

    public void SavePosition3D(Vector3 pos)
    {
        currentData.playerPosition3D = pos;
        SaveSystem.Save(currentData);
        OnGameSaved?.Invoke();
    }

    public void SavePosition2D(Vector3 pos)
    {
        currentData.playerPosition2D = pos;
        SaveSystem.Save(currentData);
        OnGameSaved?.Invoke();
    }


    // 🔹 Obtener las llaves de una puerta
    public bool[] GetKeysForDoor(int doorID)
    {
        DoorData door = currentData.doors.FirstOrDefault(d => d.doorID == doorID);
        return door != null ? door.keys : null;
    }

    // 🔹 Cambiar el estado de una llave específica
    public void SetKeyState(int doorID, int keyIndex, bool state)
    {
        DoorData door = currentData.doors.FirstOrDefault(d => d.doorID == doorID);

        // Si no existe la puerta, crearla
        if (door == null)
        {
            door = new DoorData(doorID, keyIndex + 1);
            currentData.doors.Add(door);
        }

        // Si el arreglo es muy corto, agrandarlo
        if (keyIndex >= door.keys.Length)
        {
            bool[] newKeys = new bool[keyIndex + 1];
            door.keys.CopyTo(newKeys, 0);
            door.keys = newKeys;
        }

        // Actualizar valor
        door.keys[keyIndex] = state;

        // Guardar progreso actualizado
        SaveSystem.Save(currentData);
        OnGameSaved?.Invoke();

        //  Disparar el evento solo si la llave pasa a estar activada
        if (state)
        {
            OnKeyCollected?.Invoke(doorID, keyIndex);
        }
    }

    // 🔹 Verificar si todas las llaves de una puerta están activadas
    public bool IsDoorUnlocked(int doorID)
    {
        DoorData door = currentData.doors.FirstOrDefault(d => d.doorID == doorID);

        if (door == null)
            return false;

        bool allTrue = door.keys.All(k => k);
        return allTrue;
    }


    public void SetCurrentMission(string missionKey)
    {
        currentData.currentMissionKey = missionKey;

        CurrentMission = new LocalizedString("Tabla1", missionKey);

        SaveSystem.Save(currentData);
        OnGameSaved?.Invoke();

        // Extraer texto localizado pero sin romper async
        CurrentMission.GetLocalizedStringAsync().Completed += op =>
        {
            OnMissionChanged?.Invoke(op.Result);
        };
    }


    public void GetCurrentMissionAsync(Action<string> callback)
    {
        CurrentMission.GetLocalizedStringAsync().Completed += op =>
        {
            callback(op.Result);
        };
    }

    public string GetKeyStringFromId(string tableName, long keyId)
    {
        var table = UnityEngine.Localization.Settings.LocalizationSettings
            .StringDatabase
            .GetTable(tableName);

        if (table == null)
        {
            Debug.LogError($"❌ Tabla no encontrada: {tableName}");
            return null;
        }

        var entry = table.GetEntry(keyId);

        if (entry == null)
        {
            Debug.LogError($"❌ No se encontró ninguna key con ID: {keyId}");
            return null;
        }

        return entry.Key;  // <-- esto devuelve "I018"
    }


}
