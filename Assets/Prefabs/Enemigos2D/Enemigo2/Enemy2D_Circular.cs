using UnityEngine;

[RequireComponent(typeof(BoxCollider2D), typeof(SpriteRenderer))]
public class Enemy2D_Circular : MonoBehaviour
{
    [Header("Configuración del Movimiento Circular")]
    public float speed = 2f;           // Velocidad angular
    public float radius = 2f;          // Radio del círculo
    public Vector2 center;             // Centro de la rotación

    private SpriteRenderer spriteRenderer;

    private float angle = 0f;

    [Header("Gizmos (visualización)")]
    public Color centerColor = Color.yellow;
    public Color radiusColor = Color.cyan;
    public Color enemyPositionColor = Color.red;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Si no se asigna un centro, usar la posición actual
        if (center == Vector2.zero)
            center = transform.position;
    }

    private void Update()
    {
        angle += speed * Time.deltaTime; // velocidad de rotación
        Vector2 newPos = center + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;

        // Voltear sprite según dirección horizontal
        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = newPos.x < transform.position.x;
        }

        transform.position = newPos;
    }

    private void OnDrawGizmos()
    {
        // Dibuja el centro
        Gizmos.color = centerColor;
        Gizmos.DrawSphere(center, 0.1f);

        // Dibuja el círculo de radio
        Gizmos.color = radiusColor;
        Gizmos.DrawWireSphere(center, radius);

        // Dibuja la posición actual del enemigo
        Gizmos.color = enemyPositionColor;
        Gizmos.DrawSphere(transform.position, 0.08f);

        // Línea entre el centro y el enemigo
        Gizmos.DrawLine(center, transform.position);
    }
}
