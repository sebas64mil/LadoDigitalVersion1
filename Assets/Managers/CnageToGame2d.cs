using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Localization.Settings;


public class CnageToGame2d : MonoBehaviour
{
    [Header("Referencia al Manager")]
    public ManagerTransition managerTransition;

    [Tooltip("Posición a la que aparecerá el jugador en 2D")]
    public Vector3 targetPosition2D;

    [Header("Referencia al Texto")]
    public TextMeshProUGUI messageText;

    [Header("Mensajes Localizados")]
    public LocalizedString enterMessageKey;  //  REEMPLAZA enterMessage
    public LocalizedString exitMessageKey;   //  REEMPLAZA exitMessage

    private AsyncOperationHandle<string> loadOp;

    private bool isPlayerInside = false;

    [Header("Config Music")]
    public AudioClip openSound;
    private SFXAudioController audioCtrl;

    [Header("Misión Localizada al Activar el Portal")]
    public LocalizedString missionMessageKey;


    [Header("Referencia de Cámara y Confiner (opcional)")]
    public CinemachineCamera virtualCamera2D;
    public Collider2D confinerBounds2D;

    void Start()
    {
        audioCtrl = GetComponent<SFXAudioController>();

        LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
            CargarMensaje(enterMessageKey);   // 🔥 Mensaje localizado
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
            CargarMensaje(exitMessageKey);    // 🔥 Mensaje localizado
        }
    }

    private void Update()
    {
        if (isPlayerInside && Input.GetKeyDown(KeyCode.E))
        {
            managerTransition.ChangeTo2D(targetPosition2D);
            TryAssignConfiner2D();
            audioCtrl.Play(openSound);

            isPlayerInside = false;
            CargarMensaje(exitMessageKey);


            long id = missionMessageKey.TableEntryReference.KeyId;

            // Obtener la clave string real ("I018")
            string realKey = PlayerProgressManager.Instance.GetKeyStringFromId("Tabla1", id);


            // Enviar misión usando la key STRING real
            PlayerProgressManager.Instance.SetCurrentMission(realKey);
        }
    }

    private void OnLocaleChanged(UnityEngine.Localization.Locale newLocale)
    {
        if (isPlayerInside)
            CargarMensaje(enterMessageKey);
        else
            CargarMensaje(exitMessageKey);
    }

    private void OnDestroy()
    {
        LocalizationSettings.SelectedLocaleChanged -= OnLocaleChanged;
    }


    private void CargarMensaje(LocalizedString key)
    {
        if (messageText == null || key == null)
            return;

        // Cancelar carga previa
        if (loadOp.IsValid())
            loadOp.Completed -= OnMessageLoaded;

        loadOp = key.GetLocalizedStringAsync();
        loadOp.Completed += OnMessageLoaded;
    }

    private void OnMessageLoaded(AsyncOperationHandle<string> op)
    {
        messageText.text = op.Result;
    }

    private void TryAssignConfiner2D()
    {
        if (virtualCamera2D == null)
        {
            Debug.LogWarning(" No hay cámara 2D asignada en CnageToGame2d.");
            return;
        }

        var confiner = virtualCamera2D.GetComponent<CinemachineConfiner2D>();
        if (confiner == null)
        {
            Debug.LogWarning(" La cámara 2D no tiene un CinemachineConfiner2D.");
            return;
        }

        if (confinerBounds2D != null)
        {
            confiner.BoundingShape2D = confinerBounds2D;
            confiner.InvalidateBoundingShapeCache();
        }
    }
}
