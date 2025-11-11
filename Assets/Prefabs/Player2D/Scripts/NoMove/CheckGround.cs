using UnityEngine;

public class CheckGround : MonoBehaviour
{
    public static bool isGrounded = false;

    [Tooltip("Layer(s) que representan el suelo")]
    public LayerMask groundLayers;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Filtra por layer
        if (((1 << collision.gameObject.layer) & groundLayers) != 0)
        {
            isGrounded = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & groundLayers) != 0)
        {
            isGrounded = false;
        }
    }
}
