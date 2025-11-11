using UnityEngine;
using System.Collections;

public class ManagerTransition : MonoBehaviour
{
    [Header("Referencias (asignar en Inspector)")]
    public GameObject player3D;
    public GameObject player2D;
    public GameObject CinemachineCamera;
    [SerializeField] private PlayerProgressManager ppm;


    [HideInInspector] public Animator transitionAnimator;

    [Tooltip("Duración del fade en segundos (ajusta según tu animación)")]
    public float transitionTime = 1f;

    void Awake()
    {
        // Busca el Animator en los hijos del objeto actual
        transitionAnimator = GetComponentInChildren<Animator>();
    }

    void Start()
    {
        // Cargamos los datos del guardado
        DefaultSceneData currentData = SaveSystem.Load();

        if (currentData == null)
        {
            // Si no hay datos, asumimos que empieza en 3D
            player3D.SetActive(true);
            player2D.SetActive(false);
            CinemachineCamera.SetActive(false);
            return;
        }

        // Si hay datos, activamos el modo correcto
        if (currentData.is3D)
        {
            player3D.SetActive(true);
            player2D.SetActive(false);
            CinemachineCamera.SetActive(false);
            if (LevelMusicManager.Instance != null)
                LevelMusicManager.Instance.SwitchTo3D();

        }
        else
        {
            player3D.SetActive(false);
            player2D.SetActive(true);
            CinemachineCamera.SetActive(true);

            if (LevelMusicManager.Instance != null)
                LevelMusicManager.Instance.SwitchTo2D();
        }
    }


    // --- CAMBIO A 3D ---
    public void ChangeTo3D(Vector3 position3D)
    {
        StartCoroutine(DoTransition(() => DoChangeTo3D(position3D)));
    }

    private void DoChangeTo3D(Vector3 position3D)
    {
        DimensionEvents.TriggerDimensionChange(true);
        player3D.SetActive(true);
        player3D.transform.position = position3D;
        player2D.SetActive(false);
        CinemachineCamera.SetActive(false);
        if (LevelMusicManager.Instance != null)
            LevelMusicManager.Instance.SwitchTo3D();

        if (ppm != null)
        {
            ppm.SetIs3D(true);
            ppm.SavePosition3D(player3D.transform.position);
        }

    }

    public void posicion3d(Vector3 position3D, Vector3 rotation3D) {

        player3D.transform.position = position3D;
        player3D.transform.eulerAngles = rotation3D; 

    }

    // --- CAMBIO A 2D ---
    public void ChangeTo2D(Vector3 position2D)
    {
        StartCoroutine(DoTransition(() => DoChangeTo2D(position2D)));
    }

    private void DoChangeTo2D(Vector3 position2D)
    {
        DimensionEvents.TriggerDimensionChange(false);
        player3D.SetActive(false);
        player2D.SetActive(true);
        player2D.transform.position = position2D;
        CinemachineCamera.SetActive(true);

        if (LevelMusicManager.Instance != null)
            LevelMusicManager.Instance.SwitchTo2D();

        if (ppm != null)
        {
            ppm.SetIs3D(false);
            ppm.SavePosition2D(player2D.transform.position);
        }

    }

    public void posicion2d(Vector3 position2D) {
        player2D.transform.position = position2D;
    }

    // --- CORRUTINA COMPARTIDA ---
    private IEnumerator DoTransition(System.Action changeAction)
    {
        // Dispara la animación
        transitionAnimator.SetTrigger("StartTransition");

        // Espera la duración del fade
        yield return new WaitForSeconds(transitionTime);

        // Ejecuta la acción que corresponda (3D o 2D)
        changeAction();
    }
}
