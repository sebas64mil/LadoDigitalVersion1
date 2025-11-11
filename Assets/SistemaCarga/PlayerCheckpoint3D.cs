using UnityEngine;

public class PlayerCheckpoint3D : MonoBehaviour
{
    [Tooltip("Tag del checkpoint")]
    public string checkpointTag = "Checkpoint";
    [SerializeField] private PlayerProgressManager ppm;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(checkpointTag))
        {
            if (ppm != null)
            {
                Vector3 pos3D = transform.position;
                Vector3 rot3D = transform.eulerAngles;

                ppm.ReachCheckpoint(pos3D, rot3D, Vector3.zero, true);

            }

        }
    }
}
