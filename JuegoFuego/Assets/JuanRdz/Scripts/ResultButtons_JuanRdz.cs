using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultsButtons_JuanRdz : MonoBehaviour
{
    public string levelScene = "Mini2_Bosque";
    public string hubScene = "Mini2";
    public string mapScene = "Map";

    public void RetryLevel()
    {
        SceneManager.LoadScene(levelScene);
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