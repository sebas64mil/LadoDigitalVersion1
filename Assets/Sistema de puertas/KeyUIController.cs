using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Localization;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Localization.Settings;


public class KeyUIController : MonoBehaviour
{
    public static KeyUIController Instance;

    [Header("Sprites de estado")]
    public Sprite keyEmpty;
    public Sprite keyFull;

    [Header("Referencias")]
    public Image[] keyIcons;
    public PlayerProgressManager progressManager;
    [Header("Misión Localizada al Activar el Portal")]
    public LocalizedString missionMessageKey;

    [Header("Configuración")]
    public int doorID;



    private void Awake()
    {
        Instance = this;
    }

    private IEnumerator Start()
    {
        yield return null;
        yield return new WaitUntil(() => progressManager.currentData != null && progressManager.currentData.doors != null);
        UpdateKeysUI();
    }

    private void OnEnable()
    {
        PlayerProgressManager.OnKeyCollected += HandleKeyCollected;
    }

    private void OnDisable()
    {
        PlayerProgressManager.OnKeyCollected -= HandleKeyCollected;
    }

    private void HandleKeyCollected(int collectedDoorID, int collectedKeyIndex)
    {
        // Solo actualiza si corresponde a esta puerta
        if (collectedDoorID == doorID)
        {
            UpdateKeysUI();

            // Verifica si TODAS las llaves de esta puerta están completas
            if (AllKeysFull())
            {
                long id = missionMessageKey.TableEntryReference.KeyId;

                // Obtener la clave string real ("I018")
                string realKey = PlayerProgressManager.Instance.GetKeyStringFromId("Tabla1", id);


                // Enviar misión usando la key STRING real
                PlayerProgressManager.Instance.SetCurrentMission(realKey);
            }
        }
    }

    private bool AllKeysFull()
    {
        bool[] keyArray = progressManager.GetKeysForDoor(doorID);
        if (keyArray == null || keyArray.Length == 0) return false;

        // Retorna true solo si todas las llaves están en true
        foreach (bool hasKey in keyArray)
        {
            if (!hasKey)
                return false;
        }
        return true;
    }


    public void UpdateKeysUI()
    {
        bool[] keyArray = progressManager.GetKeysForDoor(doorID);
        if (keyArray == null || keyIcons == null) return;

        for (int i = 0; i < keyIcons.Length; i++)
        {
            keyIcons[i].sprite = (i < keyArray.Length && keyArray[i]) ? keyFull : keyEmpty;
        }
    }
}
