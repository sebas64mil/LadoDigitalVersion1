using UnityEngine;
using System.Collections;

public class KeyController : MonoBehaviour
{
    [Header("Referencias")]
    public PlayerProgressManager progressManager;

    [Header("Configuración de llave")]
    public int doorID;
    public int keyIndex;

    [Header("Interacción")]
    public KeyCode interactionKey = KeyCode.E;
    private bool isPlayerNearby = false;

    [Header("UI")]
    public GameObject interactionUI;

    [Header("Sonidos Config")]
    public AudioClip openSound;
    private SFXAudioController audioCtrl;

    [Header("Efecto visual (flotante)")]
    public float floatAmplitude = 0.25f;  // desplazamiento en local Y
    public float floatSpeed = 2f;         // velocidad de oscilación
    public float rotationSpeed = 45f;     // grados por segundo (local)

    private Vector3 startLocalPos;

    private IEnumerator Start()
    {
        // Esperar unos frames para asegurarnos de que todo se haya posicionado bien
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        if (audioCtrl == null)
            audioCtrl = gameObject.AddComponent<SFXAudioController>();

        if (interactionUI != null)
            interactionUI.SetActive(false);

        bool[] keyArray = progressManager.GetKeysForDoor(doorID);
        if (keyArray != null && keyIndex < keyArray.Length && keyArray[keyIndex])
        {
            gameObject.SetActive(false);
            yield break;
        }

        // Guardar posición local real (ya estable)
        startLocalPos = transform.localPosition;
    }

    private void Update()
    {
        if (progressManager == null) return;

        // Mantener efecto visual
        FloatAndRotateLocal();

        if (isPlayerNearby && Input.GetKeyDown(interactionKey))
        {
            CollectKey();
        }
    }

    private void FloatAndRotateLocal()
    {
        // Calcular desplazamiento vertical
        float offsetY = Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;

        // Aplicar desplazamiento sobre la posición local original
        transform.localPosition = startLocalPos + new Vector3(0, offsetY, 0);

        // Rotar de forma continua (sin alterar la posición base)
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (interactionUI != null)
                interactionUI.SetActive(true);

            isPlayerNearby = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (interactionUI != null)
                interactionUI.SetActive(false);

            isPlayerNearby = false;
        }
    }

    private void CollectKey()
    {
        StartCoroutine(PlaySoundAndDisable());
    }

    private IEnumerator PlaySoundAndDisable()
    {
        if (audioCtrl != null && openSound != null)
            audioCtrl.Play(openSound);

        if (openSound != null)
            yield return new WaitForSeconds(openSound.length);
        else
            yield return null;

        progressManager.SetKeyState(doorID, keyIndex, true);

        if (KeyUITextController.Instance != null)
            KeyUITextController.Instance.ShowKeyMessage(keyIndex);

        gameObject.SetActive(false);
    }
}
