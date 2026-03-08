using TMPro;
using UnityEngine;

public class QuestController_JuanRdz : MonoBehaviour
{
    public static QuestController_JuanRdz Instance;

    [Header("UI")]
    public TMP_Text objectiveText;

    [Header("Quest State")]
    public string currentObjective;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SetObjective("Ve con el explorador.");
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
        SetObjective("Elige qué ecosistema te interesaría investigar.");
    }

    public void OnBiomeChosen(string biome)
    {
        if (biome == "terrestre")
        {
            SetObjective("Habla con el explorador para iniciar la misión.");
        }
        else if (biome == "acuatico")
        {
            SetObjective("Habla con el explorador para iniciar la misión.");
        }
    }

    public void OnMissionStart(string biome)
    {
        if (biome == "terrestre")
        {
            SetObjective("Dirígete al bosque y toma 3 fotografías.");
        }
        else if (biome == "acuatico")
        {
            SetObjective("Dirígete al lago y toma 3 fotografías.");
        }
    }

    public void OnPhotoProgress(int current, int total)
    {
        SetObjective("Fotografías tomadas: " + current + "/" + total);
    }

    public void OnMissionComplete()
    {
        SetObjective("¡Objetivo completado!");
    }
}