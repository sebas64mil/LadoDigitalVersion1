using UnityEngine;
using System.Linq;

public class HeartbeatController : MonoBehaviour
{
    [Header("Heartbeat Settings")]
    public AudioSource heartbeatAudio;
    public float maxVolume = 1f;
    public float maxPitch = 1.5f;
    public float detectionRadius = 15f;  // rango en el que empieza a latir

    private EnemyController[] enemies;

    [Header("Alarm Settings")]
    public AudioSource alarmAudio;

    void Start()
    {
        enemies = FindObjectsByType<EnemyController>(FindObjectsSortMode.None);
        heartbeatAudio.volume = 0f;
    }

    void Update()
    {
        float nearestDistance = GetNearestEnemyDistance();

        bool anyChasing = enemies.Any(e => e.CurrentState is ChaseState);

        //  Sonido de alarma
        if (anyChasing)
        {
            if (!alarmAudio.isPlaying)
                alarmAudio.Play();
        }
        else
        {
            if (alarmAudio.isPlaying)
                alarmAudio.Stop();
        }

        //  Heartbeat normal (como ya lo tienes)
        if (nearestDistance <= detectionRadius)
        {
            if (!heartbeatAudio.isPlaying)
                heartbeatAudio.Play();

            float t = 1 - (nearestDistance / detectionRadius);
            heartbeatAudio.volume = Mathf.Lerp(0f, maxVolume, t);
            heartbeatAudio.pitch = Mathf.Lerp(1f, maxPitch, t);
        }
        else
        {
            heartbeatAudio.volume = Mathf.Lerp(heartbeatAudio.volume, 0f, Time.deltaTime * 2);

            if (heartbeatAudio.volume <= 0.01f && heartbeatAudio.isPlaying)
                heartbeatAudio.Stop();
        }
    }

    float GetNearestEnemyDistance()
    {
        float minDist = Mathf.Infinity;

        foreach (var enemy in enemies)
        {
            float dist = Vector3.Distance(transform.position, enemy.transform.position);
            if (dist < minDist)
                minDist = dist;
        }

        return minDist;
    }
}
