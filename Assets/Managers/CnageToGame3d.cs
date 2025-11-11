using UnityEngine;
using TMPro;
public class ChangeToGame3D : MonoBehaviour
{
    [Header("Referencia al Manager")]
    public ManagerTransition managerTransition;

    [Tooltip("Posición a la que aparecerá el jugador en 3D")]
    public Vector3 targetPosition3D;

    [Header("Animator del portal")]
    public Animator portalAnimator;

    private bool isPlayerInside = false;


    [Header("Config Music")]
    public AudioClip openSound;
    private SFXAudioController audioCtrl;

    [Tooltip("Mensaje que se mostrará como misión actual al usar el portal")]
    [TextArea(2, 3)]
    public string missionMessage;

    void Start()
    {
        audioCtrl = GetComponent<SFXAudioController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInside = true;
            if (portalAnimator != null)
                portalAnimator.SetBool("IsEnter", true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInside = false;
            if (portalAnimator != null)
                portalAnimator.SetBool("IsEnter", false);
        }
    }

    private void Update()
    {
        if (isPlayerInside && Input.GetKeyDown(KeyCode.E))
        {
            managerTransition.ChangeTo3D(targetPosition3D);
            audioCtrl.Play(openSound);
            isPlayerInside = false;
            PlayerProgressManager.Instance.SetCurrentMission(missionMessage);

            // Una vez hecho el cambio, reseteamos el estado del portal
            if (portalAnimator != null)
                portalAnimator.SetBool("IsEnter", false);
        }
    }
}
