using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultsButtons_JuanRdz : MonoBehaviour
{
    public string hubScene = "Mini2";
    public string mapScene = "Map";

    public void RetryLevel()
    {
        string lastLevel = PlayerPrefs.GetString("LastLevel", "Mini2_Bosque");
        SceneManager.LoadScene(lastLevel);
    }

    public void GoToHub()
    {
        SceneManager.LoadScene(hubScene);
    }

    public void GoToMap()
    {
        SceneManager.LoadScene(mapScene);
    }
}