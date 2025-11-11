using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject PauseMenu;
    public GameObject OptionMenu;
    private bool isPaused = false;

    private void OnEnable()
    {
        GameManager.OnPauseKeyPressed += TogglePauseMenu;
    }

    private void OnDisable()
    {
        GameManager.OnPauseKeyPressed -= TogglePauseMenu;
    }

    private void Start()
    {
        PauseMenu.SetActive(false);
        GameManager.GamePause(false);
        GameManager.CursorVisible(false);
    }

    private void TogglePauseMenu()
    {
        isPaused = !isPaused;
        PauseMenuVisible(isPaused);

        if (LevelMusicManager.Instance != null)
        {
            if (isPaused) LevelMusicManager.Instance.FadeToPause();
            else LevelMusicManager.Instance.FadeToNormal();
        }

        if (!isPaused) { 
        
        OptionMenu.SetActive(false);
        }
    }

    public void PauseMenuVisible(bool state)
    {
        PauseMenu.SetActive(state);
        GameManager.GamePause(state);
        GameManager.CursorVisible(state);

        if (LevelMusicManager.Instance != null)
        {
            if (state) LevelMusicManager.Instance.FadeToPause();
            else LevelMusicManager.Instance.FadeToNormal();
        }
    }
}
