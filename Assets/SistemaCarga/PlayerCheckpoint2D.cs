using UnityEngine;

public class PlayerCheckpoint2D : MonoBehaviour
{
    [Tooltip("Tag del checkpoint")]
    public string checkpointTag = "Checkpoint";

    [SerializeField] private PlayerProgressManager ppm;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(checkpointTag))
        {
            if (ppm != null)
            {
                Vector3 pos2D = transform.position;

                // Para 3D usamos Vector3.zero ya que no aplica
                ppm.ReachCheckpoint(Vector3.zero, Vector3.zero, pos2D, false);

            }
            else
            {
                Debug.LogWarning("No se encontró PlayerProgressManager en la escena.");
            }
        }
    }
}
