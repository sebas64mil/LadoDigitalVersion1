using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuUI : MonoBehaviour
{
    public Slider sensitivitySlider;


    void Start()
    {
        float saved = GeneralPlayerSettingsManager.Instance.CameraSensitivity;

        // Convertir de rango 30–200 al rango del slider 0.1–1
        float sliderValue = Mathf.InverseLerp(10f, 200f, saved);

        sensitivitySlider.value = sliderValue;
    }


    public void OnSensitivitySliderChanged(float value)
    {
        float adjusted = Mathf.Lerp(10f, 200f, value);
        GeneralPlayerSettingsManager.Instance.SetCameraSensitivity(adjusted);
    }

}
