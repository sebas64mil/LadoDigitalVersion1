using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class DoorData
{
    public int doorID;        // Identificador único de la puerta
    public bool[] keys;       // Arreglo de llaves (true = obtenida / usada, false = no)

    public DoorData(int id, int keyCount)
    {
        doorID = id;
        keys = new bool[keyCount];
    }
}


[System.Serializable]

public class DefaultSceneData
{
    public string sceneName;          // Nombre de la escena
    public bool is3D;                 // Estado actual (2D o 3D)

    public Vector3 playerPosition3D;  // Posición si está en 3D
    public Vector3 playerRotation3D;  // Rotación si está en 3D
    public Vector3 playerPosition2D;  // Posición si está en 2D

    public List<DoorData> doors;

    public string currentMission;

    public DefaultSceneData(string name)
    {
        sceneName = name;
        is3D = true;
        playerPosition3D = Vector3.zero;
        playerRotation3D = Vector3.zero;
        playerPosition2D = Vector3.zero;
        doors = new List<DoorData>();

        currentMission = "Sal de la habitacion";
    }

    public void AddDoor(int id, int numKeys)
    {
        doors.Add(new DoorData(id, numKeys));
    }

}

