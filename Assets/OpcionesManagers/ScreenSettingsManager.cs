using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ScreenSettingsManager : MonoBehaviour
{
    [Header("Fullscreen UI")]
    public Toggle fullscreenToggle;

    [Header("Exposure Settings (URP PostProcessing)")]
    public Volume volume;
    private ColorAdjustments colorAdjustments;

    [Tooltip("Valor mínimo de exposición")]
    public float exposureMin = -2f;

    [Tooltip("Valor máximo de exposición")]
    public float exposureMax = 2f;

    public Slider exposureSlider;

    [Header("Bloom Settings (URP PostProcessing)")]
    private Bloom bloom;

    [Tooltip("Threshold mínimo del Bloom (Neón)")]
    public float bloomMin = 0f;

    [Tooltip("Threshold máximo del Bloom (Neón)")]
    public float bloomMax = 2f;

    public Slider bloomSlider;

    private void Start()
    {
        InitializeVolumeComponents();
        LoadSettings();
    }

    // ----------------------------------------
    // Inicializar componentes del Volume
    // ----------------------------------------
    private void InitializeVolumeComponents()
    {
        if (volume != null && volume.profile != null)
        {
            // Exposure
            volume.profile.TryGet(out colorAdjustments);

            // Bloom
            volume.profile.TryGet(out bloom);
        }
    }

    // ----------------------------------------
    // Métodos llamados desde los UI (Toggle/Sliders)
    // ----------------------------------------

    public void OnFullscreenToggle(bool isOn)
    {
        Screen.fullScreen = isOn;
        PlayerPrefs.SetInt("Fullscreen", isOn ? 1 : 0);
    }

    public void OnExposureChange(float sliderValue)
    {
        float exposure = Mathf.Lerp(exposureMin, exposureMax, sliderValue);

        if (colorAdjustments != null)
            colorAdjustments.postExposure.value = exposure;

        PlayerPrefs.SetFloat("ExposureSliderValue", sliderValue);
    }

    public void OnBloomThresholdChange(float sliderValue)
    {
        float threshold = Mathf.Lerp(bloomMax, bloomMin, sliderValue);

        if (bloom != null)
            bloom.threshold.value = threshold;

        PlayerPrefs.SetFloat("BloomSliderValue", sliderValue);
    }

    // ----------------------------------------
    // Cargar Configuración
    // ----------------------------------------
    private void LoadSettings()
    {
        // Fullscreen
        bool fullscreen = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
        fullscreenToggle.isOn = fullscreen;
        Screen.fullScreen = fullscreen;

        // Exposure
        float exposureValue = PlayerPrefs.GetFloat("ExposureSliderValue", 0.5f);
        exposureSlider.value = exposureValue;
        OnExposureChange(exposureValue);

        // Bloom
        float bloomValue = PlayerPrefs.GetFloat("BloomSliderValue", 0.5f);
        bloomSlider.value = bloomValue;
        OnBloomThresholdChange(bloomValue);
    }
}
