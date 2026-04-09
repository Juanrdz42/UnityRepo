using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour
{
    public int timeToWin = 59;
    static public GameControl Instance;
    public UIController uiController;
    public SFXManager sfxManager;
    private int coins = 0;
    private int watering = 0;
    public GameObject questionPanel; // panel de preguntas

    public void Awake()
    {

        StopAllCoroutines();
        PlayerPrefs.SetInt("TimeToWin", timeToWin); // siempre usa el valor del script
        Instance = this;
        Instance.SetReferences();
        DontDestroyOnLoad(this.gameObject);
    }

    void SetReferences()
    {
        if (uiController == null)
        {
            uiController = FindAnyObjectByType<UIController>();
        }
        if (sfxManager == null)
        {
            sfxManager = FindAnyObjectByType<SFXManager>();
        }
        timeToWin = PlayerPrefs.GetInt("TimeToWin", timeToWin);
        init();
    }

    void init()
    {
        if (uiController != null)
            uiController.StartTimer();
    }

    public void AddCoins(int amount)
    {
        PlayerController player = FindAnyObjectByType<PlayerController>();
        player.AddPlants(amount);
    }

    public int GetCoins() => coins;

    public void AddWatering(int amount)
    {
        PlayerController player = FindAnyObjectByType<PlayerController>();
        player.AddWater(amount); 
    }

    public int GetWater() => watering;

    public void TakeDamage(int amount)
    {
        PlayerController player = FindAnyObjectByType<PlayerController>();
        player.TakeDamage(amount);
    }

    public void LoadQuestion()
    {
        questionPanel.SetActive(true);
        Time.timeScale = 0f; // detiene el juego
    } 

    public void CheckGameOver()
    {
        PlayerController player = FindAnyObjectByType<PlayerController>();
        if (player != null && player.health <= 0)
        {
            PlayerPrefs.SetInt("Lives", 0);
            SceneManager.LoadScene("endScene");
        }
    }

    void Start()
    {
        
    }
    void Update()
    {
        
    }
}