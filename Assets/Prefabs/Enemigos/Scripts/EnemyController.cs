using UnityEngine;
using UnityEngine.AI;
using System.Linq;

[System.Serializable]
public class WeightedWaypoint
{
    public Vector3 position;
    [Range(0f, 1f)] public float weight = 1f; // peso relativo (mayor = más probabilidades)
}

public class EnemyController : MonoBehaviour
{
    private EnemyView enemyView;
    public EnemyModel enemyModel;
    public Transform player;
    public NavMeshAgent agent { get; private set; }

    private IEnemyState currentState;

    [Header("Patrol Setup")]
    public WeightedWaypoint[] weightedWaypoints; // 👈 ahora con pesos
    [HideInInspector] public bool isPatrol = true;
    public LayerMask visionMask;

    // Detección con raycast dinámico
    [SerializeField] private float raycastHeight = 1.7f;

    private void Start()
    {
        enemyView = GetComponent<EnemyView>();
        agent = GetComponent<NavMeshAgent>();
        SpeedEnemy();

        // Estado inicial: patrulla con pesos
        ChangeState(new WaypointPatrol(weightedWaypoints));
    }

    private void Update()
    {
        CanSeePlayer();
        currentState?.Update(this);
    }

    // --- Cambio de estados ---
    public void ChangeState(IEnemyState newState)
    {
        currentState?.Exit(this);
        currentState = newState;
        currentState.Enter(this);
    }

    // --- Movimiento NavMesh ---
    public void DestinationEnemy(Vector3 target)
    {
        agent.SetDestination(target);
    }

    public void SpeedEnemy()
    {
        agent.speed = enemyModel.Speed;
    }

    public void SpeedChaseEnemy()
    {
        agent.speed = enemyModel.SpeedChase;
    }

    // --- Sistema de detección del jugador ---
    public void CanSeePlayer()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        bool playerDetected = false;

        // --- Verificación si el jugador corre ---
        bool playerIsRunning = false;
        var staminaView = player.GetComponent<PlayerStaminaView>();
        if (staminaView != null)
        {
            playerIsRunning = Input.GetKey(KeyCode.LeftShift) && staminaView.HasStamina();
        }

        // --- Si el jugador corre cerca ---
        if (playerIsRunning && distanceToPlayer <= enemyModel.VisionRange)
        {
            playerDetected = true;
        }
        // --- Si el jugador está muy cerca ---
        else if (distanceToPlayer <= enemyModel.DetectionRadiusClose)
        {
            playerDetected = true;
        }
        else
        {
            // --- Raycasts múltiples dentro del cono ---
            Vector3 originHigh = transform.position + Vector3.up * raycastHeight;
            Vector3 originLow = transform.position + Vector3.up * (raycastHeight * 0.5f);

            int raysPerLevel = 5;
            float halfAngle = enemyModel.VisionAngle / 2f;

            for (int i = 0; i < raysPerLevel; i++)
            {
                float angleStep = Mathf.Lerp(-halfAngle, halfAngle, (float)i / (raysPerLevel - 1));
                Quaternion rayRot = Quaternion.Euler(0, angleStep, 0);
                Vector3 dir = rayRot * transform.forward;

                // Nivel alto
                if (Physics.Raycast(originHigh, dir, out RaycastHit hitHigh, enemyModel.VisionRange, visionMask))
                {
                    Debug.DrawRay(originHigh, dir * hitHigh.distance, Color.red);
                    if (hitHigh.transform.CompareTag("Player"))
                    {
                        playerDetected = true;
                        break;
                    }
                }
                else
                {
                    Debug.DrawRay(originHigh, dir * enemyModel.VisionRange, Color.yellow);
                }

                // Nivel bajo
                if (Physics.Raycast(originLow, dir, out RaycastHit hitLow, enemyModel.VisionRange, visionMask))
                {
                    Debug.DrawRay(originLow, dir * hitLow.distance, Color.green);
                    if (hitLow.transform.CompareTag("Player"))
                    {
                        playerDetected = true;
                        break;
                    }
                }
                else
                {
                    Debug.DrawRay(originLow, dir * enemyModel.VisionRange, Color.cyan);
                }
            }
        }

        // --- Resultado final ---
        isPatrol = !playerDetected;
    }

    public void DestinationEnemey(Vector3 target)
    {
        if (agent == null) return;

        agent.isStopped = false;
        agent.SetDestination(target);
    }


    // --- Gizmos visuales ---
    private void OnDrawGizmos()
    {
        if (enemyModel == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, enemyModel.VisionRange);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, enemyModel.DetectionRadiusClose);

        // Cono de visión
        Vector3 forward = transform.forward;
        Quaternion leftRot = Quaternion.AngleAxis(-enemyModel.VisionAngle / 2f, Vector3.up);
        Quaternion rightRot = Quaternion.AngleAxis(enemyModel.VisionAngle / 2f, Vector3.up);

        Vector3 leftDir = leftRot * forward;
        Vector3 rightDir = rightRot * forward;

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, leftDir * enemyModel.VisionRange);
        Gizmos.DrawRay(transform.position, rightDir * enemyModel.VisionRange);

        // Waypoints visibles
        if (weightedWaypoints != null && weightedWaypoints.Length > 0)
        {
            Gizmos.color = Color.magenta;
            for (int i = 0; i < weightedWaypoints.Length; i++)
            {
                Vector3 wp = weightedWaypoints[i].position;
                Gizmos.DrawSphere(wp, 0.2f);

                // Línea al siguiente (loop opcional)
                if (i < weightedWaypoints.Length - 1)
                    Gizmos.DrawLine(wp, weightedWaypoints[i + 1].position);
                else
                    Gizmos.DrawLine(wp, weightedWaypoints[0].position);
            }
        }
    }

    // --- Accesores ---
    public EnemyView viewEnemy => enemyView;
    public IEnemyState CurrentState => currentState;
}
