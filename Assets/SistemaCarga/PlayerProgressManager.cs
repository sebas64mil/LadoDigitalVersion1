using System;
using System.Linq;
using UnityEngine;

public class PlayerProgressManager : MonoBehaviour
{
    public ManagerTransition managerTransition;

    [HideInInspector] public DefaultSceneData currentData;
    [HideInInspector] public bool[] keysCollected;


    public static event Action<int, int> OnKeyCollected; // (doorID, keyIndex)
    public static event Action OnGameSaved;

    public static string CurrentMissionText = "Sal de la habitacion"; // <- misión accesible global
    public static event Action<string> OnMissionChanged; // <- evento


    public static PlayerProgressManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        currentData = SaveSystem.Load();

        if (!string.IsNullOrEmpty(currentData.currentMission))
        {
            CurrentMissionText = currentData.currentMission;
            OnMissionChanged?.Invoke(CurrentMissionText); // para actualizar la UI
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


    public void SetCurrentMission(string newMission)
    {
        currentData.currentMission = newMission;
        CurrentMissionText = newMission;

        SaveSystem.Save(currentData);
        OnGameSaved?.Invoke();

        OnMissionChanged?.Invoke(newMission);
    }

    public string GetCurrentMission()
    {
        return CurrentMissionText;
    }


}
