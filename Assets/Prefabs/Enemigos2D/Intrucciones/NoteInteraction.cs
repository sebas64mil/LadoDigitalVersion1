using UnityEngine;
using TMPro;

public class NoteInteractionToggle : MonoBehaviour
{
    [Header("UI References")]
    public GameObject interactIcon; // World Space icon "Press E"
    public GameObject notePanel;    // Overlay Note Panel
    public TMP_Text noteText;

    [Header("Note Content")]
    [TextArea(4, 10)]
    public string noteContent;

    private bool playerInRange = false;
    private bool noteOpen = false;

    [Header("Sonidos Config")]
    public AudioClip openSound;
    private SFXAudioController audioCtrl;

    void Start()
    {
        interactIcon.SetActive(false);
        notePanel.SetActive(false);

        if (audioCtrl == null)
            audioCtrl = gameObject.AddComponent<SFXAudioController>();
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
        noteText.text = noteContent;

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
}
