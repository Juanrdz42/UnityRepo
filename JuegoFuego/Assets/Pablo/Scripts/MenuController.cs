using UnityEngine;
using UnityEngine.SceneManagement;
namespace Pablo{
public class MenuController : MonoBehaviour
{
    public void StartToPlay() //carga la escena del juego para iniciar
    {
        SceneManager.LoadScene("GameScene");
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }
    public void Exit() //cierra el juego
    {
        //UnityEditor.EditorApplication.isPlaying = false;
        SceneManager.LoadScene("Map");
    }
}
}
