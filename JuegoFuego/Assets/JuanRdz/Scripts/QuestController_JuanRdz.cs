using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestController_JuanRdz : MonoBehaviour
{
    public static QuestController_JuanRdz Instance;

    [Header("UI")]
    public TMP_Text objectiveText;
    public TMP_Text pointsText;

    [Header("Quest State")]
    public string currentObjective;

    [Header("Post Game")]
    public bool isPostGameActive = false;
    public string postGameObjective = "";

    [Header("Photo Mission")]
    public int currentPhotos = 0;
    public int targetPhotos = 3;

    [Header("Global Score")]
    public int playerPoints = 0;

    [Header("Run Score")]
    public int runScore = 0;
    public bool runScoreCommitted = false;

    [Header("Photo Results")]
    public int perfectPhotos = 0;
    public int goodPhotos = 0;
    public int badPhotos = 0;
    public bool speedBonusEarned = false;

    [Header("Stats")]
    public float lastTimeRemaining = 0f;
    public float bestTime = 0f;

    [Header("Player Progress")]
    public bool bosqueCompleted = false;
    public bool lagoCompleted = false;

    public List<PhotoResultType> photoResults = new List<PhotoResultType>();

    private HashSet<string> completedMissions = new HashSet<string>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        transform.SetParent(null);
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        if (string.IsNullOrEmpty(currentObjective))
        {
            currentObjective = "Ve y habla con el explorador.";
        }

        LoadProgress();
        StartCoroutine(RefreshObjectiveTextNextFrame());
        StartCoroutine(RefreshPointsTextNextFrame());
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Mini2")
        {
            currentPhotos = 0;
            targetPhotos = 3;

            if (isPostGameActive)
                ApplyPostGameObjective();
            else
                SetObjective("Ve y habla con el explorador.");
        }

        StartCoroutine(RefreshObjectiveTextNextFrame());
        StartCoroutine(RefreshPointsTextNextFrame());
    }

    private IEnumerator RefreshObjectiveTextNextFrame()
    {
        yield return null;
        RefreshObjectiveTextReference();
    }

    private IEnumerator RefreshPointsTextNextFrame()
    {
        yield return null;
        RefreshPointsTextReference();
    }

    private void RefreshObjectiveTextReference()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        GameObject panelObject = GameObject.Find("ObjectivePanel");

        if (sceneName == "Mini2_Resultados")
        {
            if (panelObject != null)
                panelObject.SetActive(false);

            return;
        }

        if (panelObject != null)
            panelObject.SetActive(true);

        if (objectiveText == null)
        {
            GameObject objectiveObject = GameObject.Find("QuestText");
            if (objectiveObject != null)
                objectiveText = objectiveObject.GetComponent<TMP_Text>();
        }

        if (objectiveText != null)
            objectiveText.text = currentObjective;
    }

    private void RefreshPointsTextReference()
    {
        pointsText = null;

        GameObject pointsObject = GameObject.Find("Points");
        if (pointsObject != null)
            pointsText = pointsObject.GetComponent<TMP_Text>();

        UpdatePointsUI();
    }

    public void UpdatePointsUI()
    {
        if (pointsText != null)
            pointsText.text = playerPoints.ToString();
    }

    public void SetObjective(string newObjective)
    {
        currentObjective = newObjective;

        if (objectiveText != null)
            objectiveText.text = currentObjective;
    }

    public void ActivatePostGame()
    {
        isPostGameActive = true;
        ApplyPostGameObjective();
    }

    public void DeactivatePostGame()
    {
        isPostGameActive = false;
    }

    public bool IsPostGameActive()
    {
        return isPostGameActive;
    }

    public void ApplyPostGameObjective()
    {
        SetObjective(postGameObjective);
    }

    public void OnTalkExplorer()
    {
        if (isPostGameActive)
        {
            ApplyPostGameObjective();
            return;
        }

        SetObjective("Elige el bioma en el que más estés interesado.");
    }

    public void OnBiomeChosen(string biome)
    {
        if (isPostGameActive)
        {
            ApplyPostGameObjective();
            return;
        }

        SetObjective("Vuelve a hablar con el explorador para iniciar la toma de fotografías.");
    }

    public void OnMissionStart(string biome)
    {
        isPostGameActive = false;

        currentPhotos = 0;
        targetPhotos = 3;
        ResetRunScore();

        if (biome == "terrestre")
            SetObjective("Ve al bosque y toma 3 fotos.");
        else if (biome == "acuatico")
            SetObjective("Ve al lago y toma 3 fotos.");

        UpdatePointsUI();
        StartCoroutine(RefreshObjectiveTextNextFrame());
    }

    public void AddPhoto()
    {
        currentPhotos++;

        if (currentPhotos > targetPhotos)
            currentPhotos = targetPhotos;

        UpdatePhotoObjective();

        if (currentPhotos >= targetPhotos)
            CompleteMission("PhotoMission");
    }

    private void UpdatePhotoObjective()
    {
        SetObjective("Toma " + targetPhotos + " fotos (" + currentPhotos + "/" + targetPhotos + ")");
    }

    public void CompleteMission(string missionId)
    {
        if (!completedMissions.Contains(missionId))
            completedMissions.Add(missionId);
    }

    public bool IsMissionCompleted(string missionId)
    {
        return completedMissions.Contains(missionId);
    }

    public void ResetRunScore()
    {
        runScore = 0;
        perfectPhotos = 0;
        goodPhotos = 0;
        badPhotos = 0;
        speedBonusEarned = false;
        photoResults.Clear();
        lastTimeRemaining = 0f;
        runScoreCommitted = false;
    }

    public void AddPhotoScore(PhotoResultType result)
    {
        photoResults.Add(result);

        switch (result)
        {
            case PhotoResultType.Perfect:
                runScore += 3;
                perfectPhotos++;
                break;

            case PhotoResultType.Good:
                runScore += 2;
                goodPhotos++;
                break;

            case PhotoResultType.Bad:
                badPhotos++;
                break;
        }
    }

    public void AddSpeedBonus()
    {
        if (!speedBonusEarned)
        {
            runScore += 1;
            speedBonusEarned = true;
        }
    }

    public void RegisterTime(float timeRemaining)
    {
        lastTimeRemaining = timeRemaining;

        if (timeRemaining > bestTime)
            bestTime = timeRemaining;
    }

    public void CommitRunScoreIfWon(bool won)
    {
        if (runScoreCommitted)
            return;

        if (won)
        {
            playerPoints += runScore;
            SavePlayerPoints();
            UpdatePointsUI();
        }

        runScoreCommitted = true;
    }

    private void SavePlayerPoints()
    {
        PlayerPrefs.SetInt("playerPoints", playerPoints);
        PlayerPrefs.Save();
    }

    private void LoadProgress()
    {
        bosqueCompleted = PlayerPrefs.GetInt("bosqueCompleted", 0) == 1;
        lagoCompleted = PlayerPrefs.GetInt("lagoCompleted", 0) == 1;
        playerPoints = PlayerPrefs.GetInt("playerPoints", 0);
    }

    public void RegisterBiomeCompletion(string biome)
    {
        if (biome == "terrestre")
        {
            bosqueCompleted = true;
            PlayerPrefs.SetInt("bosqueCompleted", 1);
        }

        if (biome == "acuatico")
        {
            lagoCompleted = true;
            PlayerPrefs.SetInt("lagoCompleted", 1);
        }

        PlayerPrefs.Save();
    }
}