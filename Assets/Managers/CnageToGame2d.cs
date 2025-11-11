using TMPro; //  Necesario para usar TextMeshPro
using Unity.Cinemachine;
using UnityEngine;

public class CnageToGame2d : MonoBehaviour
{
    [Header("Referencia al Manager")]
    public ManagerTransition managerTransition;

    [Tooltip("Posición a la que aparecerá el jugador en 2D")]
    public Vector3 targetPosition2D;

    [Header("Referencia al Texto")]
    public TextMeshProUGUI messageText;

    [Tooltip("Mensaje que aparece al entrar al trigger")]
    public string enterMessage = "Presiona E para cambiar a 2D";

    [Tooltip("Mensaje que aparece al salir del trigger")]
    public string exitMessage = "";

    private bool isPlayerInside = false;

    [Header("Config Music")]
    public AudioClip openSound;
    private SFXAudioController audioCtrl;


    [Tooltip("Mensaje que se mostrará como misión actual al usar el portal")]
    [TextArea(2, 3)]
    public string missionMessage;


    [Header("Referencia de Cámara y Confiner (opcional)")]
    [Tooltip("Referencia a la cámara virtual usada en modo 2D")]
    public CinemachineCamera virtualCamera2D;

    [Tooltip("Collider2D que define los límites de la zona 2D")]
    public Collider2D confinerBounds2D;



    void Start()
    {
        audioCtrl = GetComponent<SFXAudioController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
            messageText.text = enterMessage;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
            messageText.text = exitMessage;
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
            messageText.text = exitMessage;
            PlayerProgressManager.Instance.SetCurrentMission(missionMessage);
        }
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
