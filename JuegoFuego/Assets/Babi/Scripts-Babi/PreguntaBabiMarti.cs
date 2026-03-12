using UnityEngine;
using UnityEngine.SceneManagement;

public class PreguntaBabiMarti : MonoBehaviour
{
    public string nombreEscenaDestino = "CloudScene";

    public void LoadNextScene() // cuando choca con la bandera
{
    // busca al player
    GameObject player = GameObject.FindGameObjectWithTag("Player");
    if(player != null)
    {
        // guarda su posicion
        GameData.posicionRetorno = player.transform.position;
        
        // checa cuantas plantas tiene en el script del Player
        PlayerBabi scriptJugador = player.GetComponent<PlayerBabi>();
        if(scriptJugador != null)
        {
            // guarda en GameData
            GameData.plantasGuardadas = scriptJugador.plants;
        }

        // avisa que se va al de las nubes para cuando regrese que lo ponga en ese púnto
        GameData.regresarDeNubes = true;
    }

    // checa que el tiempo no este pausado
    Time.timeScale = 1; 
    // y luego cambia de escena ahora si
    UnityEngine.SceneManagement.SceneManager.LoadScene(nombreEscenaDestino);
}
}