using UnityEngine;

public class ObjectMovement : MonoBehaviour
{
    public float speed = 10f;

    void Update()
    {
        // Mueve el objeto a la izquierda constantemente
        transform.Translate(Vector3.left * speed * Time.deltaTime);

        // Si la nube se sale mucho de la pantalla, se destruye para no gastar memoria
        if (transform.position.x < -20) 
        {
            Destroy(gameObject);
        }
    }
}
