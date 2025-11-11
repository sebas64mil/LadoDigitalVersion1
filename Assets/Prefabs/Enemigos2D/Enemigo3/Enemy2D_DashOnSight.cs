using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

[RequireComponent(typeof(BoxCollider2D), typeof(SpriteRenderer))]
public class Enemy2D_DashOnSight : MonoBehaviour
{
    [Header("Configuración de Patrulla")]
    public Vector2[] waypoints;
    private int currentWaypoint = 0;

    [Header("Configuración del Dash")]
    public float speed = 2f;
    public float dashSpeed = 6f;
    public float detectionRange = 5f;

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private bool isDashing = false;
    private Vector2 dashTarget;
    [HideInInspector] public bool IsAttack = false;

    [Header("Gizmos")]
    public Color pointColor = Color.red;
    public Color lineColor = Color.yellow;
    public float gizmoRadius = 0.1f;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Vector2 origin = transform.position;
        Vector2 direction = spriteRenderer.flipX ? Vector2.left : Vector2.right;

        Debug.DrawRay(origin, direction * detectionRange, Color.red);

        if (!isDashing)
        {
            int mask = LayerMask.GetMask("Player", "Ground");
            RaycastHit2D hit = Physics2D.Raycast(origin, direction, detectionRange, mask);

            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                dashTarget = new Vector2(hit.collider.transform.position.x, transform.position.y);
                isDashing = true;
                IsAttack = true;

                if (animator != null)
                    animator.SetBool("IsAttack", true);

            }
            else
            {
                Patrol();
            }
        }
        else
        {
            Vector2 current = transform.position;
            transform.position = Vector2.MoveTowards(current, dashTarget, dashSpeed * Time.deltaTime);

            if (Vector2.Distance(current, dashTarget) < 0.1f)
            {
                isDashing = false;
                IsAttack = false;

                if (animator != null)
                    animator.SetBool("IsAttack", false);

            }
        }
    }

    private void Patrol()
    {
        if (waypoints == null || waypoints.Length == 0)
            return;

        Vector2 target = waypoints[currentWaypoint];
        Vector2 current = transform.position;
        Vector2 direction = (target - current).normalized;

        transform.position = Vector2.MoveTowards(current, target, speed * Time.deltaTime);

        if (spriteRenderer != null)
            spriteRenderer.flipX = direction.x < 0;

        if (Vector2.Distance(current, target) < 0.05f)
        {
            currentWaypoint++;
            if (currentWaypoint >= waypoints.Length)
                currentWaypoint = 0;
        }

        // Desactivar ataque si está patrullando
        IsAttack = false;
        if (animator != null)
            animator.SetBool("IsAttack", false);
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
