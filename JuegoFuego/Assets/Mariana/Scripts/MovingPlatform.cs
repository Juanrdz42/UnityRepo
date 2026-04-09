using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float speed = 2f;
    public float distance = 3f;

    private Vector3 startPos;

    void Start() => startPos = transform.position;

    void Update()
    {
        float x = Mathf.Sin(Time.time * speed) * distance;
        transform.position = new Vector3(startPos.x + x, startPos.y, 0);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
            other.transform.SetParent(transform); // el jugador se vuelve hijo de la plataforma
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
            other.transform.SetParent(null); // se desvincula al saltar
    }
}