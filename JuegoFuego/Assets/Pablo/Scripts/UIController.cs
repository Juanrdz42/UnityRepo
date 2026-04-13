using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace Pablo{

public class UIController : MonoBehaviour
{
    [Header("Vidas")]
    public Sprite corazonVacio;
    public Image[] corazonesUI; 

    [Header("Monedas")]
    public TextMeshProUGUI monedasText;
    private int contadorMonedas = 0;

    public void UpdateLives() 
    {
        int vidasActuales = Pablo.GameControl.Instance.GetCurrentLives();

        //si perdemos una vida, el corazon se apaga
        if (vidasActuales >= 0 && vidasActuales < corazonesUI.Length)
        {
            corazonesUI[vidasActuales].sprite = corazonVacio;
        }
    }

    //actualizar monedas
    public void AñadirMoneda()
    {
        contadorMonedas++;
        if(monedasText != null) 
            monedasText.text = "x " + contadorMonedas;
    }
}
}