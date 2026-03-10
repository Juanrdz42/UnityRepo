using UnityEngine;
using TMPro;

public class CloudResponse : MonoBehaviour
{
    private TextMeshPro textoNube;
    public AudioClip popSound; 

    void Start() {
        textoNube = GetComponentInChildren<TextMeshPro>(); // busca el texto
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // sonido
            if (popSound != null)
            {
                AudioSource.PlayClipAtPoint(popSound, transform.position);
            }

            // guarda lo que dice la nube a la que choca
            string respuestaElegida = textoNube.text;
            int idPregunta = GameData.PreguntaActualID; // para que sepa cual es la pregunta que esta contestadno

            // va al gamedata y guarda la rspuesta
            if (GameData.respuestasEncuesta.ContainsKey(idPregunta))
                GameData.respuestasEncuesta[idPregunta] = respuestaElegida;
            else
                GameData.respuestasEncuesta.Add(idPregunta, respuestaElegida);

            // busca el panel de confiurmation y pone la info para que le salga al jugador y confirme su opcion
            ConfirmationPanel cm = Object.FindFirstObjectByType<ConfirmationPanel>(FindObjectsInactive.Include);
            if (cm != null)
            {
                cm.ShowPanel(respuestaElegida);
            }

            // rompe la nube
            Destroy(gameObject);
        }
    }
}