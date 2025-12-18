using UnityEngine;

public class LanguageSettingsUIConnector : MonoBehaviour
{
    public SelectorOpcion languageSelector; // 0 = Español, 1 = Inglés

    private void Start()
    {
        // Cargar el idioma guardado al abrir el menú
        languageSelector.SetIndex(LanguageSettingsManager.Instance.GetLanguage());

        // Conectar el evento del selector UI al manager
        languageSelector.OnValueChanged += (value) =>
        {
            LanguageSettingsManager.Instance.SetLanguage(value);
        };
    }
}
