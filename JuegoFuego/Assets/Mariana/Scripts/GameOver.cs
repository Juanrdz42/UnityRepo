using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.WSA;


public class GameOver : MonoBehaviour
{
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI waterText;
    public SFXManager sound;
    public GameObject gameOverPanel;
    public GameObject wonPanel;
    public GameObject star;
    public GameObject stars2;
    public GameObject stars3;
    public GameObject mono;
    public void RetryLevel()
    {
        SceneManager.LoadScene("Mini3");
    }

    public void GoToMap()
    {
        SceneManager.LoadScene("Map");
    }


    void ShowStars (int puntosFinales) {
        if (puntosFinales >= 50) {
            stars3.SetActive(true);
        }
        else if (puntosFinales >= 30) {
            stars2.SetActive(true);
        }
        else {
            star.SetActive(true);
        }
}
    void Start()
    {
        if (PlayerPrefs.GetInt("Lives") > 0)
        {
            wonPanel.SetActive(true);
            sound.WinSound();

            star.SetActive(false);
            stars2.SetActive(false);
            stars3.SetActive(false);
            
            // muestra resultados guardados en PlayerPrefs
            int water = PlayerPrefs.GetInt("Water", 0);
            ShowStars(water);

            waterText.text = "Agua recolectada: " + water;
        }
        else {
            sound.LoseSound();
            gameOverPanel.SetActive(true);
            mono.SetActive(true);
            GameObject player = GameObject.FindWithTag("Player"); 
            if (player != null) {
                Destroy(player); 
            }            
        }
    }
}

