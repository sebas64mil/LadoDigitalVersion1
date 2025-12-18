using UnityEngine;
using UnityEngine.Localization.Settings;
using System;
using UnityEngine.ResourceManagement.AsyncOperations;

public class LanguageSettingsManager : MonoBehaviour
{
    public static LanguageSettingsManager Instance { get; private set; }

    private const string KEY_LANGUAGE = "GameLanguage";
    // 0 = Español, 1 = Inglés

    public event Action<int> OnLanguageChanged;

    private void Awake()
    {
        Instance = this;
        ApplySavedLanguage();
    }

    /// Cargar idioma guardado e iniciar sistema
    private async void ApplySavedLanguage()
    {
        int index = PlayerPrefs.GetInt(KEY_LANGUAGE, 0); // default español

        // Obtener la lista de Locales disponibles
        var locales = LocalizationSettings.AvailableLocales.Locales;
        if (index < 0 || index >= locales.Count)
            index = 0;

        // Seleccionar el Locale correspondiente
        var handle = LocalizationSettings.InitializationOperation;
        await handle.Task;
        LocalizationSettings.SelectedLocale = locales[index];

        OnLanguageChanged?.Invoke(index);
    }

    /// Cambiar idioma desde UI
    public async void SetLanguage(int index)
    {
        // Guardar PlayerPrefs
        PlayerPrefs.SetInt(KEY_LANGUAGE, index);
        PlayerPrefs.Save();

        // Obtener la lista de Locales disponibles
        var locales = LocalizationSettings.AvailableLocales.Locales;
        if (index < 0 || index >= locales.Count)
            index = 0;

        // Seleccionar el Locale correspondiente
        var handle = LocalizationSettings.InitializationOperation;
        await handle.Task;
        LocalizationSettings.SelectedLocale = locales[index];

        // Notificar
        OnLanguageChanged?.Invoke(index);
    }

    public int GetLanguage() => PlayerPrefs.GetInt(KEY_LANGUAGE, 0);
}

