using System;
using UnityEngine;
using TMPro;

public class SelectorOpcion : MonoBehaviour
{
    public string[] opciones;
    public TextMeshProUGUI textoOpcion;

    public Action<int> OnValueChanged; // ✅ Evento que se disparará al cambiar

    private int indiceActual = 0;

    void Start()
    {
        ActualizarTexto();
    }

    public void OpcionIzquierda()
    {
        indiceActual--;
        if (indiceActual < 0)
            indiceActual = opciones.Length - 1;

        CambiarOpcion();
    }

    public void OpcionDerecha()
    {
        indiceActual++;
        if (indiceActual >= opciones.Length)
            indiceActual = 0;

        CambiarOpcion();
    }

    private void CambiarOpcion()
    {
        ActualizarTexto();
        OnValueChanged?.Invoke(indiceActual); // ✅ Avisamos del cambio
    }

    private void ActualizarTexto()
    {
        if (opciones != null && opciones.Length > 0 && textoOpcion != null)
            textoOpcion.text = opciones[indiceActual];
    }

    public int GetIndex() => indiceActual;

    public void SetIndex(int index)
    {
        indiceActual = Mathf.Clamp(index, 0, opciones.Length - 1);
        ActualizarTexto();
    }
}
