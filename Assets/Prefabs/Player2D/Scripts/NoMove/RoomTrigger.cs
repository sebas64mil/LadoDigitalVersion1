using UnityEngine;
using Unity.Cinemachine; // <- importante

[RequireComponent(typeof(Collider2D))]
public class RoomTrigger : MonoBehaviour
{
    [Header("Referencias (asignar en Inspector)")]
    public Collider2D roomBounds;
    public CinemachineCamera virtualCamera;

    private CinemachineConfiner2D confiner;

    private void Reset()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void Awake()
    {
        if (virtualCamera != null)
            confiner = virtualCamera.GetComponent<CinemachineConfiner2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player") || confiner == null || roomBounds == null)
            return;

        // En Cinemachine 3 la propiedad se llama BoundingShape2D (antes m_BoundingShape2D)
        if (confiner.BoundingShape2D == roomBounds)
            return;

        confiner.BoundingShape2D = roomBounds;

        // InvalidateCache() está marcado como obsolete — usa InvalidateBoundingShapeCache()
        confiner.InvalidateBoundingShapeCache();
    }
}
