using UnityEngine;

public class ObjectMovement : MonoBehaviour
{
    public float speed = 10f; // que tan rapido van las nubes (o nose si le iba a poner otras cosas)
    public float delayBeforeMove = 5f; // segundos que se espera
    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= delayBeforeMove)
        {
            // mueve el objeto a la izquierda
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }

        // si la nube se sale mucho de la pantalla, se destruye sola
        if (transform.position.x < -20) 
        {
            Destroy(gameObject);
        }
    }
}
