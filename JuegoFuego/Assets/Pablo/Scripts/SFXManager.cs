using UnityEngine;
using UnityEngine.SceneManagement;
namespace Pablo{

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance;

    [Header("Fuentes de audio")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Música")]
    public AudioClip musicMenu;
    public AudioClip musicNivel;
    public AudioClip musicJefe;
    public AudioClip musicVictoria;
    public AudioClip musicDerrota;

    [Header("Efectos SFX")]
    public AudioClip sfxSalto;
    public AudioClip sfxDisparo;
    public AudioClip sfxGolpeJugador;
    public AudioClip sfxMuerteJugador;
    public AudioClip sfxMuerteEnemigo;
    public AudioClip sfxMuerteJefe;
    public AudioClip sfxPlanta;
    public AudioClip sfxBoton;
    public AudioClip sfxPuerta;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable() { SceneManager.sceneLoaded += AlCambiarEscena; }
    void OnDisable() { SceneManager.sceneLoaded -= AlCambiarEscena; }

    //cambia la musica segun la escena
    void AlCambiarEscena(Scene escena, LoadSceneMode modo)
    {
        if (escena.name == "MenuScene") PlayMusic(musicMenu);
        else if (escena.name == "GameScene") PlayMusic(musicNivel);
        else if (escena.name == "GameOver") PlayMusic(musicDerrota);
        else if (escena.name == "EndScene") PlayMusic(musicVictoria);
    }

    public void PlayMusic(AudioClip clip)
    {
        if (musicSource.clip == clip) return; //no reiniciar si ya suena
        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
}
}