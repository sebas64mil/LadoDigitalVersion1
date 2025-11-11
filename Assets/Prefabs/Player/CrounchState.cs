using UnityEngine;

public class CrouchState : PlayerState
{
    private bool wantsToStand = false;
    private float moveX;
    private float moveZ;

    public override void Enter(PlayerMove3D player)
    {
        player.isCrouching = true;
        wantsToStand = false;

        player.animator.SetBool("IsCrounch", true);
        player.StartCrouchBreathing();

        // Collider
        player.capsule.height = player.playerStats.crouchHeight;
        float centerOffset = (player.playerStats.standHeight - player.playerStats.crouchHeight) / 2f;
        player.capsule.center = player.GetColliderCenter() - new Vector3(0, centerOffset, 0);

        player.navmeshObstacle.height = player.capsule.height;
        player.navmeshObstacle.center = player.capsule.center;

        if (player.boxTrigger != null)
            player.boxTrigger.enabled = true;

        // Cámara
        Vector3 crouchPos = new Vector3(
            player.GetCameraStandPos().x,
            player.playerStats.cameraCrouchY,
            player.GetCameraStandPos().z
        );

        player.StopAllCoroutines();
        player.StartCoroutine(player.LerpCamera(crouchPos));
    }

    public override void Exit(PlayerMove3D player)
    {
        player.isCrouching = false;
        wantsToStand = false;

        player.animator.SetBool("IsCrounch", false);
        player.StopCrouchBreathing();

        player.capsule.height = player.playerStats.standHeight;
        player.capsule.center = player.GetColliderCenter();

        if (player.boxTrigger != null)
            player.boxTrigger.enabled = false;

        player.StopAllCoroutines();
        player.StartCoroutine(player.LerpCamera(player.GetCameraStandPos()));
    }

    public override void Update(PlayerMove3D player)
    {
        //-------------------------------------
        // INPUT DE SALIR DEL CROUCH
        //-------------------------------------
        int crouchMode = GeneralPlayerSettingsManager.Instance.GetCrouchMode();

        if (crouchMode == 1) // HOLD
        {
            if (!Input.GetKey(KeyCode.LeftControl))
                wantsToStand = true;
        }
        else // TOGGLE
        {
            if (Input.GetKeyDown(KeyCode.LeftControl))
                wantsToStand = !wantsToStand;
        }

        if (wantsToStand && !player.hayObstaculoArriba)
        {
            player.crouchToggleState = false;
            player.ChangeState(new NormalState());
            return;
        }

        //-------------------------------------
        // INPUT CAMERA / LOOK
        //-------------------------------------
        float mouseX = Input.GetAxis("Mouse X") * player.playerStats.mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * player.playerStats.mouseSensitivity * Time.deltaTime;

        player.xRotation -= mouseY;
        player.xRotation = Mathf.Clamp(player.xRotation, -90f, 90f);

        player.playerCamera.localRotation = Quaternion.Euler(player.xRotation, 0f, 0f);
        player.transform.Rotate(Vector3.up * mouseX);

        //-------------------------------------
        // STORE MOVEMENT INPUT (for FixedUpdate)
        //-------------------------------------
        moveX = Input.GetAxis("Horizontal");
        moveZ = Input.GetAxis("Vertical");

        //-------------------------------------
        // ANIMACIÓN
        //-------------------------------------
        Vector3 move = new Vector3(moveX, 0, moveZ);
        float blendValue = move.magnitude > 0.1f ? 1f : 0f;

        player.animator.SetFloat("CrounchX", blendValue, 0.1f, Time.deltaTime);
    }

    public override void FixedUpdate(PlayerMove3D player)
    {
        //-------------------------------------
        // MOVIMIENTO FÍSICO
        //-------------------------------------
        Vector3 move = player.transform.right * moveX + player.transform.forward * moveZ;
        player.rb.MovePosition(player.rb.position + move * player.playerStats.crouchSpeed * Time.fixedDeltaTime);

        player.HandleWalkSteps(move.magnitude);
    }
}
