using UnityEngine;

public class Key : MonoBehaviour
{
    public GameObject door;
    public AudioClip keySound;
    [Range(0f, 1f)] public float volume = 2f; // que tan fuerte se escucha, le estuve cambiando porque se escuchaba super bajito


void Start()
{
    // aqui checa si ya se quito la puerta para que cuando regrese de las nubes siga sin ponerse la llave otravezs
    if (GameData.puertaAbierta)
    {
        Destroy(gameObject);
    }
}
    private void OnTriggerEnter2D(Collider2D collision)
{
    // si el plater toca la llave
    if(collision.gameObject.CompareTag("Player"))
    {
        if (keySound != null) // le pone sonido
        {
            AudioSource.PlayClipAtPoint(keySound, transform.position, volume);
        }

        // y si todvia hay puerta la quita
        if (door != null) Destroy(door);
        
        // GUARDA QUE LA PUERTA YA NO EXISTE
        GameData.puertaAbierta = true; 

        // borra la llave
        Destroy(gameObject);
    }
}
}