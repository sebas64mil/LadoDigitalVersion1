using UnityEngine;
using System;

public class GeneralPlayerSettingsManager : MonoBehaviour
{
    public static GeneralPlayerSettingsManager Instance { get; private set; }

    // --- PlayerPrefs Keys ---
    private const string KEY_CAMERA_SENS = "CameraSensitivity";
    private const string KEY_CROUCH = "CrouchMode";
    private const string KEY_CLIMB = "ClimbMode";
    private const string KEY_RUN = "RunMode";

    // --- Valores de sensibilidad ---
    public float defaultCameraSensitivity = 100f;
    public float minCameraSensitivity = 30f;
    public float maxCameraSensitivity = 200f;


    // Valor actual
    private float cameraSensitivity;
    private int crouchMode;
    private int climbMode;
    private int runMode;
    public float CameraSensitivity => cameraSensitivity;

    // Evento para notificar cambios
    public event Action<float> OnCameraSensitivityChanged;

    private void Awake()
    {
        Instance = this;
        LoadSettings();
    }

    void LoadSettings()
    {
        cameraSensitivity = PlayerPrefs.GetFloat(KEY_CAMERA_SENS, defaultCameraSensitivity);
        cameraSensitivity = Mathf.Clamp(cameraSensitivity, minCameraSensitivity, maxCameraSensitivity);
    }

    public void SetCameraSensitivity(float value, bool save = true)
    {
        float clamped = Mathf.Clamp(value, minCameraSensitivity, maxCameraSensitivity);

        if (Mathf.Approximately(clamped, cameraSensitivity))
            return;

        cameraSensitivity = clamped;

        if (save)
            PlayerPrefs.SetFloat(KEY_CAMERA_SENS, cameraSensitivity);

        OnCameraSensitivityChanged?.Invoke(cameraSensitivity);
    }

    // Para forzar actualización al abrir menú o cargar nivel
    public void ApplyCurrentSettings()
    {
        OnCameraSensitivityChanged?.Invoke(cameraSensitivity);
    }

    public int GetCrouchMode() => PlayerPrefs.GetInt(KEY_CROUCH, 0);
    public int GetClimbMode() => PlayerPrefs.GetInt(KEY_CLIMB, 0);
    public int GetRunMode() => PlayerPrefs.GetInt(KEY_RUN, 0);

    public void SetCrouchMode(int v) => PlayerPrefs.SetInt(KEY_CROUCH, v);
    public void SetClimbMode(int v) => PlayerPrefs.SetInt(KEY_CLIMB, v);
    public void SetRunMode(int v) => PlayerPrefs.SetInt(KEY_RUN, v);
}
