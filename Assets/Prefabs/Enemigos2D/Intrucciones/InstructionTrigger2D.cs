using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class InstructionTrigger2D : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Animator targetAnimator;    // Animator con el parámetro IsInstruc
    [SerializeField] private string playerTag = "Player"; // Tag del jugador

    [Header("Configuración inicial")]
    [SerializeField] private bool startActive = false;   // Si comienza en true, el Animator inicia con IsInstruc = true

    private bool isInside = false;


    [Header("Config Music")]
    public AudioClip openSound;
    private SFXAudioController audioCtrl;

    private void Start()
    {
        Collider2D col = GetComponent<Collider2D>();
        col.isTrigger = true;
        audioCtrl = GetComponent<SFXAudioController>();


        // Detectar si el jugador ya está dentro del trigger al iniciar
        Collider2D[] overlaps = Physics2D.OverlapBoxAll(col.bounds.center, col.bounds.size, 0f);
        foreach (Collider2D overlap in overlaps)
        {
            if (overlap.CompareTag(playerTag))
            {
                isInside = true;
                break;
            }
        }

        // Si está activo desde el inicio o el jugador está dentro, activar IsInstruc
        bool initialState = startActive || isInside;
        if (targetAnimator)
            targetAnimator.SetBool("IsInstruc", initialState);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isInside && other.CompareTag(playerTag))
        {
            isInside = true;
            audioCtrl.Play(openSound);

            if (targetAnimator)
                targetAnimator.SetBool("IsInstruc", true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (isInside && other.CompareTag(playerTag))
        {
            isInside = false;
            audioCtrl.Play(openSound);
            if (targetAnimator)
                targetAnimator.SetBool("IsInstruc", false);
        }
    }

}

