using UnityEngine;
using System.Collections;

public class ClimbState2D : PlayerStateBase2d
{
    private float climbTimer = 0f;
    private bool isExhausted = false;
    private bool isClimbJump = false;
    private bool wasClimbing = false;
    private Coroutine climbJumpCoroutine;

    public override void EnterState(PlayerController2d player)
    {
        player.rb.gravityScale = 0f;
        player.animator.SetBool("IsSliding", true);
        player.animator.SetBool("IsClimbing", false);
        wasClimbing = false;
        player.sfxController.PlayClimb();

    }

    public override void UpdateState(PlayerController2d player)
    {
        if (isClimbJump) return;

        bool isTouchingWall = false;
        bool isClimbing = false;

        // --- Detectar pared ---
        Vector2 direction = player.spriteRenderer.flipX ? Vector2.left : Vector2.right;
        RaycastHit2D hit = Physics2D.Raycast(
            player.rb.position,
            direction,
            player.stats.distanceRaycastClimb,
            player.obstacleLayer
        );
        isTouchingWall = hit.collider != null;

        // --- Sistema Toggle vs Hold ---
        int mode = GeneralPlayerSettingsManager.Instance.GetClimbMode();

        if (mode == 0) // TOGGLE
        {

            if (Input.GetMouseButtonDown(0))
            {
                player.climbActive = !player.climbActive;
                if (!player.climbActive)
                {
                    player.ChangeState(new NormalMove2D());
                    return;
                }
            }

            if (!player.climbActive)
            {
                player.ChangeState(new NormalMove2D());
                return;
            }
        }

        else // HOLD
        {
            // En Hold, la intención depende de si el botón está presionado
            player.climbActive = Input.GetMouseButton(0);

            if (!player.climbActive)
            {
                player.ChangeState(new NormalMove2D());
                return;
            }
        }

        // --- Escalar (W / S mientras climbActive) ---
        if (player.climbActive && !isExhausted && isTouchingWall)
        {
            climbTimer += Time.deltaTime;
            if (climbTimer >= player.stats.timeToClimb)
                isExhausted = true;

            float verticalInput = 0f;
            float currentClimbSpeed = climbTimer >= player.stats.timeToClimb * 0.8f
                ? player.stats.climbSpeed * 0.5f
                : player.stats.climbSpeed;

            if (Input.GetKey(KeyCode.W))
                verticalInput = currentClimbSpeed;
            else if (Input.GetKey(KeyCode.S))
                verticalInput = -currentClimbSpeed;

            player.rb.gravityScale = 0f;
            player.rb.linearVelocity = new Vector2(0, verticalInput);

            if (Mathf.Abs(verticalInput) > 0.05f)
            {
                player.animator.SetBool("IsClimbing", true);
                player.animator.SetBool("IsSliding", false);
            }
            else
            {
                player.animator.SetBool("IsClimbing", false);
                player.animator.SetBool("IsSliding", true);
            }

            isClimbing = true;
        }
        // --- Slide ---
        else if (isTouchingWall)
        {
            player.rb.gravityScale = player.stats.defaultGravity;

            if (player.rb.linearVelocity.y < -player.stats.wallSlideSpeed)
                player.rb.linearVelocity = new Vector2(player.rb.linearVelocity.x, -player.stats.wallSlideSpeed);

            player.animator.SetBool("IsClimbing", false);
            player.animator.SetBool("IsSliding", true);
        }
        // --- Jump away (si estaba escalando y suelta pared con W activo) ---
        else if (wasClimbing && !isClimbJump && player.climbActive && Input.GetKey(KeyCode.W))
        {
            climbJumpCoroutine = player.StartCoroutine(
                ClimbJumpHorizontal(player.spriteRenderer.flipX ? Vector2.left : Vector2.right, player)
            );
            return;
        }
        else
        {
            player.ChangeState(new NormalMove2D());
            return;
        }

        wasClimbing = isClimbing;
    }

    private IEnumerator ClimbJumpHorizontal(Vector2 moveDir, PlayerController2d player)
    {
        isClimbJump = true;
        yield return new WaitForSeconds(player.stats.climbJumpDelay);

        float elapsed = 0f;
        Vector3 startPos = player.rb.transform.position;
        Vector3 endPos = startPos + (Vector3)(moveDir * player.stats.climbJumpDistance);

        while (elapsed < player.stats.climbJumpDuration)
        {
            player.rb.MovePosition(Vector3.Lerp(startPos, endPos, elapsed / player.stats.climbJumpDuration));
            elapsed += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        player.rb.MovePosition(endPos);
        isClimbJump = false;
        player.ChangeState(new NormalMove2D());
    }

    public override void ExitState(PlayerController2d player)
    {
        player.animator.SetBool("IsSliding", false);
        player.animator.SetBool("IsClimbing", false);
        player.rb.gravityScale = player.stats.defaultGravity;
        player.sfxController.StopClimb();
        ResetClimb(player);
    }

    private void ResetClimb(PlayerController2d player)
    {
        climbTimer = 0f;
        isExhausted = false;
        isClimbJump = false;
        if (climbJumpCoroutine != null)
        {
            player.StopCoroutine(climbJumpCoroutine);
            climbJumpCoroutine = null;
        }
        wasClimbing = false;
        // limpiar flags en player
        player.climbActive = false;
    }

    // Keep ForceCancelClimb but ensure it clears the player's flags
    public void ForceCancelClimb(PlayerController2d player)
    {
        if (climbJumpCoroutine != null)
            player.StopCoroutine(climbJumpCoroutine);

        player.rb.gravityScale = player.stats.defaultGravity;
        player.rb.linearVelocity = Vector2.zero;
        player.animator.SetBool("IsSliding", false);
        player.animator.SetBool("IsClimbing", false);
        ResetClimb(player);
    }
}
