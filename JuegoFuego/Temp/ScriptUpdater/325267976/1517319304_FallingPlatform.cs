using UnityEngine;

using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    public float timeBeforeFall = 0.1f;
    private Vector3 posicionInicial;
    private Rigidbody2D rb;
    private bool seCayo = false;

    void Start()
    {
        // guarda donde esta para ponerla otravez si se muere el player
        posicionInicial = transform.position;
        rb = GetComponent<Rigidbody2D>();
        
        // que no se mueva por la gravedad
        if (rb != null) {
            rb.bodyType = RigidbodyType2D.Kinematic;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !seCayo)
        {
            Invoke("Fall", timeBeforeFall);
        }
    }

    void Fall()
    {
        seCayo = true;
        // se vuelve dinamica para qeu con la gravedad se caiga ahora si
        rb.bodyType = RigidbodyType2D.Dynamic;
    }

    // cuadno el player se muere
    public void ResetPlatform()
    {
        CancelInvoke("Fall"); // por si se muere antes de que se caiga
        seCayo = false;
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.linearVelocity = Vector2.zero; // quita la fuerza
        rb.angularVelocity = 0;
        transform.position = posicionInicial; // la regresa al lugar
        transform.rotation = Quaternion.identity; // la pone derecha
    }
}