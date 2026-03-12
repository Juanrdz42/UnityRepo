using UnityEngine;

public class Plantbb : MonoBehaviour
{
    public AudioClip plantClip;
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {

            PlayerBabi player = collision.GetComponent<PlayerBabi>();

            if (player != null)
            {
                
                if (plantClip != null)
                {
                    player.PlaySFX(plantClip);
                }

                player.plants++;
                
                
                if (player.Plants_Text != null)
                {
                    player.Plants_Text.text = player.plants.ToString();
                }

                
                GameData.plantasGuardadas = player.plants;

                Destroy(gameObject);
            }
        }
    }
}