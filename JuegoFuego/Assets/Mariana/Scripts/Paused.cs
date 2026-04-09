using UnityEngine;

public class Paused : MonoBehaviour
{
    public GameObject container;

    public void OpenPauseMenu()
    {
        container.SetActive(true);
        Time.timeScale = 0; 
    }

    public void ResumeButton()
    {
        container.SetActive(false);
        Time.timeScale = 1; 
    }

    public void MainMenuButton()
    {
        Time.timeScale = 1; 
        UnityEngine.SceneManagement.SceneManager.LoadScene("Map");
    }
}

