using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CloudResponse : MonoBehaviour
{
    private TextMeshPro textoNube;

    void Start() {
        textoNube = GetComponentInChildren<TextMeshPro>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Guardamos la respuesta que eligió el jugador
            string respuestaElegida = textoNube.text;
            int idPregunta = GameData.PreguntaActualID;

            if (GameData.respuestasEncuesta.ContainsKey(idPregunta))
                GameData.respuestasEncuesta[idPregunta] = respuestaElegida;
            else
                GameData.respuestasEncuesta.Add(idPregunta, respuestaElegida);

            Debug.Log("El jugador eligió: " + respuestaElegida + " para la pregunta " + idPregunta);

            // Regresamos a la escena principal
            SceneManager.LoadScene("Mini1"); 
        }
    }
}