using UnityEngine;

public class Instruction : MonoBehaviour
{
    public GameObject panelInstrucciones;

    void Start()
    {
        // checa si es la primera vez que entra
        if (GameData.yaVioInstrucciones == false)
        {
            Time.timeScale = 0;
            panelInstrucciones.SetActive(true);
        }
        else
        {
            // si ya las vio, lo apaga
            // y el tiempo corriendo
            panelInstrucciones.SetActive(false);
            Time.timeScale = 1;
        }
    }

    public void CerrarInstrucciones()
    {
        Time.timeScale = 1;
        panelInstrucciones.SetActive(false);
        
        // AQUI CEHCA QUE YA LO VIO
        GameData.yaVioInstrucciones = true;
        
        
    }
}