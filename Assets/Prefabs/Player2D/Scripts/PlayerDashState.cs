using UnityEngine;

public class PlayerDashState : PlayerStateBase2d
{
    private bool isDashing = false;
    private float dashEndTime;
    private Vector2 dashStart, dashEnd;

    private LayerMask obstacleLayer;
    private PlayerStats2D stats;
    private PlayerController2d playerRef;

    public PlayerDashState(LayerMask obstacleLayer, PlayerStats2D stats)
    {
        this.obstacleLayer = obstacleLayer;
        this.stats = stats;
    }

    public override void EnterState(PlayerController2d player)
    {
        playerRef = player;


        player.spriteRenderer.color = player.dashColor;

        player.animator.SetBool("IsDashing", true);

        player.sfxController.PlayDash();
        // Dirección del dash
        float dashX = Input.GetAxisRaw("Horizontal");
        float dashY = Input.GetAxisRaw("Vertical");

        if (dashX == 0 && dashY == 0)
            dashX = player.spriteRenderer.flipX ? -1 : 1;

        Vector2 dashDir = new Vector2(dashX, dashY).normalized;

        // Evitar colisión inmediata
        RaycastHit2D hit = Physics2D.Raycast(player.rb.position, dashDir, stats.distanceRaycastDash, obstacleLayer);
        if (hit.collider != null)
        {
            isDashing = false;
            return;
        }

        dashStart = player.rb.position;
        dashEnd = player.rb.position + dashDir * stats.dashDistance;

        isDashing = true;
        dashEndTime = Time.time + stats.dashDuration;

        player.rb.gravityScale = 0f;
        player.rb.linearVelocity = Vector2.zero;
    }

    public override void UpdateState(PlayerController2d player)
    {

        shadowsDash.shadow.Sombras_Skill();

        if (!isDashing)
        {
            // Termina dash, volver a movimiento normal
            player.rb.gravityScale = stats.defaultGravity;
            player.ChangeState(new NormalMove2D());
        }
    }

    public override void FixedUpdateState(PlayerController2d player)
    {
        if (isDashing)
        {
            float t = 1 - ((dashEndTime - Time.time) / stats.dashDuration);
            player.rb.MovePosition(Vector2.Lerp(dashStart, dashEnd, t));

            if (Time.time >= dashEndTime)
            {
                isDashing = false;
                player.rb.gravityScale = stats.defaultGravity;
            }
        }
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDashing && ((1 << collision.gameObject.layer) & obstacleLayer) != 0)
        {
            CancelDash();
        }
    }

    public override void ExitState(PlayerController2d player)
    {

        player.rb.gravityScale = stats.defaultGravity;
        player.animator.SetBool("IsDashing", false);
    }

    private void CancelDash()
    {
        isDashing = false;
        playerRef.rb.gravityScale = stats.defaultGravity;
        playerRef.rb.linearVelocity = Vector2.zero;
    }
}
