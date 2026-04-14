using UnityEngine;
using UnityEngine.SceneManagement;

namespace Pablo{

public class GameControl : MonoBehaviour
{
    static public GameControl Instance;
    public UIController uiController;
    public SFXManager sfxManager;

    public void Awake()
    {
        //evita que el objeto se duplique al recargar escenas
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        //reset vidas si es la primera vez que inicia el juego
        if (!PlayerPrefs.HasKey("Lives")) PlayerPrefs.SetInt("Lives", 3);
        
        SetReferences();
    }

    //buscar el nuevo UI
    void OnEnable() { SceneManager.sceneLoaded += OnSceneLoaded; }
    void OnDisable() { SceneManager.sceneLoaded -= OnSceneLoaded; }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SetReferences();
    }

    void SetReferences()
    {
        uiController = FindAnyObjectByType<UIController>();
        sfxManager = FindAnyObjectByType<SFXManager>();
    }

    public int GetCurrentLives()
    {
        return PlayerPrefs.GetInt("Lives");
    }
    
    public void SpendLives()
    {
        int currentLives = GetCurrentLives();
        if(currentLives > 0)
        {
            int newLives = currentLives - 1;
            PlayerPrefs.SetInt("Lives", newLives);
            if(uiController != null) uiController.UpdateLives();
        }
        CheckGameOver();
    }

    public void CheckGameOver()
    {
        if (GetCurrentLives() <= 0)
        {
            SceneManager.LoadScene("GameOver");
        }
    }

    public void IrAVictoria()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene("EndScene");
    }
}
}