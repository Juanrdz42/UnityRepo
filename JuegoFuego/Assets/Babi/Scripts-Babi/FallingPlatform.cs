using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    public float timeBeforeFall = 1.0f; // tiempo que se tarda en caerse (en unity le puse mucho menos)

    // cuando la pisa
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // si la toca el player
        if (collision.gameObject.CompareTag("Player"))
        {
            // hace como una cuenta regresiva (deq en tantos segundos haz la funcion fall)
            Invoke("Fall", timeBeforeFall);
        }
    }

    void Fall()
    {
        // primero cheeca si ya tiene un Rigidbody para no ponerlo doble, entonces se lo pone y ahora como tiene gravedad se cae
        if (GetComponent<Rigidbody2D>() == null)
        {
            gameObject.AddComponent<Rigidbody2D>();
        }
    }
}