using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ConfirmationPanel : MonoBehaviour
{
    public TextMeshProUGUI chosenText;

    public void ShowPanel(string respuesta)
    {
        gameObject.SetActive(true);
        chosenText.text = "Seleccionaste: " + respuesta;
        Time.timeScale = 0; // Pausamos el juego mientras confirma
    }

    public void ConfirmarYRegresar()
    {
        Time.timeScale = 1; // Reanudamos el tiempo
        SceneManager.LoadScene("Mini1"); 
    }
}