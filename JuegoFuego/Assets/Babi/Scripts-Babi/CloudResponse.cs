using UnityEngine;
using TMPro;

public class CloudResponse : MonoBehaviour
{
    private TextMeshPro textoNube;
    public AudioClip popSound; 

    void Start() {
        textoNube = GetComponentInChildren<TextMeshPro>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // reproduce el sonido en la posición de la nube
            if (popSound != null)
            {
                AudioSource.PlayClipAtPoint(popSound, transform.position);
            }

            string respuestaElegida = textoNube.text;
            int idPregunta = GameData.PreguntaActualID;

            if (GameData.respuestasEncuesta.ContainsKey(idPregunta))
                GameData.respuestasEncuesta[idPregunta] = respuestaElegida;
            else
                GameData.respuestasEncuesta.Add(idPregunta, respuestaElegida);

            ConfirmationPanel cm = Object.FindFirstObjectByType<ConfirmationPanel>(FindObjectsInactive.Include);
            if (cm != null)
            {
                cm.ShowPanel(respuestaElegida);
            }

            Destroy(gameObject);
        }
    }
}