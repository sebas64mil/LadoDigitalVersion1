using UnityEngine;
using UnityEngine.Rendering;

[System.Serializable]
public class EnemyModel
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 2f;
    [SerializeField] private float speedChase = 4f;
    [SerializeField] private float distanceToAttack = 3f;

    [Header("Vision Settings")]
    [SerializeField] private float visionRange = 10f;
    [SerializeField] private float visionAngle = 45f;
    [SerializeField] private float scanSpeed = 2f;
    [SerializeField] public float DetectionRadiusClose = 1f;

    [Header("Chase Settings")]
    [SerializeField] private float timerDurationChase = 6f; // Tiempo que sigue al jugador tras perderlo de vista

    // --- Getters & Setters ---
    public float Speed
    {
        get => speed;
        set { if (value >= 0) speed = value; }
    }

    public float SpeedChase
    {
        get => speedChase;
        set { if (value >= 0) speedChase = value; }
    }

    public float DistanceToAttack
    {
        get => distanceToAttack;
        set { if (value >= 0) distanceToAttack = value; }
    }

    public float VisionRange
    {
        get => visionRange;
        set { if (value >= 0) visionRange = value; }
    }

    public float VisionAngle
    {
        get => visionAngle;
        set { if (value >= 0) visionAngle = value; }
    }

    public float ScanSpeed
    {
        get => scanSpeed;
        set { if (value >= 0) scanSpeed = value; }
    }

    public float TimerDurationChase
    {
        get => timerDurationChase;
        set { if (value >= 0) timerDurationChase = value; }
    }


    public float DetectionRadius
    {
        get => DetectionRadiusClose;
        set { if (value >= 0) DetectionRadiusClose = value; }

    }
}
