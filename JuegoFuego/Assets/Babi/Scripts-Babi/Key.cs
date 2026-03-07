using UnityEngine;

public class Key : MonoBehaviour
{
    public GameObject door;
    public AudioClip keySound;
    [Range(0f, 1f)] public float volume = 2f; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            if (keySound != null)
            {
                AudioSource.PlayClipAtPoint(keySound, transform.position, volume);
            }

            if (door != null) Destroy(door);
            Destroy(gameObject);
        }
    }
}