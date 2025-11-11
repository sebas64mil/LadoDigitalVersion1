using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

[RequireComponent(typeof(BoxCollider2D), typeof(SpriteRenderer))]
public class Enemy2D_FixedPath : MonoBehaviour
{
    [Header("Configuración del Enemigo")]
    public float speed = 2f;
    public Vector2[] waypoints;
    private int currentWaypoint = 0;

    [Header("Componentes")]
    private SpriteRenderer spriteRenderer;

    [HideInInspector] public bool IsAttack = false;

    [Header("Gizmos")]
    public Color pointColor = Color.red;
    public Color lineColor = Color.yellow;
    public float gizmoRadius = 0.1f;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (waypoints == null || waypoints.Length == 0)
            return;

        Vector2 target = waypoints[currentWaypoint];
        Vector2 current = transform.position;
        Vector2 direction = (target - current).normalized;

        // Movimiento hacia el waypoint
        transform.position = Vector2.MoveTowards(
            current,
            target,
            speed * Time.deltaTime
        );

        // Girar sprite según dirección
        if (direction.x > 0.01f)
            spriteRenderer.flipX = true;
        else if (direction.x < -0.01f)
            spriteRenderer.flipX = false;

        // Pasar al siguiente waypoint
        if (Vector2.Distance(current, target) < 0.05f)
        {
            currentWaypoint++;
            if (currentWaypoint >= waypoints.Length)
                currentWaypoint = 0;
        }
    }

    private void OnDrawGizmos()
    {
        if (waypoints == null || waypoints.Length == 0)
            return;

        Gizmos.color = pointColor;

        // Dibujar puntos
        foreach (Vector2 wp in waypoints)
        {
            Gizmos.DrawSphere((Vector2)wp, gizmoRadius);
        }

        // Dibujar líneas entre puntos
        Gizmos.color = lineColor;
        for (int i = 0; i < waypoints.Length - 1; i++)
        {
            Gizmos.DrawLine(waypoints[i], waypoints[i + 1]);
        }

        // Línea final para cerrar el circuito (opcional)
        if (waypoints.Length > 1)
        {
            Gizmos.DrawLine(waypoints[waypoints.Length - 1], waypoints[0]);
        }
    }
}
