using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

[RequireComponent(typeof(Collider))]
public class WallLine : MonoBehaviour
{
    [Header("Offset individual por eje")]
    public float offsetX = 0.1f;
    public float offsetZ = 0.1f;

    [Tooltip("Dibujar las líneas de debug en la escena")]
    public bool debugDraw = true;
    public Color lineColor = Color.green;
    public Color textColor = Color.yellow;

    private Vector3[] points;

    void Awake()
    {
        CalculateCorners();
    }

    // Calcula las esquinas en el plano XZ usando el Bounds del Collider
    private void CalculateCorners()
    {
        Collider col = GetComponent<Collider>();
        if (col == null) return;

        Bounds b = col.bounds;

        // Calculamos las 4 esquinas exactas del plano XZ del collider
        points = new Vector3[4];
        points[0] = new Vector3(b.min.x - offsetX, transform.position.y, b.min.z - offsetZ);
        points[1] = new Vector3(b.min.x - offsetX, transform.position.y, b.max.z + offsetZ);
        points[2] = new Vector3(b.max.x + offsetX, transform.position.y, b.max.z + offsetZ);
        points[3] = new Vector3(b.max.x + offsetX, transform.position.y, b.min.z - offsetZ);
    }



    public Vector3 GetPoint(int index)
    {
        if (points == null || index < 0 || index >= points.Length) return Vector3.zero;
        return points[index];
    }

    public int SegmentCount => (points != null) ? Mathf.Max(0, points.Length) : 0;

    public Vector3 ClosestPointOnSegment(int index, Vector3 position)
    {
        if (index < 0 || points == null || points.Length < 2) return Vector3.zero;

        int next = (index + 1) % points.Length;

        Vector3 a = GetPoint(index);
        Vector3 b = GetPoint(next);

        Vector3 ab = b - a;
        float length = ab.magnitude;
        Vector3 dir = ab.normalized;

        Vector3 ap = position - a;
        float dot = Vector3.Dot(ap, dir);
        dot = Mathf.Clamp(dot, 0f, length);

        return a + dir * dot;
    }

    public Vector3 SegmentDirection(int index)
    {
        if (points == null || points.Length < 2) return Vector3.forward;
        int next = (index + 1) % points.Length;
        return (GetPoint(next) - GetPoint(index)).normalized;
    }

    private void OnDrawGizmos()
    {
        if (!debugDraw) return;
        CalculateCorners();

        Gizmos.color = lineColor;

        // Dibujar líneas
        for (int i = 0; i < points.Length; i++)
        {
            Vector3 current = points[i];
            Vector3 next = points[(i + 1) % points.Length];

            Gizmos.DrawLine(current, next);

#if UNITY_EDITOR
            // Dibujar número de índice encima del punto
            GUIStyle style = new GUIStyle();
            style.normal.textColor = textColor;
            style.fontSize = 14;
            Handles.Label(current + Vector3.up * 0.1f, $"[{i}]", style);
#endif

            // Dibujar esferas en cada vértice
            Gizmos.DrawSphere(current, 0.05f);
        }
    }
}
