using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTimer : MonoBehaviour
{
    public static LevelTimer Instance;

    private float currentTime = 0f;

    private const string BEST_TIME_KEY = "Level1_BestTime";
    private const string CURRENT_TIME_KEY = "Level1_CurrentTime";

    private string targetLevelName = "Nivel1";

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;

       // Debug.Log("⏱️ LevelTimer inicializado y persistente.");
    }

    void Start()
    {
        // ⭐ Cargar el tiempo guardado al abrir el juego
        currentTime = PlayerPrefs.GetFloat(CURRENT_TIME_KEY, 0f);
      //  Debug.Log("🔄 Tiempo cargado al iniciar: " + currentTime);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == targetLevelName)
        {
       //     Debug.Log("🎯 Entramos al nivel → continuando tiempo cargado...");
            // NO restablecemos aquí, porque el usuario quiere que NO se reinicie
        }
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name != targetLevelName)
            return;

        if (GameManager.IsPaused)
            return;

        currentTime += Time.deltaTime;

      //  Debug.Log($"⏱️ Tiempo actual: {FormatTime(currentTime)}");
    }

    void OnApplicationQuit()
    {
        // ⭐ Guardar tiempo actual al cerrar juego
        PlayerPrefs.SetFloat(CURRENT_TIME_KEY, currentTime);
        PlayerPrefs.Save();

      //  Debug.Log("💾 Tiempo guardado al cerrar: " + currentTime);
    }

    public float GetCurrentTime() => currentTime;

    public string GetFormattedCurrentTime() => FormatTime(currentTime);

    public void SaveBestTime()
    {
        float best = PlayerPrefs.GetFloat(BEST_TIME_KEY, float.MaxValue);

        if (currentTime < best)
        {
            PlayerPrefs.SetFloat(BEST_TIME_KEY, currentTime);
            PlayerPrefs.Save();
        }
    }

    public static string GetFormattedBestTime()
    {
        float best = PlayerPrefs.GetFloat(BEST_TIME_KEY, float.MaxValue);

        return best == float.MaxValue ? "00:00" : FormatTime(best);
    }
    public void ResetTime()
    {
        currentTime = 0f;
        PlayerPrefs.SetFloat(CURRENT_TIME_KEY, 0f);
        PlayerPrefs.Save();

       // Debug.Log("🔁 Tiempo reiniciado tras completar el nivel.");
    }

    public static string FormatTime(float t)
    {
        int minutes = Mathf.FloorToInt(t / 60f);
        int seconds = Mathf.FloorToInt(t % 60f);

        return $"{minutes:00}:{seconds:00}";
    }
}
