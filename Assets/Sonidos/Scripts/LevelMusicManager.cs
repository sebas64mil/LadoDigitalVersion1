using UnityEngine;
using System.Collections;

public class LevelMusicManager : MonoBehaviour
{
    public static LevelMusicManager Instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource source2D;
    [SerializeField] private AudioSource source3D;

    [Header("Volumenes")]
    public float normalVolume = 1f;
    public float pausedVolume = 0.3f;
    public float fadeSpeed = 2f;

    private bool isPaused = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        // Inicializar ambos AudioSources
        source2D.volume = 0f;
        source3D.volume = 0f;

        source2D.loop = true;
        source3D.loop = true;

        source2D.Play();
        source3D.Play();

        // Revisar datos del guardado después de un frame
        StartCoroutine(InitializeMusic());
    }

    private IEnumerator InitializeMusic()
    {
        yield return null;

        DefaultSceneData data = SaveSystem.Load();

        if (data != null)
        {
            if (data.is3D)
                SetMode3DImmediate();
            else
                SetMode2DImmediate();
        }
        else
        {
            SetMode3DImmediate();
        }
    }

    // --- Cambiar música según dimensión ---
    public void SwitchTo3D()
    {
        StartCoroutine(Crossfade(source2D, source3D));
    }

    public void SwitchTo2D()
    {
        StartCoroutine(Crossfade(source3D, source2D));
    }

    private IEnumerator Crossfade(AudioSource from, AudioSource to)
    {
        while (from.volume > 0.01f || to.volume < normalVolume)
        {
            from.volume = Mathf.MoveTowards(from.volume, 0f, Time.deltaTime * fadeSpeed);
            to.volume = Mathf.MoveTowards(to.volume, normalVolume, Time.deltaTime * fadeSpeed);
            yield return null;
        }
    }

    // --- Cambiar instantáneamente sin transición (para carga inicial) ---
    private void SetMode3DImmediate()
    {
        source2D.volume = 0f;
        source3D.volume = normalVolume;
    }

    private void SetMode2DImmediate()
    {
        source3D.volume = 0f;
        source2D.volume = normalVolume;
    }

    // --- Métodos llamados desde Pause/Resume ---
    public void FadeToPause()
    {
        isPaused = true;
        SetPausedVolume();
    }

    public void FadeToNormal()
    {
        isPaused = false;
        SetPausedVolume();
    }

    private void SetPausedVolume()
    {
        float target = isPaused ? pausedVolume : normalVolume;

        if (source3D.volume > 0f)
            source3D.volume = target;

        if (source2D.volume > 0f)
            source2D.volume = target;
    }
}
