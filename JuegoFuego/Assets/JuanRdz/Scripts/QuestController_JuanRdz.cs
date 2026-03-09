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

    [Header("Photo Mission")]
    public int currentPhotos = 0;
    public int targetPhotos = 3;

    [Header("Score")]
    public int totalScore = 0;
    public int perfectPhotos = 0;
    public int goodPhotos = 0;
    public int badPhotos = 0;
    public bool speedBonusEarned = false;

    public List<PhotoResultType> photoResults = new List<PhotoResultType>();

    private HashSet<string> completedMissions = new HashSet<string>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
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
        RefreshObjectiveTextReference();

        if (string.IsNullOrEmpty(currentObjective))
        {
            SetObjective("Ve y habla con el explorador.");
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        RefreshObjectiveTextReference();
    }

    private void RefreshObjectiveTextReference()
    {
        GameObject objectiveObject = GameObject.Find("ObjectiveText");

        if (objectiveObject != null)
        {
            objectiveText = objectiveObject.GetComponent<TMP_Text>();
        }
        else
        {
            Debug.LogWarning("No se encontró un objeto llamado 'ObjectiveText' en esta escena.");
        }

        if (objectiveText != null && !string.IsNullOrEmpty(currentObjective))
        {
            objectiveText.text = currentObjective;
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

    public void OnTalkExplorer()
    {
        SetObjective("Elige el bioma en el que más estés interesado.");
    }

    public void OnBiomeChosen(string biome)
    {
        SetObjective("Vuelve a hablar con el explorador para iniciar la toma de fotografías.");
    }

    public void OnMissionStart(string biome)
    {
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
    }

    public void AddPhotoScore(PhotoResultType result)
    {
        photoResults.Add(result);

        switch (result)
        {
            case PhotoResultType.Perfect:
                totalScore += 5;
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
            totalScore += 5;
            speedBonusEarned = true;
        }
    }
}