using UnityEngine;

public class NormalMove2D : PlayerStateBase2d
{
    public override void EnterState(PlayerController2d player)
    {
        player.animator.SetBool("IsRunning", false);
        player.animator.SetBool("IsJump", false);
        player.animator.SetBool("IsFalling", false);
    }

    public override void UpdateState(PlayerController2d player)
    {

        // --- Dash ---
        if (Input.GetMouseButtonDown(1) && !player.hasDashed)
        {
            player.hasDashed = true;
            player.ChangeState(new PlayerDashState(player.obstacleLayer, player.stats));
            return;
        }

        // --- Jump ---
        if (Input.GetKeyDown(KeyCode.Space) && CheckGround.isGrounded)
        {
            player.rb.AddForce(Vector2.up * player.stats.jumpForce, ForceMode2D.Impulse);
            player.animator.SetBool("IsJump", true);
            player.sfxController.PlayJump();
        }

        // --- DETECCIÓN DE PARED ---
        bool isTouchingWall = player.IsTouchingWall();

        int mode = GeneralPlayerSettingsManager.Instance.GetClimbMode();

        if (mode == 0) // TOGGLE
        {
            if (isTouchingWall && Input.GetMouseButtonDown(0))
            {
                // Alterna la intención de trepar y marca para consumir el mismo click
                player.climbActive = !player.climbActive;

                if (player.climbActive)
                {
                    player.ChangeState(new ClimbState2D());
                    return;
                }
            }
        }
        else // HOLD
        {
            if (isTouchingWall && Input.GetMouseButton(0))
            {
                player.climbActive = true;
                player.ChangeState(new ClimbState2D());
                return;
            }
        }



        if (CheckGround.isGrounded && Mathf.Abs(player.rb.linearVelocity.y) < 0.1f)
        {
            player.spriteRenderer.color = player.originalColor;
            player.hasDashed = false;
        }


        // --- Animaciones salto/caída ---
        if (!CheckGround.isGrounded)
        {
            if (player.rb.linearVelocity.y > 0)
            {
                player.animator.SetBool("IsJump", true);
                player.animator.SetBool("IsFalling", false);
            }
            else
            {
                player.animator.SetBool("IsJump", false);
                player.animator.SetBool("IsFalling", true);
            }
        }
        else
        {
            player.animator.SetBool("IsJump", false);
            player.animator.SetBool("IsFalling", false);
        }
    }

    public override void FixedUpdateState(PlayerController2d player)
    {
        bool isMoving = false;

        if (Input.GetKey("d") || Input.GetKey("right"))
        {
            player.spriteRenderer.flipX = false;
            player.animator.SetBool("IsRunning", true);
            player.rb.linearVelocity = new Vector2(player.stats.moveSpeed, player.rb.linearVelocity.y);
            isMoving = true;
        }
        else if (Input.GetKey("a") || Input.GetKey("left"))
        {
            player.spriteRenderer.flipX = true;
            player.animator.SetBool("IsRunning", true);
            player.rb.linearVelocity = new Vector2(-player.stats.moveSpeed, player.rb.linearVelocity.y);
            isMoving = true;
        }
        else
        {
            player.animator.SetBool("IsRunning", false);
            player.rb.linearVelocity = new Vector2(0, player.rb.linearVelocity.y);
        }

        // --- Sonido de caminar ---
        if (isMoving && CheckGround.isGrounded)
            player.sfxController.PlayWalk();
        else
            player.sfxController.StopWalk();

        // --- Better Jump ---
        if (player.stats.betterJump)
        {
            if (player.rb.linearVelocity.y < 0)
            {
                player.rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (player.stats.fallMultiplier - 1) * Time.deltaTime;
            }
            else if (player.rb.linearVelocity.y > 0 && !Input.GetKey(KeyCode.Space))
            {
                player.rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (player.stats.lowJumpMultiplier - 1) * Time.deltaTime;
            }
        }
    }

}
