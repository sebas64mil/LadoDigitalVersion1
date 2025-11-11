using UnityEngine;
using System.Collections;

public class DoorController : MonoBehaviour
{
    [Header("Referencias")]
    public PlayerProgressManager progressManager;
    public Transform teleportTarget;

    [Header("Configuración de puerta")]
    public int doorID;
    public KeyController[] keys;

    [Tooltip("Si está activado, al usar la puerta se cambiará de escena en lugar de teletransportar al jugador.")]
    public bool goToMenu = false;

    [Tooltip("Nombre de la escena a cargar si 'goToMenu' está activado.")]
    public string targetSceneName = "MainMenu";

    [Header("Animación")]
    public Animator doorAnimator;
    public string openTriggerName = "OpenDoor";
    public float animationDelay = 1f; //  tiempo para esperar tras abrir la puerta

    [Header("UI")]
    public GameObject interactionUI;


    [Header("Sonidos Config")]
    public AudioClip GoodSound;
    public AudioClip wrongSound;
    private SFXAudioController audioCtrl;

    public string missionText = "Nueva misión asignada";

    private bool isPlayerInside = false;
    private bool hasOpened = false; //  evita que se repita

    private void Start()
    {
        if (interactionUI != null)
            interactionUI.SetActive(false);

        if (audioCtrl == null)
            audioCtrl = gameObject.AddComponent<SFXAudioController>();

        bool[] keyArray = progressManager.GetKeysForDoor(doorID);

        // 🔑 Desactivar llaves ya recolectadas
        foreach (KeyController key in keys)
        {
            if (keyArray != null && key.keyIndex < keyArray.Length && keyArray[key.keyIndex])
            {
                key.gameObject.SetActive(false);
            }
        }
    }

    private void Update()
    {
        if (isPlayerInside && Input.GetKeyDown(KeyCode.E))
        {
            bool unlocked = progressManager.IsDoorUnlocked(doorID);

            // ❌ No tiene llaves → sonido + UI roja
            if (!unlocked)
            {
                audioCtrl.Play(wrongSound);
                StartCoroutine(FlashWrongUI());
                return;
            }

            // ✅ Tiene llaves y puerta aún no se abrió 
            if (unlocked && !hasOpened)
            {
                audioCtrl.Play(GoodSound);
                StartCoroutine(OpenDoorRoutine());
            }
        }
    }

    private IEnumerator FlashWrongUI()
    {
        if (interactionUI == null)
            yield break;

        // Guardar el color original
        var img = interactionUI.GetComponent<UnityEngine.UI.Image>();
        if (img == null) yield break;

        Color originalColor = img.color;

        // Cambiar a rojo
        img.color = Color.red;

        // Mantener rojo por un instante
        yield return new WaitForSeconds(0.2f);

        // Volver al color original
        img.color = originalColor;
    }

    private IEnumerator OpenDoorRoutine()
    {
        hasOpened = true;

        // 🌀 Ejecutar animación solo si NO va al menú
        if (!goToMenu && doorAnimator != null)
        {
            doorAnimator.SetTrigger(openTriggerName);
        }



        yield return new WaitForSeconds(animationDelay);

        if (goToMenu)
        {
            GameManager.CursorVisible(true);
            SaveSystem.Delete();
            GameManager.ResetMissionProgress();
            GameManager.LoadScene(targetSceneName);
        }
        else
        {
            var player3D = progressManager.managerTransition.player3D;

            if (player3D != null && teleportTarget != null)
            {
                player3D.transform.position = teleportTarget.position;
                progressManager.SavePosition3D(teleportTarget.position);
                progressManager.SetCurrentMission(missionText);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (interactionUI != null)
                interactionUI.SetActive(true);
            isPlayerInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (interactionUI != null)
            interactionUI.SetActive(false);

        if (other.CompareTag("Player"))
            isPlayerInside = false;
    }
}
