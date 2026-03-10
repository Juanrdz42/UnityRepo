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
        SceneManager.LoadScene("Mini1");  // regresa al otro y ahora va a saber donde se quedo la vez pasada
    }
}