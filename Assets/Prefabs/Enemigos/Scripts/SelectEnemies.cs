using UnityEngine;

[System.Serializable]
public class EnemyConfig
{
    [Header("Movimiento")]
    public float Speed = 2f;
    public float SpeedChase = 5f;

    [Header("Comportamiento")]
    public float TimerDurationChase = 4f;
    public float ScanSpeed = 3f;

    [Header("Detección")]
    public float VisionAngle = 50f;
    public float VisionRange = 12f;
    public float DetectionRadiusClose = 5f;
}

public class SelectEnemies : MonoBehaviour
{
    public enum Enemies { Guardia, Cyborg }

    [Header("Selección actual")]
    public Enemies enemySelected;

    [Header("Referencias visuales")]
    public Animator animator;
    public GameObject[] ListEnemies;
    public Avatar[] EnemiesAvatar;

    [Header("Configuraciones por tipo de enemigo (en el mismo orden que el enum)")]
    public EnemyConfig[] enemyConfigs;

    private EnemyController enemyController;

    void Start()
    {
        enemyController = GetComponent<EnemyController>();
        ApplyEnemySelection();
    }


    public void ApplyEnemySelection()
    {
        int index = (int)enemySelected;

        if (ListEnemies == null || index >= ListEnemies.Length)
        {
            Debug.LogWarning(" ListEnemies no configurado correctamente.");
            return;
        }

        if (enemyConfigs == null || index >= enemyConfigs.Length)
        {
            Debug.LogWarning(" enemyConfigs no configurado correctamente.");
            return;
        }

        // Desactivar todos los modelos
        foreach (GameObject enemy in ListEnemies)
            if (enemy != null)
                enemy.SetActive(false);

        // Activar el modelo correspondiente
        ListEnemies[index].SetActive(true);

        // Cambiar avatar de animación
        if (EnemiesAvatar != null && index < EnemiesAvatar.Length)
            animator.avatar = EnemiesAvatar[index];

        // Aplicar configuración del enemigo
        EnemyConfig config = enemyConfigs[index];
        enemyController.enemyModel.Speed = config.Speed;
        enemyController.enemyModel.SpeedChase = config.SpeedChase;
        enemyController.enemyModel.TimerDurationChase = config.TimerDurationChase;
        enemyController.enemyModel.ScanSpeed = config.ScanSpeed;
        enemyController.enemyModel.VisionAngle = config.VisionAngle;
        enemyController.enemyModel.VisionRange = config.VisionRange;
        enemyController.enemyModel.DetectionRadiusClose = config.DetectionRadiusClose;
    }
}
