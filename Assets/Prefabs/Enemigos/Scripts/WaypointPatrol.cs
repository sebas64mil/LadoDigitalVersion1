using UnityEngine;
using System.Collections;
using System.Linq;

public class WaypointPatrol : EnemyState
{
    private bool isMoving = false;
    private WeightedWaypoint[] waypoints;

    // Tiempo que el enemigo espera al llegar a un punto
    private float waitTime = 2.5f;

    // Velocidad de rotación al mirar lados
    private float lookRotationSpeed = 90f; // grados por segundo
    private float lookAngle = 45f;         // ángulo que mira hacia los lados

    public WaypointPatrol(WeightedWaypoint[] waypoints)
    {
        this.waypoints = waypoints;
    }

    public override void Enter(EnemyController enemy)
    {
        isMoving = false;
    }

    public override void Update(EnemyController enemy)
    {
        // Si detecta al jugador → cambiar a persecución
        if (!enemy.isPatrol)
        {
            enemy.ChangeState(new ChaseState());
            return;
        }

        // Si no hay puntos o ya está en movimiento, no hacer nada
        if (waypoints == null || waypoints.Length == 0 || isMoving) return;

        // Elegir un waypoint aleatorio según su peso
        WeightedWaypoint targetWaypoint = GetRandomWeightedWaypoint();
        enemy.StartCoroutine(MoveToWaypoint(enemy, targetWaypoint.position));
    }

    public override void Exit(EnemyController enemy)
    {
        enemy.viewEnemy.ChangeToAnimation("Xmov", 0f);
    }

    // --- Corrutina principal de movimiento ---
    private IEnumerator MoveToWaypoint(EnemyController enemy, Vector3 target)
    {
        isMoving = true;

        // Animación de caminar
        enemy.viewEnemy.ChangeToAnimation("Xmov", 0.5f);
        enemy.DestinationEnemy(target);

        // Esperar hasta llegar al punto
        while (Vector3.Distance(enemy.transform.position, target) > 0.3f)
        {
            yield return null;
        }

        // Parar animación
        enemy.viewEnemy.ChangeToAnimation("Xmov", 0f);

        // --- Mirar a los lados ---
        yield return new WaitForSeconds(0.3f);
        yield return LookAround(enemy);

        // Esperar un momento antes de continuar
        yield return new WaitForSeconds(waitTime);

        isMoving = false;
    }

    // --- Movimiento de rotación izquierda-derecha ---
    private IEnumerator LookAround(EnemyController enemy)
    {
        Quaternion startRot = enemy.transform.rotation;

        // Girar a la izquierda
        Quaternion leftRot = Quaternion.Euler(0, enemy.transform.eulerAngles.y - lookAngle, 0);
        while (Quaternion.Angle(enemy.transform.rotation, leftRot) > 1f)
        {
            enemy.transform.rotation = Quaternion.RotateTowards(enemy.transform.rotation, leftRot, lookRotationSpeed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        // Girar a la derecha
        Quaternion rightRot = Quaternion.Euler(0, enemy.transform.eulerAngles.y + lookAngle * 2f, 0);
        while (Quaternion.Angle(enemy.transform.rotation, rightRot) > 1f)
        {
            enemy.transform.rotation = Quaternion.RotateTowards(enemy.transform.rotation, rightRot, lookRotationSpeed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        // Volver a la rotación original
        while (Quaternion.Angle(enemy.transform.rotation, startRot) > 1f)
        {
            enemy.transform.rotation = Quaternion.RotateTowards(enemy.transform.rotation, startRot, lookRotationSpeed * Time.deltaTime);
            yield return null;
        }
    }

    // --- Selección aleatoria ponderada ---
    private WeightedWaypoint GetRandomWeightedWaypoint()
    {
        float totalWeight = waypoints.Sum(wp => wp.weight);
        float randomValue = Random.value * totalWeight;
        float cumulative = 0f;

        foreach (var wp in waypoints)
        {
            cumulative += wp.weight;
            if (randomValue <= cumulative)
                return wp;
        }

        return waypoints[0]; // fallback
    }
}
