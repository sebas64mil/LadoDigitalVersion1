using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro; // 👈 Importante para usar TextMeshPro

[RequireComponent(typeof(Collider))]
public class VisualTransitionController : MonoBehaviour
{
    [Header(" Referencias")]
    [SerializeField] private Renderer normalRenderer;     // Objeto A (cambio de color)
    [SerializeField] private Renderer hologramRenderer;   // Objeto B (shader con CutOfHeight)
    [SerializeField] private Image uiImage;               // Imagen UI principal (fade alpha)
    [SerializeField] private Image uiImage2;              // ✅ Nueva: segunda imagen UI
    [SerializeField] private TMP_Text uiText;             // ✅ Nueva: texto TMP
    [SerializeField] private string playerTag = "Player"; // Tag del jugador

    [Header(" Colores")]
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color highlightColor = Color.cyan;

    [Header("Parámetros de transición")]
    [SerializeField] private float colorSpeed = 3f;
    [SerializeField] private float uiFadeSpeed = 2f;
    [SerializeField] private float hologramSpeed = 2f;

    [Header(" Margen vertical extra (para CutOfHeight)")]
    [SerializeField] private float bottomOffset = 0.2f;
    [SerializeField] private float topOffset = 0.2f;

    [Header(" Estado inicial")]
    [SerializeField] private bool startActive = false;

    [Header("Sonidos Config")]
    public AudioClip openSound;
    private SFXAudioController audioCtrl;

    private Material normalMatInstance;
    private Material hologramMatInstance;

    private float bottom;
    private float top;
    private bool isInside = false;

    private void Start()
    {
        Collider col = GetComponent<Collider>();

        if (audioCtrl == null)
            audioCtrl = gameObject.AddComponent<SFXAudioController>();

        col.isTrigger = true;

        // 🧱 Clonar materiales
        if (normalRenderer)
        {
            normalMatInstance = Instantiate(normalRenderer.sharedMaterial);
            normalRenderer.material = normalMatInstance;
            normalMatInstance.color = normalColor;
        }

        if (hologramRenderer)
        {
            hologramMatInstance = Instantiate(hologramRenderer.sharedMaterial);
            hologramRenderer.material = hologramMatInstance;
            bottom = hologramRenderer.bounds.min.y - bottomOffset;
            top = hologramRenderer.bounds.max.y + topOffset;
            hologramMatInstance.SetFloat("_CutOfHeight", bottom);
        }

        // Ocultar al inicio
        SetUIAlpha(0f);

        // Verificar si el jugador ya está dentro al inicio
        Collider[] overlaps = Physics.OverlapBox(col.bounds.center, col.bounds.extents, transform.rotation);
        foreach (Collider overlap in overlaps)
        {
            if (overlap.CompareTag(playerTag))
            {
                isInside = true;
                break;
            }
        }

        if (startActive || isInside)
            ApplyActiveState();
        else
            ApplyInactiveState();
    }

    // ============================================================
    //  Detección de entrada/salida del jugador
    // ============================================================
    private void OnTriggerEnter(Collider other)
    {
        if (!isInside && other.CompareTag(playerTag))
        {
            audioCtrl.Play(openSound);
            isInside = true;
            OnEnterZone();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isInside && other.CompareTag(playerTag))
        {
            audioCtrl.Play(openSound);
            isInside = false;
            OnExitZone();
        }
    }

    // ============================================================
    //  Métodos principales
    // ============================================================
    private void OnEnterZone()
    {
        StopAllCoroutines();
        StartCoroutine(ChangeColor(normalColor, highlightColor));
        StartCoroutine(FadeUI(0f, 1f));
        StartCoroutine(HologramTransition(bottom, top));
    }

    private void OnExitZone()
    {
        StopAllCoroutines();
        StartCoroutine(ChangeColor(highlightColor, normalColor));
        StartCoroutine(FadeUI(1f, 0f));
        StartCoroutine(HologramTransition(top, bottom));
    }

    // ============================================================
    //  Aplicar estado inicial sin transición
    // ============================================================
    private void ApplyActiveState()
    {
        if (normalMatInstance)
        {
            if (normalMatInstance.HasProperty("_BaseColor"))
                normalMatInstance.SetColor("_BaseColor", highlightColor);
            if (normalMatInstance.HasProperty("_EmissionColor"))
            {
                normalMatInstance.EnableKeyword("_EMISSION");
                normalMatInstance.SetColor("_EmissionColor", highlightColor);
            }
        }

        SetUIAlpha(1f);

        if (hologramMatInstance)
            hologramMatInstance.SetFloat("_CutOfHeight", top);
    }

    private void ApplyInactiveState()
    {
        if (normalMatInstance)
        {
            if (normalMatInstance.HasProperty("_BaseColor"))
                normalMatInstance.SetColor("_BaseColor", normalColor);
            if (normalMatInstance.HasProperty("_EmissionColor"))
            {
                normalMatInstance.EnableKeyword("_EMISSION");
                normalMatInstance.SetColor("_EmissionColor", normalColor);
            }
        }

        SetUIAlpha(0f);

        if (hologramMatInstance)
            hologramMatInstance.SetFloat("_CutOfHeight", bottom);
    }

    // ============================================================
    //  Cambio de color (Base + Emission)
    // ============================================================
    private IEnumerator ChangeColor(Color from, Color to)
    {
        if (!normalMatInstance) yield break;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * colorSpeed;
            Color newColor = Color.Lerp(from, to, t);

            if (normalMatInstance.HasProperty("_BaseColor"))
                normalMatInstance.SetColor("_BaseColor", newColor);
            else if (normalMatInstance.HasProperty("_Color"))
                normalMatInstance.SetColor("_Color", newColor);

            if (normalMatInstance.HasProperty("_EmissionColor"))
            {
                normalMatInstance.EnableKeyword("_EMISSION");
                normalMatInstance.SetColor("_EmissionColor", newColor);
            }

            yield return null;
        }
    }

    // ============================================================
    //  Fade de UI (Imagen 1 + Imagen 2 + TMP)
    // ============================================================
    private IEnumerator FadeUI(float from, float to)
    {
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * uiFadeSpeed;
            float currentAlpha = Mathf.Lerp(from, to, t);
            SetUIAlpha(currentAlpha);
            yield return null;
        }
    }

    // ============================================================
    //  Aplicar alpha a múltiples elementos UI
    // ============================================================
    private void SetUIAlpha(float alpha)
    {
        // Imagen principal
        if (uiImage)
        {
            Color c = uiImage.color;
            c.a = alpha;
            uiImage.color = c;
        }

        // Segunda imagen
        if (uiImage2)
        {
            Color c2 = uiImage2.color;
            c2.a = alpha;
            uiImage2.color = c2;
        }

        // Texto TMP
        if (uiText)
        {
            Color tColor = uiText.color;
            tColor.a = alpha;
            uiText.color = tColor;
        }
    }

    // ============================================================
    //  Holograma (CutOfHeight)
    // ============================================================
    private IEnumerator HologramTransition(float from, float to)
    {
        if (!hologramMatInstance) yield break;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * hologramSpeed;
            float current = Mathf.Lerp(from, to, t);
            hologramMatInstance.SetFloat("_CutOfHeight", current);
            yield return null;
        }
    }
}
