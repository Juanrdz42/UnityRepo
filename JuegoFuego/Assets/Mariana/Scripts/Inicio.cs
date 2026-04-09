using UnityEngine;

public class Inicio : MonoBehaviour
{
    public GameObject panelInstrucciones;

    void Start()
    {
        // checa si es la primera vez que entra
        if (PlayerController.firstTime == false)
        {
            Time.timeScale = 0;
            panelInstrucciones.SetActive(true);
        }
        else
        {
            // si ya las vio, lo apaga
            panelInstrucciones.SetActive(false);
            Time.timeScale = 1;
        }
    }

    public void Close()
    {
        Time.timeScale = 1;
        panelInstrucciones.SetActive(false);
        
        // AQUI CEHCA QUE YA LO VIO
        PlayerController.firstTime = true;
        
        
    }
}
