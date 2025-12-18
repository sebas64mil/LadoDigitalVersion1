using UnityEngine;
using TMPro;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.ResourceManagement.AsyncOperations;

public class KeyUITextController : MonoBehaviour
{
    [Header("Referencias")]
    public Animator animator;
    public TMP_Text messageText;

    [Header("Localization")]
    public LocalizedString keyMessage; // El mensaje localizado (Smart String)

    public string triggerName = "Show";

    private AsyncOperationHandle<string> loadOp;
    public static KeyUITextController Instance;

    private int lastKeyID = -1;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;
    }

    private void OnDisable()
    {
        LocalizationSettings.SelectedLocaleChanged -= OnLocaleChanged;
    }

    private void OnLocaleChanged(Locale loc)
    {
        if (lastKeyID != -1)
        {
            UpdateKeyMessageText(lastKeyID); // solo actualizar texto
        }
    }

    public void ShowKeyMessage(int keyID)
    {
        lastKeyID = keyID;

        UpdateKeyMessageText(keyID); // actualizar texto

        // Activar animación solo cuando se muestra el mensaje
        if (animator != null)
        {
            animator.ResetTrigger(triggerName);
            animator.SetTrigger(triggerName);
        }
    }


    private void UpdateKeyMessageText(int keyID)
    {
        // Pasar parámetros a la Smart String
        keyMessage.Arguments = new object[] { keyID + 1 };

        // Cancelar cargas anteriores
        if (loadOp.IsValid())
            loadOp.Completed -= OnTextLoaded;

        loadOp = keyMessage.GetLocalizedStringAsync();
        loadOp.Completed += OnTextLoaded;
    }


    private void OnTextLoaded(AsyncOperationHandle<string> op)
    {
        messageText.text = op.Result;
    }
}
