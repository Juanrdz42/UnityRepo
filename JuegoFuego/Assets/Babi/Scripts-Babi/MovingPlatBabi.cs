using UnityEngine;

public class MovingPlatBabi : MonoBehaviour
{
    public float speed = 2f;
    public Transform[] points; // la lista de los 2 puntos donde se va a mover (podria poner más)
    private int i; // contador para saber a que punto va a ir

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // al principio pone a la plataforma en el primer punto
        transform.position = points[0].position;
    }

    // Update is called once per frame
    void FixedUpdate()
    { 
        // si la distancia entre la plataforma y el punto al que quiere ir es menos de 0
        if (Vector2.Distance(transform.position, points[i].position) < 0.01f)
        {
            i++; // lo lleva al siguiente punto
            if (i == points.Length) // si ya llego al ultimo punto (son solo 2 lol)
            {
                i = 0; // regresa otravez al primero
            }
        }

        // va moviendo la plataforma poquito a poquito
        transform.position = Vector2.MoveTowards(transform.position, points[i].position, speed * Time.fixedDeltaTime);
    }   // con move towards evita que la plataforma se pase el punto del destino y se quede vibrando

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // cuando toca la plataforma el player se hace hijo para que se mueva con ella
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // cuando ya no la esta tocando deja de ser hijo para que ya no se mueva con la platadorma
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.CompareTag("Player"))
        {
            if (gameObject.activeInHierarchy)
            {
                collision.transform.SetParent(null);
            }
        }
        }
    }
}  
