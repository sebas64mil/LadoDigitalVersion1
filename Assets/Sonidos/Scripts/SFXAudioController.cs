using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SFXAudioController : MonoBehaviour
{
    private AudioSource audioSource;

    [Header("Pitch Randomization")]
    public bool enableRandomPitch = true;
    [Range(-3f, 3f)] public float minPitch = 0.9f;
    [Range(-3f, 3f)] public float maxPitch = 1.1f;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        // Sonido 3D por defecto
        audioSource.playOnAwake = false;
    }

    public void Play(AudioClip clip, bool randomPitch = true)
    {
        audioSource.clip = clip;

        audioSource.pitch = (enableRandomPitch && randomPitch)
            ? Random.Range(minPitch, maxPitch)
            : 1f;

        audioSource.Play();
    }

    public void Stop() => audioSource.Stop();
    public void Pause() => audioSource.Pause();
}
