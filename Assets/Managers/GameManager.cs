using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static bool IsPaused = false;
    public static event Action OnPauseKeyPressed;

    [Header("Transición entre escenas")]
    [SerializeField] private Animator transitionAnimator;
    [SerializeField] private float transitionTime = 1f;

    public static GameManager instance;

    private bool isTransitioning = false;
    private float timer = 0f;
    private string nextScene = "";

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            OnPauseKeyPressed?.Invoke();

        // Si hay transición en curso, avanza el temporizador
        if (isTransitioning)
        {
            timer += Time.unscaledDeltaTime; // usa tiempo real, no afectado por Time.timeScale

            if (timer >= transitionTime)
            {
                SceneManager.LoadScene(nextScene);
                isTransitioning = false;
                timer = 0f;
            }
        }
    }

    public static void CursorVisible(bool state)
    {
        Cursor.visible = state;
        Cursor.lockState = state ? CursorLockMode.None : CursorLockMode.Locked;
    }

    public static void GamePause(bool state)
    {
        IsPaused = state;
        Time.timeScale = state ? 0 : 1;
    }

    // ---------------- TRANSICIÓN ----------------
    public static void LoadScene(string sceneName)
    {
        if (instance == null) return;

        instance.StartTransition(sceneName);
    }

    public static void RestartScene()
    {
        if (instance == null) return;

        string currentScene = SceneManager.GetActiveScene().name;
        instance.StartTransition(currentScene);
    }

    private void StartTransition(string sceneName)
    {
        Time.timeScale = 1;
        nextScene = sceneName;

        if (transitionAnimator != null)
            transitionAnimator.SetBool("IsChange", true);

        isTransitioning = true;
        timer = 0f;
    }

    public static void QuitGame()
    {
        SaveSystem.DeleteAllSaves();
        ResetMissionProgress();
        Application.Quit();
    }

    public static void ResetMissionProgress()
    {
        string keys = PlayerPrefs.GetString("Mission_Keys", "");

        foreach (string key in keys.Split('|'))
        {
            if (!string.IsNullOrEmpty(key))
            {
                PlayerPrefs.DeleteKey(key);
            }
        }

        PlayerPrefs.DeleteKey("Mission_Keys");
        PlayerPrefs.Save();
    }

}
