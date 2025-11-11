using System.Collections.Generic;
using UnityEngine;

public class DimensionObjectManager : MonoBehaviour
{
    private static List<DimensionObjectToggle> allObjects = new List<DimensionObjectToggle>();

    private void OnEnable()
    {
        DimensionEvents.OnDimensionChange += UpdateDimensionState;
    }

    private void OnDisable()
    {
        DimensionEvents.OnDimensionChange -= UpdateDimensionState;
    }

    public static void Register(DimensionObjectToggle obj)
    {
        if (!allObjects.Contains(obj))
            allObjects.Add(obj);
    }

    public static void Unregister(DimensionObjectToggle obj)
    {
        allObjects.Remove(obj);
    }

    public static void UpdateDimensionState(bool is3D)
    {
        foreach (var obj in allObjects)
        {
            if (obj != null)
                obj.ApplyDimension(is3D);
        }
    }
}
