using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class ForestGameController_JuanRdz : MonoBehaviour
{
    public static ForestGameController_JuanRdz Instance;

    public TextMeshProUGUI countdownText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI resultText;

    public GameObject timerPanel;

    public float gameTime = 60f;

    private float currentTime;
    private bool gameStarted = false;
    private bool gameEnded = false;

    public PhotoSpot[] photoSpots;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        timerText.gameObject.SetActive(false);

        if (timerPanel != null)
            timerPanel.SetActive(false);

        if (resultText != null)
            resultText.gameObject.SetActive(false);

        StartCoroutine(StartCountdown());
    }

    IEnumerator StartCountdown()
    {
        countdownText.gameObject.SetActive(true);

        countdownText.text = "5";
        yield return new WaitForSeconds(1);

        countdownText.text = "4";
        yield return new WaitForSeconds(1);

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

        if (timerPanel != null)
            timerPanel.SetActive(true);

        StartGame();
        ActivateRandomSpots();
    }

    void StartGame()
    {
        gameStarted = true;
        gameEnded = false;
        currentTime = gameTime;
    }

    void Update()
    {
        if (!gameStarted || gameEnded) return;

        if (QuestController_JuanRdz.Instance != null)
        {
            if (QuestController_JuanRdz.Instance.currentPhotos >= QuestController_JuanRdz.Instance.targetPhotos)
            {
                EndGame();
                return;
            }
        }

        currentTime -= Time.deltaTime;

        if (currentTime < 0)
            currentTime = 0;

        timerText.text = "Tiempo: " + Mathf.Ceil(currentTime).ToString();

        if (currentTime <= 0)
        {
            EndGame();
        }
    }

    void ActivateRandomSpots()
    {
        foreach (PhotoSpot spot in photoSpots)
        {
            spot.SetActiveSpot(false);
        }

        System.Collections.Generic.List<PhotoSpot> shuffledSpots =
            new System.Collections.Generic.List<PhotoSpot>(photoSpots);

        for (int i = 0; i < shuffledSpots.Count; i++)
        {
            PhotoSpot temp = shuffledSpots[i];
            int randomIndex = Random.Range(i, shuffledSpots.Count);
            shuffledSpots[i] = shuffledSpots[randomIndex];
            shuffledSpots[randomIndex] = temp;
        }

        for (int i = 0; i < 3 && i < shuffledSpots.Count; i++)
        {
            shuffledSpots[i].SetActiveSpot(true);
        }
    }

    void EndGame()
    {
        gameStarted = false;
        gameEnded = true;

        bool won = false;

        if (QuestController_JuanRdz.Instance != null)
        {
            won = QuestController_JuanRdz.Instance.currentPhotos >= QuestController_JuanRdz.Instance.targetPhotos;
        }

        if (resultText != null)
        {
            resultText.gameObject.SetActive(true);

            if (won)
                resultText.text = "Ganaste";
            else
                resultText.text = "Perdiste";
        }

        Debug.Log(won ? "Ganaste" : "Perdiste");
    }

    public void FinishGameAndGoToResults()
    {
        gameStarted = false;
        gameEnded = true;

        if (QuestController_JuanRdz.Instance != null &&
            QuestController_JuanRdz.Instance.currentPhotos >= QuestController_JuanRdz.Instance.targetPhotos &&
            currentTime > 30f)
        {
            QuestController_JuanRdz.Instance.AddSpeedBonus();
        }

        SceneManager.LoadScene("Mini2_Resultados");
    }
}