using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class FinalResultsPanel : MonoBehaviour
{
    public TextMeshProUGUI textoResumen; // salen las respuestas

    public void MostrarResultados()
    {
        gameObject.SetActive(true);
        Time.timeScale = 0; // Pausamos el juego

        string resumen = "RESUMEN DE TU PERFIL:\n\n";

        // poe el diccionario donde guardamos las respuestas
        foreach (var entrada in GameData.respuestasEncuesta)
        {
            resumen += "Pregunta " + entrada.Key + ": " + entrada.Value + "\n";
        }

        textoResumen.text = resumen;
    }

    public void RegresarAlMapaFinal()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Map");
    }
}