using UnityEngine;

public class Parallax : MonoBehaviour
{
    [Header("Parallax Config")]
    [Range(0f, 1f)]
    public float parallaxMultiplier = 0.1f;

    [Header("References")]
    public Transform player; // asignalo desde el inspector

    private Material parallaxMaterial;
    private float lastPlayerX;

    private void Start()
    {
        parallaxMaterial = GetComponent<Renderer>().material;

        if (player == null)
        {
            enabled = false;
            return;
        }

        lastPlayerX = player.position.x;
    }

    private void Update()
    {
        float deltaX = player.position.x - lastPlayerX;

        parallaxMaterial.mainTextureOffset += new Vector2(deltaX * parallaxMultiplier, 0);

        lastPlayerX = player.position.x;
    }
}
