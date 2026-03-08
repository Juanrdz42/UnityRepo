using UnityEngine;
using TMPro;
using System.Collections;

public class ForestGameController_JuanRdz : MonoBehaviour
{

    public static ForestGameController_JuanRdz Instance;
    public TextMeshProUGUI countdownText;
    public TextMeshProUGUI timerText;

    public float gameTime = 60f;

    private float currentTime;
    private bool gameStarted = false;
    public PhotoSpot[] photoSpots;

    void Awake()
    {
        Instance = this;
    }

        void Start()
    {
        timerText.gameObject.SetActive(false); 
        StartCoroutine(StartCountdown());
    }

    IEnumerator StartCountdown()
    {
        countdownText.gameObject.SetActive(true);

        countdownText.text = "3";
        yield return new WaitForSeconds(1);

        countdownText.text = "2";
        yield return new WaitForSeconds(1);

        countdownText.text = "1";
        yield return new WaitForSeconds(1);

        countdownText.text = "¡Vamos!";
        yield return new WaitForSeconds(1);

        countdownText.gameObject.SetActive(false);

        timerText.gameObject.SetActive(true);
    
        StartGame();
        ActivateRandomSpots();
    }

    void StartGame()
    {
        gameStarted = true;
        currentTime = gameTime;
    }

    void Update()
    {
        if (!gameStarted) return;

        currentTime -= Time.deltaTime;

        timerText.text = Mathf.Ceil(currentTime).ToString();

        if (currentTime <= 0)
        {
            EndGame();
        }
    }

    void ActivateRandomSpots()
{
    // Apagar todos primero
    foreach (PhotoSpot spot in photoSpots)
    {
        spot.SetActiveSpot(false);
    }

    // Crear copia de la lista
    System.Collections.Generic.List<PhotoSpot> shuffledSpots =
        new System.Collections.Generic.List<PhotoSpot>(photoSpots);

    // Mezclar
    for (int i = 0; i < shuffledSpots.Count; i++)
    {
        PhotoSpot temp = shuffledSpots[i];
        int randomIndex = Random.Range(i, shuffledSpots.Count);
        shuffledSpots[i] = shuffledSpots[randomIndex];
        shuffledSpots[randomIndex] = temp;
    }

    // Activar solo 3
    for (int i = 0; i < 3 && i < shuffledSpots.Count; i++)
    {
        shuffledSpots[i].SetActiveSpot(true);
    }
}

    void EndGame()
    {
        gameStarted = false;
        Debug.Log("Fin del minijuego");
    }
}
