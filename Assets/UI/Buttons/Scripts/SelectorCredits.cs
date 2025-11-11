using UnityEngine;
using TMPro;

public class SelectorCredits : MonoBehaviour
{
    public GameObject[] paneles;
    public TextMeshProUGUI textoIndice; // Asigna el componente TMP desde el Inspector

    private int indiceActual = 0;

    void Start()
    {
        ActualizarPaneles();
        ActualizarTexto();
    }

    // Método para la flecha izquierda (<--)
    public void PanelIzquierda()
    {
        indiceActual--;
        if (indiceActual < 0)
            indiceActual = paneles.Length - 1;
        ActualizarPaneles();
        ActualizarTexto();
    }

    // Método para la flecha derecha (-->)
    public void PanelDerecha()
    {
        indiceActual++;
        if (indiceActual >= paneles.Length)
            indiceActual = 0;
        ActualizarPaneles();
        ActualizarTexto();
    }

    private void ActualizarPaneles()
    {
        for (int i = 0; i < paneles.Length; i++)
            paneles[i].SetActive(i == indiceActual);
    }

    private void ActualizarTexto()
    {
        if (textoIndice != null && paneles != null && paneles.Length > 0)
            textoIndice.text = $"{indiceActual + 1}/{paneles.Length}";
    }
}
