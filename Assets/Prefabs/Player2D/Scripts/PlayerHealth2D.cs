using UnityEngine;

public class PlayerHealth2D : MonoBehaviour
{
    [Header("Vida del jugador")]
    [SerializeField] private int maxHealth = 1;
    private int currentHealth;

    [Header("Referencias")]
    public PlayerController2d playerController;
    private Rigidbody2D rb;
    private Animator animator;
    private BoxCollider2D BoxCollider2D;

    private bool isDead = false;

    private void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        BoxCollider2D = GetComponent<BoxCollider2D>();
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return; // evita que reciba más daño si ya murió

        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;

        playerController.sfxController.PlayDeath();
        playerController.sfxController.StopWalk();
        playerController.sfxController.StopClimb();

        // Desactivar control de movimiento
        if (playerController != null)
            playerController.enabled = false;

        // Detener completamente el Rigidbody2D
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.gravityScale = 0f;
            rb.bodyType = RigidbodyType2D.Static;
        }

        // Desactivar el collider para evitar más colisiones

        if (BoxCollider2D != null)
            BoxCollider2D.enabled = false;

        // Activar animación de muerte
        if (animator != null)
            animator.SetBool("Isdead", true);

        // Reiniciar la escena después de un pequeño delay (espera animación)
        Invoke(nameof(RestartLevel), 2f); // puedes ajustar el tiempo según la duración de tu animación
    }

    private void RestartLevel()
    {
        GameManager.RestartScene();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            TakeDamage(1);
        }
    }
}
