using UnityEngine;

public class Key : MonoBehaviour
{
    public GameObject door;
    public AudioClip keySound;
    [Range(0f, 1f)] public float volume = 2f; 


void Start()
{
    if (GameData.puertaAbierta)
    {
        Destroy(gameObject);
    }
}
    private void OnTriggerEnter2D(Collider2D collision)
{
    if(collision.gameObject.CompareTag("Player"))
    {
        if (keySound != null)
        {
            AudioSource.PlayClipAtPoint(keySound, transform.position, volume);
        }

        if (door != null) Destroy(door);
        
        // GUARDAMOS QUE LA PUERTA YA NO DEBE EXISTIR
        GameData.puertaAbierta = true; 

        Destroy(gameObject);
    }
}
}