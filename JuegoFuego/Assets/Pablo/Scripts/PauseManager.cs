using UnityEngine;
using UnityEngine.InputSystem; 
using UnityEngine.SceneManagement;

namespace Pablo{

public class PauseManager : MonoBehaviour
{
    [Header("Referencias UI")]
    public GameObject canvasPausa; 
    
    private bool estaPausado = false;

    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (estaPausado) Reanudar();
            else Pausar();
        }
    }

    public void alternarPausa()
    {
        if (estaPausado) Reanudar();
        else Pausar();
    }

    public void Pausar()
    {
        estaPausado = true;
        canvasPausa.SetActive(true); 
        Time.timeScale = 0f; //congela el juego
        
        //mostrar el cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Reanudar()
    {
        estaPausado = false;
        canvasPausa.SetActive(false); 
        Time.timeScale = 1f;         
    }

    public void IrAlMenu()
    {
        Time.timeScale = 1f; //resetear el tiempo
        SceneManager.LoadScene("MenuScene"); // Cambia por el nombre de tu escena
    }
}
}