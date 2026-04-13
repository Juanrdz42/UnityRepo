using UnityEngine;
namespace Pablo{
public class QuizManager : MonoBehaviour
{
    public GameObject quizPanel;
    public GameObject puertaHab;
    public UIController uiController;

    public void RespuestaCorrecta()
    {
        Debug.Log("¡Correcto!");
        if (puertaHab != null)
        {
            puertaHab.SetActive(false); //puerta desaparece
            SFXManager.Instance.PlaySFX(SFXManager.Instance.sfxPuerta);
        }
        
        //cuando es correcta, cierra el panel
        quizPanel.SetActive(false);
        Time.timeScale = 1f; 
    }

    public void RespuestaIncorrecta()
{
    PlayerController player = FindAnyObjectByType<PlayerController>();
    if (player != null)
    {
        player.TakeDamage(1); 
    }
}
}
}
