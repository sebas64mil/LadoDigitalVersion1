using UnityEngine;
using UnityEngine.UI;

public class PlayerStaminaView : MonoBehaviour
{
    [Header("Referencias UI")]
    [SerializeField] private Image staminaRoot;
    [SerializeField] private Image staminaFill;

    [Header("Configuración de Stamina")]
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float staminaDrainRate = 20f;
    [SerializeField] private float staminaRecoveryRate = 15f;
    [SerializeField] private float colorHighlightDuration = 1.5f; // duración del cambio de color
    [SerializeField] private float stayVisibleAfterFull = 1.5f;   // tiempo extra visible al llenarse

    [Header("Configuración visual")]
    [SerializeField] private float fadeSpeed = 3f;
    [SerializeField] private Color normalColor = Color.yellow;
    [SerializeField] private Color rechargeFullColor = Color.green;

    [Header("Running Breathing Audio")]
    public AudioSource runBreathingSource;
    public float runBreathingMaxVolume = 1f;
    public float runBreathingMinVolume = 0.2f;
    public float breathingBuildSpeed = 1f;
    public float breathingDecreaseSpeed = 2f;

    private float runningIntensity = 0f;

    private float moveMagnitude;
    public float movementThreshold = 0.1f;


    private float currentStamina;
    private bool isRunning;
    private bool staminaDepleted;
    private float targetAlpha;
    private float colorTimer;
    private bool highlightActive;

    private float hideTimer;

    void Start()
    {
        currentStamina = maxStamina;
        staminaFill.fillAmount = 1f;
        staminaDepleted = false;

        // Iniciar invisible
        SetAlpha(0f);
        staminaFill.color = normalColor;
    }

    void Update()
    {
        UpdateStamina();
        UpdateUIVisibility();
        UpdateFade();
        UpdateColorHighlight();
        UpdateRunBreathing();

    }

    public void SetRunningState(bool value)
    {
        isRunning = value;
    }


    public void SetMovementMagnitude(float magnitude)
    {
        moveMagnitude = magnitude;
    }


    private void UpdateStamina()
    {
        if (isRunning && currentStamina > 0)
        {
            currentStamina -= staminaDrainRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);

            if (currentStamina <= 0)
            {
                staminaDepleted = true;
                isRunning = false;
            }
        }
        else if (!isRunning && currentStamina < maxStamina)
        {
            currentStamina += staminaRecoveryRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);

            if (currentStamina >= maxStamina)
            {
                staminaDepleted = false;

                // Efecto visual de recarga completa
                highlightActive = true;
                colorTimer = 0f;
                hideTimer = 0f;
            }
        }

        staminaFill.fillAmount = currentStamina / maxStamina;
    }


    private void UpdateUIVisibility()
    {
        if (isRunning)
        {
            // Mientras corre
            targetAlpha = 1f;
            hideTimer = 0f;
        }
        else if (staminaDepleted)
        {
            // Se agotó, mostrar hasta recargarse
            targetAlpha = 1f;
        }
        else if (!isRunning && currentStamina < maxStamina && !staminaDepleted)
        {
            // Se dejó de correr antes de agotarla → ocultar mientras recarga
            targetAlpha = 0f;
        }
        else if (currentStamina >= maxStamina)
        {
            // Barra llena → visible solo durante highlight y un poco más
            hideTimer += Time.deltaTime;
            if (hideTimer < colorHighlightDuration + stayVisibleAfterFull)
                targetAlpha = 1f;
            else
                targetAlpha = 0f;
        }
    }

    private void UpdateRunBreathing()
    {
        bool isActuallyRunning = IsRunning;

        if (isActuallyRunning)
        {
            runningIntensity += breathingBuildSpeed * Time.deltaTime;
            runningIntensity = Mathf.Clamp01(runningIntensity);

            //  Solo volumen (sin pitch)
            runBreathingSource.volume = Mathf.Lerp(runBreathingMinVolume, runBreathingMaxVolume, runningIntensity);

            if (!runBreathingSource.isPlaying)
                runBreathingSource.Play();
        }
        else
        {
            float speed = staminaDepleted ? breathingDecreaseSpeed * 2f : breathingDecreaseSpeed;

            runningIntensity -= speed * Time.deltaTime;
            runningIntensity = Mathf.Clamp01(runningIntensity);

            //  Solo volumen (sin pitch)
            runBreathingSource.volume = Mathf.Lerp(runBreathingMinVolume, runBreathingMaxVolume, runningIntensity);

            if (runningIntensity <= 0.01f && runBreathingSource.isPlaying)
                runBreathingSource.Pause();
        }
    }




    private void UpdateFade()
    {
        float newAlpha = Mathf.Lerp(staminaRoot.color.a, targetAlpha, fadeSpeed * Time.deltaTime);
        SetAlpha(newAlpha);
    }

    private void UpdateColorHighlight()
    {
        if (!highlightActive) return;

        colorTimer += Time.deltaTime;
        staminaFill.color = Color.Lerp(rechargeFullColor, normalColor, colorTimer / colorHighlightDuration);

        if (colorTimer >= colorHighlightDuration)
        {
            staminaFill.color = normalColor;
            highlightActive = false;
        }
    }

    private void SetAlpha(float a)
    {
        Color root = staminaRoot.color;
        root.a = a;
        staminaRoot.color = root;

        Color fill = staminaFill.color;
        fill.a = a;
        staminaFill.color = fill;
    }

    // --- Métodos auxiliares ---
    public bool HasStamina() => !staminaDepleted;
    public bool IsRunning => isRunning && !staminaDepleted;
}
