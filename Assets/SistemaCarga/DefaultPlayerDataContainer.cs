using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DefaultSceneDataContainer
{
    public List<DefaultSceneData> scenesDefault = new List<DefaultSceneData>();

    public DefaultSceneDataContainer()
    {
        // Nivel 1
        var nivel1 = new DefaultSceneData("Nivel1")
        {
            playerPosition3D = new Vector3(-411.566f, 1.38f, -90.434f),
            playerRotation3D = new Vector3(0, -38.1f, 0),
            playerPosition2D = new Vector3(-36f, 0.87f, 0),
            is3D = true
        };

        nivel1.doors.Add(new DoorData(0, 1));

        nivel1.doors.Add(new DoorData(1, 2));

        nivel1.doors.Add(new DoorData(2, 3));

        nivel1.currentMission = "Sal de la habitacion";


        scenesDefault.Add(nivel1);
    }

    public DefaultSceneData GetDefaultForScene(string sceneName)
    {
        return scenesDefault.Find(s => s.sceneName == sceneName);
    }
}
