using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats2D", menuName = "Config/PlayerStats2D")]
public class PlayerStats2D : ScriptableObject
{
    [Header("Movimiento")]
    [Tooltip("Velocidad de movimiento normal del jugador.")]
    public float moveSpeed = 2f;

    [Tooltip("Velocidad de carrera del jugador.")]
    public float runSpeed = 4f;

    [Header("Salto")]
    [Tooltip("Fuerza del salto del jugador.")]
    public float jumpForce = 5f;

    [Tooltip("Activa el salto mejorado con caída más rápida.")]
    public bool betterJump = true;

    [Tooltip("Multiplicador de gravedad cuando el jugador cae.")]
    public float fallMultiplier = 2.5f;

    [Tooltip("Multiplicador de gravedad cuando el jugador hace un salto corto.")]
    public float lowJumpMultiplier = 2f;

    [Header("Dash")]
    [Tooltip("Distancia que recorre el jugador al hacer dash.")]
    public float dashDistance = 8f;

    [Tooltip("Duración del dash en segundos.")]
    public float dashDuration = 0.2f;

    [Tooltip("Distancia del raycast para detectar obstáculos al dashar.")]
    public float distanceRaycastDash = 0.2f;

    [Header("Climb")]
    [Tooltip("Distancia del raycast para detectar paredes escalables.")]
    public float distanceRaycastClimb = 0.2f;

    [Tooltip("Velocidad de escalada del jugador.")]
    public float climbSpeed = 2f;

    [Tooltip("Tiempo máximo que el jugador puede escalar antes de agotarse.")]
    public float timeToClimb = 3f;

    [Header("Climb Jump")]
    [Tooltip("Tiempo de espera antes de aplicar el impulso horizontal al hacer un climb jump.")]
    public float climbJumpDelay = 0.2f;

    [Tooltip("Distancia horizontal del climb jump.")]
    public float climbJumpDistance = 1f;

    [Tooltip("Duración del movimiento horizontal del climb jump.")]
    public float climbJumpDuration = 0.15f;

    [Tooltip("Impulso vertical al terminar de escalar antes de soltar la pared.")]
    public float climbEndImpulse = 10f;

    [Header("Wall Slide")]
    [Tooltip("Velocidad máxima de caída al deslizarse por una pared.")]
    public float wallSlideSpeed = 2f;

    [Header("Físicas")]
    [Tooltip("Valor base de la gravedad del jugador (para restaurar después de escalar).")]
    public float defaultGravity = 1f;


}

