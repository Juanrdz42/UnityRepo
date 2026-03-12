using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    public string nombreDeEscena;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Cargando: " + nombreDeEscena);
            SceneManager.LoadScene(nombreDeEscena);
        }
    }
}