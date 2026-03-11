using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ConfirmationPanel : MonoBehaviour
{
    public TextMeshProUGUI chosenText;

    // esta funcion la pone la nube cuando el avion la choca
    public void ShowPanel(string respuesta)
    {
        gameObject.SetActive(true);
        chosenText.text = "Seleccionaste: " + respuesta;
        Time.timeScale = 0; // pausa el juego
    }

    public void ConfirmarYRegresar()
    {
        Time.timeScale = 1; // quita la pausa porque sino se queda stuck para siempre
        if (GameData.PreguntaActualID >= 5) 
    {
        // busca el panel de resultados y lo activa
        FinalResultsPanel panelFinal = Object.FindFirstObjectByType<FinalResultsPanel>(FindObjectsInactive.Include);
        if (panelFinal != null)
        {
            gameObject.SetActive(false); 
            
            // Y mostramos el nuevo
            panelFinal.MostrarResultados();
        }
    }
    else
    {
        // si no es la última, regresa al de siempre
        SceneManager.LoadScene("Mini1");
    }
    }

    public void ReintentarEscena()
    {
        Time.timeScale = 1; // quitar pausa
        
        string escenaActual = SceneManager.GetActiveScene().name;
        
        SceneManager.LoadScene(escenaActual);
    }
}