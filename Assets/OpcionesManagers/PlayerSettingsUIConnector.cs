using UnityEngine;

public class PlayerSettingsUIConnector : MonoBehaviour
{
    public SelectorOpcion crouchSelector;
    public SelectorOpcion climbSelector;
    public SelectorOpcion runSelector;

    private void Start()
    {
        // Cargar valores guardados
        crouchSelector.SetIndex(GeneralPlayerSettingsManager.Instance.GetCrouchMode());
        climbSelector.SetIndex(GeneralPlayerSettingsManager.Instance.GetClimbMode());
        runSelector.SetIndex(GeneralPlayerSettingsManager.Instance.GetRunMode());

        // ✅ Conectar eventos
        crouchSelector.OnValueChanged += (value) =>
        {
            GeneralPlayerSettingsManager.Instance.SetCrouchMode(value);
        };

        climbSelector.OnValueChanged += (value) =>
        {
            GeneralPlayerSettingsManager.Instance.SetClimbMode(value);
        };

        runSelector.OnValueChanged += (value) =>
        {
            GeneralPlayerSettingsManager.Instance.SetRunMode(value);
        };
    }
}
