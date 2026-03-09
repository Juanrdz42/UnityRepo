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
        CheckSceneObjective(SceneManager.GetActiveScene().name);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        RefreshObjectiveTextReference();
        CheckSceneObjective(scene.name);
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

    private void CheckSceneObjective(string sceneName)
    {
        if (sceneName == "Mini2_Bosque")
        {
            if (!IsMissionCompleted("Mini2_Bosque_PhotoMission"))
            {
                UpdatePhotoObjective();
            }
        }
        else if (string.IsNullOrEmpty(currentObjective))
        {
            SetObjective("Ve con el explorador.");
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
        SetObjective("Elige un ecosistema.");
    }

    public void OnBiomeChosen(string biome)
    {
        SetObjective("Habla con el explorador.");
    }

    public void OnMissionStart(string biome)
    {
        if (biome == "terrestre")
        {
            currentPhotos = 0;
            targetPhotos = 3;
            UpdatePhotoObjective();
        }
        else if (biome == "acuatico")
        {
            currentPhotos = 0;
            targetPhotos = 3;
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
            CompleteMission("Mini2_Bosque_PhotoMission");
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
}