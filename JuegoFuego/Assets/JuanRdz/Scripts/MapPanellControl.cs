using UnityEngine;
using UnityEngine.SceneManagement;

public class MapPanelController : MonoBehaviour
{
    public GameObject panel;
    public string mapSceneName = "Map"; 

    public void ReturnToMap()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mapSceneName);
    }

    public void ClosePanel()
    {
        panel.SetActive(false);
        Time.timeScale = 1f;
    }
}