using UnityEngine;

public class PlayerHealth3D : MonoBehaviour
{
    [SerializeField] private int maxHealth = 1;
    private int currentHealth;
    public PlayerMove3D playerController; 

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        //  Desactiva el control
        if (playerController != null)
        {
            playerController.enabled = false;
        }

        playerController.stepsSource.Stop();
        playerController.StopCrouchBreathing();
        playerController.PlayDeathSound();
        //  Reinicia la escena
        GameManager.RestartScene();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            GetComponent<PlayerHealth3D>().TakeDamage(1);
        }
    }

}
