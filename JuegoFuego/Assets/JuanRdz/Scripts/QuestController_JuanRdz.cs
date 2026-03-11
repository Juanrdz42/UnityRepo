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

    [Header("Quest State")]
    public string currentObjective;

    [Header("Post Game")]
    public bool isPostGameActive = false;
    public string postGameObjective = "";

    [Header("Photo Mission")]
    public int currentPhotos = 0;
    public int targetPhotos = 3;

    [Header("Score")]
    public int totalScore = 0;
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

    public bool mini3Unlocked = false;
    public bool mini4Unlocked = false;

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
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(RefreshObjectiveTextNextFrame());
    }

    private IEnumerator RefreshObjectiveTextNextFrame()
    {
        yield return null;
        RefreshObjectiveTextReference();
    }

    private void RefreshObjectiveTextReference()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        if ((sceneName == "Mini2" || sceneName == "Mini2_Lago") && isPostGameActive)
        {
            GameObject panelObject = GameObject.Find("ObjectivePanel");

            if (panelObject != null)
                panelObject.SetActive(false);

            return;
        }

        if (objectiveText == null)
        {
            GameObject objectiveObject = GameObject.Find("QuestText");

            if (objectiveObject != null)
            {
                objectiveText = objectiveObject.GetComponent<TMP_Text>();
            }
        }

        if (objectiveText != null)
        {
            objectiveText.text = currentObjective;
        }
        else
        {
            Debug.LogWarning("No se encontró referencia al texto del objetivo en la escena " + sceneName);
        }
    }

    public void SetObjective(string newObjective)
    {
        currentObjective = newObjective;

        if (objectiveText != null)
        {
            objectiveText.text = currentObjective;
        }
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
        if (isPostGameActive)
        {
            ApplyPostGameObjective();
            return;
        }

        currentPhotos = 0;
        targetPhotos = 3;
        ResetScore();

        if (biome == "terrestre")
        {
            SetObjective("Ve al bosque y toma 3 fotos.");
        }
        else if (biome == "acuatico")
        {
            SetObjective("Ve al lago y toma 3 fotos.");
        }
    }

    public void AddPhoto()
    {
        currentPhotos++;

        if (currentPhotos > targetPhotos)
            currentPhotos = targetPhotos;

        UpdatePhotoObjective();

        if (currentPhotos >= targetPhotos)
        {
            CompleteMission("PhotoMission");
        }
    }

    private void UpdatePhotoObjective()
    {
        SetObjective("Toma " + targetPhotos + " fotos (" + currentPhotos + "/" + targetPhotos + ")");
    }

    public void CompleteMission(string missionId)
    {
        if (!completedMissions.Contains(missionId))
        {
            completedMissions.Add(missionId);
        }
    }

    public bool IsMissionCompleted(string missionId)
    {
        return completedMissions.Contains(missionId);
    }

    public void ResetScore()
    {
        totalScore = 0;
        perfectPhotos = 0;
        goodPhotos = 0;
        badPhotos = 0;
        speedBonusEarned = false;
        photoResults.Clear();
        lastTimeRemaining = 0f;
    }

    public void AddPhotoScore(PhotoResultType result)
    {
        photoResults.Add(result);

        switch (result)
        {
            case PhotoResultType.Perfect:
                totalScore += 3;
                perfectPhotos++;
                break;

            case PhotoResultType.Good:
                totalScore += 2;
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
            totalScore += 1;
            speedBonusEarned = true;
        }
    }

    public void RegisterTime(float timeRemaining)
    {
        lastTimeRemaining = timeRemaining;

        if (timeRemaining > bestTime)
        {
            bestTime = timeRemaining;
        }
    }

    private void CheckMiniGameUnlocks()
    {
        if (bosqueCompleted || lagoCompleted)
        {
            mini3Unlocked = true;
            mini4Unlocked = true;

            PlayerPrefs.SetInt("mini3Unlocked", 1);
            PlayerPrefs.SetInt("mini4Unlocked", 1);
        }
    }

    private void LoadProgress()
    {
        bosqueCompleted = PlayerPrefs.GetInt("bosqueCompleted", 0) == 1;
        lagoCompleted = PlayerPrefs.GetInt("lagoCompleted", 0) == 1;

        mini3Unlocked = PlayerPrefs.GetInt("mini3Unlocked", 0) == 1;
        mini4Unlocked = PlayerPrefs.GetInt("mini4Unlocked", 0) == 1;
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

        CheckMiniGameUnlocks();
        PlayerPrefs.Save();
    }
}