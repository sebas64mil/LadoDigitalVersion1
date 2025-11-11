using UnityEngine;

public class ChaseState : EnemyState
{
    private float loseSightTimer = 0f;
    private float loseSightDuration; // Segundos que seguirá persiguiendo tras perder de vista al jugador

    public override void Enter(EnemyController enemy)
    {
        enemy.viewEnemy.ChangeToAnimation("Xmov", 1f);
        loseSightTimer = 0f;
        enemy.SpeedChaseEnemy();
        loseSightDuration = enemy.enemyModel.TimerDurationChase;
    }

    public override void Update(EnemyController enemy)
    {
        if (enemy.player == null) return;

        // Seguir al jugador
        enemy.DestinationEnemy(enemy.player.position);

        if (!enemy.isPatrol) // Lo ve
        {
            loseSightTimer = 0f; // Reseteo mientras lo tengo a la vista
        }
        else // No lo ve
        {
            loseSightTimer += Time.deltaTime;

            if (loseSightTimer >= loseSightDuration)
            {
                //  Volver al modo de patrulla con pesos
                enemy.ChangeState(new WaypointPatrol(enemy.weightedWaypoints));
            }
        }
    }

    public override void Exit(EnemyController enemy)
    {
        enemy.viewEnemy.ChangeToAnimation("Xmov", 0f);
        enemy.SpeedEnemy();
    }
}
