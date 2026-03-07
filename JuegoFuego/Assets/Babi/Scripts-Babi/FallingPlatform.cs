using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    public float timeBeforeFall = 1.0f; // Tiempo que tarda en caer

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Usamos CompareTag que es más eficiente
        if (collision.gameObject.CompareTag("Player"))
        {
            // Llamamos a la función que la hace caer después de unos segundos
            Invoke("Fall", timeBeforeFall);
        }
    }

    void Fall()
    {
        // Verificamos si ya tiene un Rigidbody para no añadirlo dos veces
        if (GetComponent<Rigidbody2D>() == null)
        {
            gameObject.AddComponent<Rigidbody2D>();
        }
    }
}