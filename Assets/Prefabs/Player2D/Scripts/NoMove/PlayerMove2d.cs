using UnityEngine;

public class PlayerMove2d : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private PlayerStats2D stats;          // ScriptableObject con todas las variables
    [SerializeField] private LayerMask obstacleLayer;      // Layer de obstáculos

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private bool canDash = true;
    private bool isDashing = false;
    private float dashEndTime;
    private Vector2 dashStart;
    private Vector2 dashEnd;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (!isDashing)
        {
            Move();
            Jump();
            Run();

            // Better Jump
            if (stats.betterJump)
            {
                if (rb.linearVelocity.y < 0)
                {
                    rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (stats.fallMultiplier - 1) * Time.deltaTime;
                }
                else if (rb.linearVelocity.y > 0 && !Input.GetKey(KeyCode.Space))
                {
                    rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (stats.lowJumpMultiplier - 1) * Time.deltaTime;
                }
            }
        }
        else
        {
            // Movimiento interpolado del dash
            float t = 1 - ((dashEndTime - Time.time) / stats.dashDuration);
            rb.MovePosition(Vector2.Lerp(dashStart, dashEnd, t));

            if (Time.time >= dashEndTime)
            {
                isDashing = false;
                rb.gravityScale = 1f;
            }
        }

        // Animaciones de salto y caída
        if (!CheckGround.isGrounded && !isDashing)
        {
            if (rb.linearVelocity.y > 0)
            {
                animator.SetBool("IsJump", true);
                animator.SetBool("IsFalling", false);
            }
            else if (rb.linearVelocity.y < 0)
            {
                animator.SetBool("IsJump", false);
                animator.SetBool("IsFalling", true);
            }
        }
        else if (!isDashing)
        {
            animator.SetBool("IsJump", false);
            animator.SetBool("IsFalling", false);
        }

        // Reset dash al tocar el suelo
        if (CheckGround.isGrounded && !isDashing)
        {
            canDash = true;

            animator.SetBool("IsFalling", false);
            animator.SetBool("IsJump", false);
        }
    }

    private void Move()
    {
        if (Input.GetKey("d") || Input.GetKey("right"))
        {
            spriteRenderer.flipX = false;
            animator.SetBool("IsRunning", true);
            rb.linearVelocity = new Vector2(stats.moveSpeed, rb.linearVelocity.y);
        }
        else if (Input.GetKey("a") || Input.GetKey("left"))
        {
            spriteRenderer.flipX = true;
            animator.SetBool("IsRunning", true);
            rb.linearVelocity = new Vector2(-stats.moveSpeed, rb.linearVelocity.y);
        }
        else
        {
            animator.SetBool("IsRunning", false);
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }
    }

    private void Run()
    {
        float speed = Input.GetKey(KeyCode.LeftShift) ? stats.runSpeed : stats.moveSpeed;
        rb.linearVelocity = new Vector2(
            rb.linearVelocity.x > 0 ? speed : (rb.linearVelocity.x < 0 ? -speed : 0),
            rb.linearVelocity.y
        );
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && CheckGround.isGrounded)
        {
            rb.AddForce(Vector2.up * stats.jumpForce, ForceMode2D.Impulse);
            animator.SetBool("IsJump", true);
        }
    }

    void Update()
    {
        HandleDash();
    }

    private void HandleDash()
    {
        if (Input.GetMouseButtonDown(1) && canDash) // click derecho
        {
            float dashX = Input.GetAxisRaw("Horizontal");
            float dashY = Input.GetAxisRaw("Vertical");

            if (dashX == 0 && dashY == 0)
            {
                dashX = spriteRenderer.flipX ? -1 : 1;
            }

            if (dashY < 0) return;

            Vector2 dashDir = new Vector2(dashX, dashY).normalized;

            RaycastHit2D hit = Physics2D.Raycast(rb.position, dashDir, stats.distanceRaycastDash, obstacleLayer);
            if (hit.collider != null) return;

            dashStart = rb.position;
            dashEnd = rb.position + dashDir * stats.dashDistance;

            isDashing = true;
            canDash = false;
            dashEndTime = Time.time + stats.dashDuration;

            rb.gravityScale = 0f;
            rb.linearVelocity = Vector2.zero;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDashing && ((1 << collision.gameObject.layer) & obstacleLayer) != 0)
        {
            CancelDash();
        }
    }

    private void CancelDash()
    {
        isDashing = false;
        rb.gravityScale = 1f;
        rb.linearVelocity = Vector2.zero;
    }
}

