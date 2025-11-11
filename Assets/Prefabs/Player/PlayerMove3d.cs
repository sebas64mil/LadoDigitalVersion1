using UnityEngine;
using UnityEngine.AI;

public class PlayerMove3D : MonoBehaviour
{
    [Header("Datos del jugador (SO)")]
    public PlayerStats3d playerStats;

    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public CapsuleCollider capsule;
    [HideInInspector] public BoxCollider boxTrigger;
    [HideInInspector] public NavMeshObstacle navmeshObstacle;

    [Header("Referencias")]
    public Transform playerCamera;
    public Transform playerPrefab;


    [HideInInspector] public bool isCrouching = false;
    [HideInInspector] public bool isRunning = false;
    [HideInInspector] public bool hayObstaculoArriba = false;
    [HideInInspector] public float xRotation = 0f;
    [HideInInspector] public bool runToggleState = false;
    [HideInInspector] public bool crouchToggleState = false;

    [Header("animations and controller")]
    public Animator animator;


    public PlayerStaminaView staminaView;

    private Vector3 cameraStandPos;
    private Vector3 originalColliderCenter;
    private Vector3 runCameraPos;
    private Vector3 walkCameraPos;

    private PlayerState currentState;

    [Header("Configuración del Trigger")]
    [SerializeField] private string ignoreTag = "IgnoreTrigger";
    [SerializeField] private string ignoreTag2 = "IgnoreTrigger";

    [Header("Audio Footsteps / Breathing")]
    public AudioSource stepsSource;       // AudioSource ya configurado en el inspector
    public AudioSource breathingSource;   // AudioSource ya configurado
    public AudioSource DeathSound;

    public float walkStepRate = 0.5f;


    void Start()
    {

        rb = GetComponent<Rigidbody>();
        capsule = GetComponent<CapsuleCollider>();
        boxTrigger = GetComponent<BoxCollider>();
        navmeshObstacle = GetComponent<NavMeshObstacle>();

        rb.freezeRotation = true;
        cameraStandPos = playerCamera.localPosition;
        originalColliderCenter = capsule.center;

        cameraStandPos = playerCamera.localPosition;
        runCameraPos = cameraStandPos + new Vector3(0, -0.3f, 0);
        walkCameraPos = cameraStandPos + new Vector3(0, 0f, 0); // baja un poco al caminar
     

        if (boxTrigger != null)
            boxTrigger.enabled = false;

        GeneralPlayerSettingsManager.Instance.OnCameraSensitivityChanged += UpdateSensitivity;
        UpdateSensitivity(GeneralPlayerSettingsManager.Instance.CameraSensitivity);

        ChangeState(new NormalState()); // arrancamos en estado normal
    }
    void OnDestroy()
    {
        if (GeneralPlayerSettingsManager.Instance != null)
            GeneralPlayerSettingsManager.Instance.OnCameraSensitivityChanged -= UpdateSensitivity;
    }

    void UpdateSensitivity(float value)
    {
        playerStats.mouseSensitivity = value;
    }
    void Update()
    {
        currentState?.Update(this);
        float moveMagnitude = new Vector2(rb.linearVelocity.x, rb.linearVelocity.z).magnitude;
        staminaView.SetMovementMagnitude(moveMagnitude);

        staminaView.SetRunningState(isRunning);
    }

    void FixedUpdate()
    {
        currentState?.FixedUpdate(this);
    }

    public void ChangeState(PlayerState newState)
    {
        currentState?.Exit(this);
        currentState = newState;
        currentState.Enter(this);
    }

    public void HandleWalkSteps(float moveMagnitude)
    {
        bool shouldPlaySteps = moveMagnitude > 0.1f; // Sí suena pasos si se mueve

        // --- Si NO se está moviendo, silencio ---
        if (!shouldPlaySteps)
        {
            if (stepsSource.isPlaying)
                stepsSource.Pause();
            return;
        }

        // --- Ajuste según estado ---
        if (isCrouching)
        {
            stepsSource.pitch = 0.8f;
            stepsSource.volume = 0.4f;
        }
        else if (isRunning)
        {
            stepsSource.pitch = 1.3f;
            stepsSource.volume = 0.8f;
        }
        else
        {
            stepsSource.pitch = 1f;
            stepsSource.volume = 0.6f;
        }

        if (!stepsSource.isPlaying)
            stepsSource.Play();
    }




    public void StartCrouchBreathing()
    {
        if (!breathingSource.isPlaying)
            breathingSource.Play();
    }

    public void StopCrouchBreathing()
    {
        if (breathingSource.isPlaying)
            breathingSource.Stop();
    }

    public void PlayDeathSound()
    {
        if (!DeathSound.isPlaying)
            DeathSound.Play();
    }




    // --- Helpers accesibles por estados ---
    public Vector3 GetCameraStandPos() => cameraStandPos;
    public Vector3 GetColliderCenter() => originalColliderCenter;

    public Vector3 GetCameraRunPos() => runCameraPos;
    public Vector3 GetCameraWalkPos() => walkCameraPos;


    public System.Collections.IEnumerator LerpCamera(Vector3 targetPos)
    {
        float elapsed = 0f;
        Vector3 startPos = playerCamera.localPosition;
        while (elapsed < 0.2f)
        {
            playerCamera.localPosition = Vector3.Lerp(startPos, targetPos, elapsed / 0.2f);
            elapsed += Time.deltaTime;
            yield return null;
        }
        playerCamera.localPosition = targetPos;
    }
    private void OnTriggerEnter(Collider other)
    {

        // Si NO tiene ninguna de las dos tags ignoradas → obstáculo
        if (!other.CompareTag(ignoreTag) && !other.CompareTag(ignoreTag2))
            hayObstaculoArriba = true;
    }

    private void OnTriggerExit(Collider other)
    {

        if (!other.CompareTag(ignoreTag) && !other.CompareTag(ignoreTag2))
            hayObstaculoArriba = false;
    }

}
