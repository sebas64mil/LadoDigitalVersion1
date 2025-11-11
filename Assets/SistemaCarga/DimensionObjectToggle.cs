using UnityEngine;

public class DimensionObjectToggle : MonoBehaviour
{
    [Header("Configuración del objeto")]
    public bool isFor3D = true;

    private void Awake()
    {
        DimensionObjectManager.Register(this);
    }

    private void OnDestroy()
    {
        DimensionObjectManager.Unregister(this);
    }

    private void Start()
    {
        var data = SaveSystem.Load();
        bool currentIs3D = data != null ? data.is3D : true;
        ApplyDimension(currentIs3D);
    }

    public void ApplyDimension(bool is3D)
    {
        bool shouldBeActive = (is3D && isFor3D) || (!is3D && !isFor3D);
        gameObject.SetActive(shouldBeActive);
    }
}
