using UnityEngine;

public class ObjectMovement : MonoBehaviour
{
    public float speed = 10f; // que tan rapido van las nubes (o nose si le iba a poner otras cosas)

    void Update()
    {
        // mueve el objeto a la izquierda todo el tiempo
        transform.Translate(Vector3.left * speed * Time.deltaTime);

        // si la nube se sale mucho de la pantalla, se destruye sola
        if (transform.position.x < -20) 
        {
            Destroy(gameObject);
        }
    }
}
