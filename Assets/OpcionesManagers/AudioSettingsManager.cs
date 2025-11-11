using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettingsManager : MonoBehaviour
{
    [Header("Audio Mixer")]
    public AudioMixer audioMixer;

    [Header("Sliders UI")]
    public Slider masterSlider;
    public Slider ambientSlider;
    public Slider sfxSlider;

    private void Start()
    {
        LoadVolumeSettings();
    }

    // --- Métodos llamados desde los Sliders ---
    public void OnMasterVolumeChange(float value)
    {
        SetVolume("MasterVolume", value);
        PlayerPrefs.SetFloat("MasterVolume", value);
    }

    public void OnAmbientVolumeChange(float value)
    {
        SetVolume("AmbientVolume", value);
        PlayerPrefs.SetFloat("AmbientVolume", value);
    }

    public void OnSFXVolumeChange(float value)
    {
        SetVolume("SFXVolume", value);
        PlayerPrefs.SetFloat("SFXVolume", value);
    }

    // --- Aplicar volumen en decibeles ---
    private void SetVolume(string parameter, float value)
    {
        // Convertir slider (0–1) a decibeles (-80 a 0)
        float dB = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f;
        audioMixer.SetFloat(parameter, dB);
    }

    // --- Cargar configuración guardada ---
    private void LoadVolumeSettings()
    {
        float master = PlayerPrefs.GetFloat("MasterVolume", 1f);
        float ambient = PlayerPrefs.GetFloat("AmbientVolume", 1f);
        float sfx = PlayerPrefs.GetFloat("SFXVolume", 1f);

        masterSlider.value = master;
        ambientSlider.value = ambient;
        sfxSlider.value = sfx;

        SetVolume("MasterVolume", master);
        SetVolume("AmbientVolume", ambient);
        SetVolume("SFXVolume", sfx);
    }
}
