using UnityEngine;


[CreateAssetMenu(fileName = "PlayerStats3D", menuName = "Config/PlayerStats3D")]
public class PlayerStats3d : ScriptableObject
{
    [Header("Movimiento")]
    [Tooltip("Velocidad normal de movimiento")]
    public float speed = 5f;
    [Tooltip("Velocidad al correr de pie")]
    public float runSpeed = 8f;
    [Tooltip("Sensibilidad del ratón para rotación")]
    public float mouseSensitivity = 100f;

    [Header("Agacharse")]
    [Tooltip("Altura del CapsuleCollider al agacharse")]
    public float crouchHeight = 1f;
    [Tooltip("Altura del CapsuleCollider estando de pie")]
    public float standHeight = 2f;
    [Tooltip("Velocidad de movimiento al estar agachado")]
    public float crouchSpeed = 2.5f;
    [Tooltip("Posición Y de la cámara cuando el jugador está agachado")]
    public float cameraCrouchY = 0.5f;

    [Header("Sigilo / Paredes")]
    [Tooltip("Offset al cambiar de segmento para no quedar pegado al punto A o B")]
    public float segmentOffset = 0.8f;
    [Tooltip("Velocidad para la transicion entre los segmentos")]
    public float segmentTransitionSpeed = 5f;



    [Header("IK parameters")]
    [Tooltip("boleano de que si el ik esta activado o no")]
    public bool ikActive = false;
    [Tooltip("Distancia del raycast para detectar la pared")]
    public float DistanceRaycastCouch = 0.3f;

}
