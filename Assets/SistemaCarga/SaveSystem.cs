using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SaveSystem
{
    private static string GetFilePath()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        return Path.Combine(Application.persistentDataPath, sceneName + "_playerData.json");
    }

    public static void Save(DefaultSceneData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(GetFilePath(), json);
    }

    public static DefaultSceneData Load()
    {
        string path = GetFilePath();

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            DefaultSceneData data = JsonUtility.FromJson<DefaultSceneData>(json);
            return data;
        }
        else
        {
            string sceneName = SceneManager.GetActiveScene().name;
            DefaultSceneDataContainer defaults = new DefaultSceneDataContainer();
            DefaultSceneData defaultData = defaults.GetDefaultForScene(sceneName);

            if (defaultData != null)
            {
                return defaultData;
            }

            return new DefaultSceneData(sceneName);
        }
    }

    public static void Delete()
    {
        string path = GetFilePath();
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }

    public static void DeleteAllSaves()
    {
        string folderPath = Application.persistentDataPath;
        string[] files = Directory.GetFiles(folderPath, "*.json");

        foreach (string file in files)
        {
            File.Delete(file);
        }
    }
}

