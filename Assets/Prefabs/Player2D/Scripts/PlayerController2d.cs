using UnityEngine;

public class PlayerController2d : MonoBehaviour
{
    public PlayerStats2D stats;
    public LayerMask obstacleLayer;

     public Color originalColor;
    public Color dashColor;

    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public Animator animator;
    [HideInInspector] public SpriteRenderer spriteRenderer;
    [HideInInspector] public bool hasDashed = false;
    [HideInInspector] public bool climbToggleState = false;
    [HideInInspector] public bool climbActive = false;


    private PlayerStateBase2d currentState;

    [HideInInspector]public PlayerSFXController2D sfxController;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        ChangeState(new NormalMove2D());
        sfxController = GetComponent<PlayerSFXController2D>();
    }

    void Update()
    {
        currentState?.UpdateState(this);
    }

    void FixedUpdate()
    {
        currentState?.FixedUpdateState(this);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        currentState?.OnCollisionEnter2D(collision);
    }

    public void ChangeState(PlayerStateBase2d newState)
    {
        currentState?.ExitState(this);
        currentState = newState;
        currentState.EnterState(this);
    }

    // --- MÉTODOS DE DETECCIÓN CON RAYCAST ---
    public bool IsTouchingWallRight()
    {
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            Vector2.right,
            stats.distanceRaycastClimb,
            obstacleLayer
        );
        return hit.collider != null;
    }

    public bool IsTouchingWallLeft()
    {
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            Vector2.left,
            stats.distanceRaycastClimb,
            obstacleLayer
        );
        return hit.collider != null;
    }

    public bool IsTouchingWall()
    {
        return IsTouchingWallRight() || IsTouchingWallLeft();
    }

    void OnDrawGizmosSelected()
    {
        if (stats == null) return;

        Gizmos.color = Color.yellow;
        Vector3 pos = transform.position ;
        float d = stats.distanceRaycastClimb;

        Gizmos.DrawLine(pos, pos + Vector3.right * d);
        Gizmos.DrawLine(pos, pos + Vector3.left * d);
    }

    
}
