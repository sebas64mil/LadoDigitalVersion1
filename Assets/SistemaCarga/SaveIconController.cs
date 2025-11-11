using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SaveIconController : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Image saveIcon;
    [SerializeField] private float blinkDuration = 1f;
    [SerializeField] private float blinkSpeed = 6f;

    private Coroutine blinkRoutine;

    private void OnEnable()
    {
        PlayerProgressManager.OnGameSaved += HandleGameSaved;
    }

    private void OnDisable()
    {
        PlayerProgressManager.OnGameSaved -= HandleGameSaved;
    }

    private void HandleGameSaved()
    {
        if (blinkRoutine != null)
            StopCoroutine(blinkRoutine);

        blinkRoutine = StartCoroutine(BlinkIcon());
    }

    private IEnumerator BlinkIcon()
    {
        float elapsed = 0f;
        Color color = saveIcon.color;

        while (elapsed < blinkDuration)
        {
            float alpha = Mathf.Abs(Mathf.Sin(Time.time * blinkSpeed));
            saveIcon.color = new Color(color.r, color.g, color.b, alpha);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Al final, dejar el ícono invisible
        saveIcon.color = new Color(color.r, color.g, color.b, 0);
    }
}
