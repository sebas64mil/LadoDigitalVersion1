using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Localization.Settings;

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

    [Header("Misión Localizada al Activar el Portal")]
    public LocalizedString missionMessageKey;

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


            long id = missionMessageKey.TableEntryReference.KeyId;

            // Obtener la clave string real ("I018")
            string realKey = PlayerProgressManager.Instance.GetKeyStringFromId("Tabla1", id);


            // Enviar misión usando la key STRING real
            PlayerProgressManager.Instance.SetCurrentMission(realKey);

            // Una vez hecho el cambio, reseteamos el estado del portal
            if (portalAnimator != null)
                portalAnimator.SetBool("IsEnter", false);
        }
    }
}
