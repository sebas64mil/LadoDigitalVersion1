using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class VisionConeMesh : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private EnemyController enemy; // Asignado automáticamente si está en el mismo GameObject
    private EnemyModel model;
    private Mesh mesh;

    [Header("Material")]
    [SerializeField] private Material coneMaterial; // Asignar desde el inspector

    [Header("Parámetros de visión")]
    public float rangonVision = 10f;


    public Light spotLight;

    [Header("Ajustes visuales")]
    [SerializeField] private Color patrolColor = Color.cyan;
    [SerializeField] private Color alertColor = Color.red;

    // Referencia a la propiedad de color del Shader Graph
    private static readonly int ColorProperty = Shader.PropertyToID("_Color");
    private static readonly int EmissionProperty = Shader.PropertyToID("_EmissionColor");

    private void Awake()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        if (enemy == null)
            enemy = GetComponentInParent<EnemyController>();

        if (enemy != null)
            model = enemy.enemyModel;

        MeshRenderer renderer = GetComponent<MeshRenderer>();

        if (coneMaterial != null)
        {
            // Clonamos el material para no modificar el asset original
            coneMaterial = new Material(coneMaterial);
            renderer.material = coneMaterial;
            coneMaterial.EnableKeyword("_EMISSION");
        }
        else
        {
            coneMaterial = renderer.material;
            coneMaterial.EnableKeyword("_EMISSION");
        }
    }

    private void Start()
    {
        if (model != null)
        {
            CreateVisionMesh(model.VisionAngle, model.VisionRange);
            ConfigLight(model.VisionAngle, model.VisionRange);
            UpdateColor();
        }
    }

    private void Update()
    {
        if (model == null) return;

        // Actualizar geometría y color
        CreateVisionMesh(model.VisionAngle, model.VisionRange);
        UpdateColor();
    }


    private void ConfigLight(float visionAngle, float visionRange) { 
    
        if (spotLight != null)
        {
            spotLight.spotAngle = visionAngle;
            spotLight.range = visionRange;
        }
    }

    private void CreateVisionMesh(float visionAngle, float visionRange)
    {
        int resolution = 30; // puedes usar model.Resolution si lo tienes

        Vector3[] vertices = new Vector3[resolution + 2];
        int[] triangles = new int[resolution * 3];

        vertices[0] = Vector3.zero;

        float angleStep = visionAngle / resolution;
        float startAngle = -visionAngle / 2f;

        for (int i = 0; i <= resolution; i++)
        {
            float currentAngle = startAngle + angleStep * i;
            float rad = currentAngle * Mathf.Deg2Rad;

            Vector3 vertex = new Vector3(Mathf.Sin(rad), 0, Mathf.Cos(rad)) * visionRange;
            vertices[i + 1] = vertex;
        }

        for (int i = 0; i < resolution; i++)
        {
            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = i + 1;
            triangles[i * 3 + 2] = i + 2;
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }

private void UpdateColor()
{
    if (coneMaterial == null || enemy == null) return;

    Color newColor = patrolColor;
    Color NewColorLight = patrolColor;

        if (enemy.CurrentState is WaypointPatrol)
    {
        newColor = patrolColor;
        NewColorLight = patrolColor;
        }
    else if (enemy.CurrentState is ChaseState)
    {
        newColor = alertColor;
          NewColorLight = alertColor; 
        }

        coneMaterial.SetColor(ColorProperty, newColor);
    coneMaterial.SetColor(EmissionProperty, newColor * 0.5f);
        spotLight.color = NewColorLight;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (mesh != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireMesh(mesh, transform.position, transform.rotation);
        }
    }
#endif
}
