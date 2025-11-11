using UnityEngine;
using TMPro;

public class KeyUITextController : MonoBehaviour
{
    [Header("Referencias")]
    public Animator animator;              // Animator del texto
    public TMP_Text messageText;           // Texto que muestra el mensaje
    [Tooltip("Nombre del trigger que reproduce la animación en el Animator.")]
    public string triggerName = "Show";    // Trigger en el Animator (por defecto "Show")

    public static KeyUITextController Instance;

    private void Awake()
    {
        Instance = this; 
    }

    /// <param name="keyID">El ID de la tarjeta (empezando desde 0)</param>
    public void ShowKeyMessage(int keyID)
    {
        // Cambiar el texto (ID + 1 solo para mostrarlo correctamente)
        messageText.text = $"¡Has conseguido la card {keyID + 1}!";

        // Activar la animación
        if (animator != null)
        {
            animator.ResetTrigger(triggerName);
            animator.SetTrigger(triggerName);
        }
  
    }
}
