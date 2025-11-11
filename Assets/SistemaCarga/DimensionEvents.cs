using System;
using UnityEngine;

public static class DimensionEvents
{
    // Acción que notifica cuando se cambia entre 3D ↔ 2D
    public static Action<bool> OnDimensionChange;

    // Método para disparar el evento
    public static void TriggerDimensionChange(bool is3D)
    {
        OnDimensionChange?.Invoke(is3D);
    }
}
