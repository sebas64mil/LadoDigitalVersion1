using UnityEngine;
using TMPro;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.ResourceManagement.AsyncOperations;

public class NoteInteractionToggle : MonoBehaviour
{
    [Header("UI References")]
    public GameObject interactIcon;
    public GameObject notePanel;
    public TMP_Text noteText;

    [Header("Note Content (Localization Key)")]
    public LocalizedString noteKey;   //  antes era string, ahora es LocalizedString

    private AsyncOperationHandle<string> loadOp;

    private bool playerInRange = false;
    private bool noteOpen = false;

    [Header("Sonidos")]
    public AudioClip openSound;
    private SFXAudioController audioCtrl;

    void Start()
    {
        interactIcon.SetActive(false);
        notePanel.SetActive(false);

        if (audioCtrl == null)
            audioCtrl = gameObject.AddComponent<SFXAudioController>();

        //  Cargar texto inicial
        LoadLocalizedText();

        //  Escuchar cambios de idioma
        LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;
    }

    private void OnDestroy()
    {
        LocalizationSettings.SelectedLocaleChanged -= OnLocaleChanged;
    }

    private void OnLocaleChanged(Locale obj)
    {
        if (noteOpen)   // si la nota está abierta, se actualiza en vivo
            LoadLocalizedText();
    }

    void Update()
    {
        if (!playerInRange) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!noteOpen)
                OpenNote();
            else
                CloseNote();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseNote();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            interactIcon.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            CloseNote();
            interactIcon.SetActive(false);
        }
    }

    void OpenNote()
    {
        noteOpen = true;
        notePanel.SetActive(true);

        LoadLocalizedText(); //  cargar texto localizado

        interactIcon.SetActive(false);
        audioCtrl.Play(openSound);
    }

    void CloseNote()
    {
        noteOpen = false;
        notePanel.SetActive(false);
        audioCtrl.Play(openSound);

        if (playerInRange)
            interactIcon.SetActive(true);
    }

    //  Carga segura del texto localizado
    private void LoadLocalizedText()
    {
        if (loadOp.IsValid())
            loadOp.Completed -= OnTextLoaded;

        loadOp = noteKey.GetLocalizedStringAsync();
        loadOp.Completed += OnTextLoaded;
    }

    private void OnTextLoaded(AsyncOperationHandle<string> op)
    {
        noteText.text = op.Result;
    }
}
