using UnityEngine;
namespace Pablo{

public class BotonInteractivo : MonoBehaviour
{
    public Sprite botonPresionadoSprite;
    public GameObject quizPanel;
    private bool yaSePresiono = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        //si el jugador pisa el botón y no se ha presionado
        if (other.CompareTag("Player") && !yaSePresiono)
        {
            yaSePresiono = true;
            SFXManager.Instance.PlaySFX(SFXManager.Instance.sfxBoton);
            
            GetComponent<SpriteRenderer>().sprite = botonPresionadoSprite;
            
            quizPanel.SetActive(true);
        }
    }
    
}
}