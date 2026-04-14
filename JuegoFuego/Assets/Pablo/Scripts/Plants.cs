using UnityEngine;

namespace Pablo{

public class Plants : MonoBehaviour
{
    public int valor = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SFXManager.Instance.PlaySFX(SFXManager.Instance.sfxPlanta);
            PlayerController player = collision.GetComponent<PlayerController>();

            if (player != null)
            {
                player.AddPlants(valor);
                Destroy(gameObject);    
            }
        }
    }

    void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.AddForce(Vector2.up * 5f, ForceMode2D.Impulse);
        }
    }
}
}