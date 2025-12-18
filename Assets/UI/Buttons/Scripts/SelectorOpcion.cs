using System;
using UnityEngine;
using TMPro;
using UnityEngine.Localization;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Localization.Settings;


public class SelectorOpcion : MonoBehaviour
{
    public LocalizedString[] opciones;
    public TextMeshProUGUI textoOpcion;

    public Action<int> OnValueChanged;

    private int indiceActual = 0;
    private AsyncOperationHandle<string> loadOp;

    void Start()
    {
        ActualizarTexto();

        // 🔥 Escuchar si el idioma cambia
        LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;
    }

    private void OnDestroy()
    {
        // Muy importante para evitar memory leaks
        LocalizationSettings.SelectedLocaleChanged -= OnLocaleChanged;
    }

    private void OnLocaleChanged(Locale obj)
    {
        ActualizarTexto(); // 🔥 Recargar la opción actual con el nuevo idioma
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
        OnValueChanged?.Invoke(indiceActual);
    }

    private void ActualizarTexto()
    {
        if (opciones == null || opciones.Length == 0 || textoOpcion == null)
            return;

        // Cancelar carga anterior si existe
        if (loadOp.IsValid())
            loadOp.Completed -= OnTextLoaded;

        loadOp = opciones[indiceActual].GetLocalizedStringAsync();
        loadOp.Completed += OnTextLoaded;
    }

    private void OnTextLoaded(AsyncOperationHandle<string> op)
    {
        textoOpcion.text = op.Result;
    }

    public int GetIndex() => indiceActual;

    public void SetIndex(int index)
    {
        indiceActual = Mathf.Clamp(index, 0, opciones.Length - 1);
        ActualizarTexto();
    }
}
