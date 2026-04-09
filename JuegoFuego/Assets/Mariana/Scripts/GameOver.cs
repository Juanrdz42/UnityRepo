using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI waterText;
    public TextMeshProUGUI plantsText;
    public SFXManager sound;

    public void RetryLevel()
    {
        SceneManager.LoadScene("Mini3");
    }

    public void GoToMap()
    {
        SceneManager.LoadScene("Map");
    }


    void Start()
    {
        // muestra resultados guardados en PlayerPrefs
        int water = PlayerPrefs.GetInt("Water", 0);
        int plants = PlayerPrefs.GetInt("Plants", 0);

        waterText.text = "Agua recolectada: " + water;
        plantsText.text = "Plantas recolectadas: " + plants;

        if (PlayerPrefs.GetInt("Lives") > 0)
        {
            resultText.text = "¡Ganaste!";
            sound.WinSound();
        }
        else
        {
            resultText.text = "Game Over";
            sound.LoseSound();
        }
    }

    
    
}
