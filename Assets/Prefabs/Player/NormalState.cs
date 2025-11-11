using UnityEngine;

public class NormalState : PlayerState
{
    float moveX;
    float moveZ;

    public override void Enter(PlayerMove3D player)
    {
        player.isCrouching = false;
        player.capsule.height = player.playerStats.standHeight;
        player.capsule.center = player.GetColliderCenter();

        if (player.boxTrigger != null)
            player.boxTrigger.enabled = false;

        player.StopAllCoroutines();
        player.StartCoroutine(player.LerpCamera(player.GetCameraStandPos()));
    }

    public override void Update(PlayerMove3D player)
    {
        //------------------------
        // INPUT RUN
        //------------------------
        int runMode = GeneralPlayerSettingsManager.Instance.GetRunMode();
        bool shiftPressed = Input.GetKey(KeyCode.LeftShift);
        bool shiftDown = Input.GetKeyDown(KeyCode.LeftShift);
        bool hasStamina = player.staminaView.HasStamina();

        if (runMode == 0) // Toggle
        {
            if (shiftDown && hasStamina)
                player.runToggleState = !player.runToggleState;

            if (!hasStamina && player.runToggleState)
                player.runToggleState = false;

            player.isRunning = player.runToggleState;
        }
        else // Hold
        {
            player.isRunning = shiftPressed && hasStamina;
        }

        bool isMoving = Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f ||
                        Mathf.Abs(Input.GetAxis("Vertical")) > 0.1f;

        player.isRunning &= isMoving;

        //------------------------
        // INPUT CROUCH
        //------------------------
        int crouchMode = GeneralPlayerSettingsManager.Instance.GetCrouchMode();
        bool ctrlPressed = Input.GetKey(KeyCode.LeftControl);
        bool ctrlDown = Input.GetKeyDown(KeyCode.LeftControl);

        if (crouchMode == 0) // Toggle
        {
            if (ctrlDown && !player.isRunning)
            {
                player.crouchToggleState = !player.crouchToggleState;
                if (player.crouchToggleState)
                {
                    player.ChangeState(new CrouchState());
                }
                return;
            }
        }
        else // Hold
        {
            if (ctrlDown && !player.isRunning)
            {
                player.ChangeState(new CrouchState());
                return;
            }
        }

        //------------------------
        // INPUT LOOK
        //------------------------
        float mouseX = Input.GetAxis("Mouse X") * player.playerStats.mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * player.playerStats.mouseSensitivity * Time.deltaTime;

        player.xRotation -= mouseY;
        player.xRotation = Mathf.Clamp(player.xRotation, -90f, 90f);

        player.playerCamera.localRotation = Quaternion.Euler(player.xRotation, 0f, 0f);
        player.transform.Rotate(Vector3.up * mouseX);

        //------------------------
        // STORE MOVEMENT INPUT (for FixedUpdate)
        //------------------------
        moveX = Input.GetAxis("Horizontal");
        moveZ = Input.GetAxis("Vertical");

        //------------------------
        // CAMERA BOB / SLIDE
        //------------------------
        Vector3 targetCamPos;
        Vector2 m = new Vector2(moveX, moveZ);

        if (m.magnitude > 0.1f)
            targetCamPos = player.isRunning ? player.GetCameraRunPos() : player.GetCameraWalkPos();
        else
            targetCamPos = player.GetCameraStandPos();

        player.playerCamera.localPosition = Vector3.Lerp(
            player.playerCamera.localPosition,
            targetCamPos,
            Time.deltaTime * 8f
        );

        //------------------------
        // ANIMATION
        //------------------------
        float blendValue = 0f;
        if (m.magnitude > 0.1f)
            blendValue = player.isRunning ? 1f : 0.5f;

        float currentValue = player.animator.GetFloat("StaigthX");
        float smoothValue = Mathf.Lerp(currentValue, blendValue, Time.deltaTime * 8f);
        player.animator.SetFloat("StaigthX", smoothValue);
    }

    public override void FixedUpdate(PlayerMove3D player)
    {
        // FÍSICA
        Vector3 move = player.transform.right * moveX + player.transform.forward * moveZ;
        float speed = player.isRunning ? player.playerStats.runSpeed : player.playerStats.speed;

        player.rb.MovePosition(player.rb.position + move * speed * Time.fixedDeltaTime);

        // Pasos sonido
        player.HandleWalkSteps(move.magnitude);
    }
}
