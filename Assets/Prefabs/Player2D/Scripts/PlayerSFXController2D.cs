using UnityEngine;

public class PlayerSFXController2D : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource walkSource;
    public AudioSource dashSource;
    public AudioSource climbSource;
    public AudioSource deathAudio;
    public AudioSource jumpAudio;

    [Header("Pitch Randomization")]
    public bool randomizePitch = true;
    [Range(0.9f, 1.1f)] public float minPitch = 0.95f;
    [Range(0.9f, 1.1f)] public float maxPitch = 1.05f;

    private float GetRandomPitch()
    {
        return randomizePitch ? Random.Range(minPitch, maxPitch) : 1f;
    }

    // ===================== WALK =====================
    public void PlayWalk()
    {
        if (walkSource == null) return;

        walkSource.pitch = GetRandomPitch();
        if (!walkSource.isPlaying)
            walkSource.Play();
    }

    public void StopWalk()
    {
        if (walkSource != null && walkSource.isPlaying)
            walkSource.Stop();
    }

    // ===================== DASH =====================
    public void PlayDash()
    {
        if (dashSource == null || dashSource.clip == null) return;

        dashSource.pitch = GetRandomPitch();
        dashSource.PlayOneShot(dashSource.clip);
    }

    public void StopDash()
    {
        if (dashSource != null && dashSource.isPlaying)
            dashSource.Stop();
    }

    // ===================== CLIMB =====================
    public void PlayClimb()
    {
        if (climbSource == null) return;

        climbSource.pitch = GetRandomPitch();
        if (!climbSource.isPlaying)
            climbSource.Play();
    }

    public void StopClimb()
    {
        if (climbSource != null && climbSource.isPlaying)
            climbSource.Stop();
    }

    // ===================== DEATH =====================        

    public void PlayDeath()
    {
        if (deathAudio == null || deathAudio.clip == null) return;

        deathAudio.pitch = GetRandomPitch();
        deathAudio.Play();
    }

    // ===================== JUMP =====================

    public void PlayJump()
    {
        if (jumpAudio == null || jumpAudio.clip == null) return;
        jumpAudio.pitch = GetRandomPitch();
        jumpAudio.Play();
    }
}
