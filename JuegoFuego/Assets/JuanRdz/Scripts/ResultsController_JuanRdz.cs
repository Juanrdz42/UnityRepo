using UnityEngine;
using TMPro;

public class ResultsController_JuanRdz : MonoBehaviour
{
    public TextMeshProUGUI photo1Text;
    public TextMeshProUGUI photo2Text;
    public TextMeshProUGUI photo3Text;
    public TextMeshProUGUI bonusText;
    public TextMeshProUGUI totalText;

    void Start()
    {
        if (QuestController_JuanRdz.Instance == null)
        {
            Debug.LogWarning("QuestController_JuanRdz.Instance es null en la escena de resultados.");
            return;
        }

        QuestController_JuanRdz quest = QuestController_JuanRdz.Instance;

        if (photo1Text != null)
            photo1Text.text = GetPhotoLine("Primera foto", 0, quest);

        if (photo2Text != null)
            photo2Text.text = GetPhotoLine("Segunda foto", 1, quest);

        if (photo3Text != null)
            photo3Text.text = GetPhotoLine("Tercera foto", 2, quest);

        if (bonusText != null)
            bonusText.text = "Bono de tiempo: " + (quest.speedBonusEarned ? "+5" : "+0");

        if (totalText != null)
            totalText.text = "Total: " + quest.totalScore;
    }

    private string GetPhotoLine(string label, int index, QuestController_JuanRdz quest)
    {
        if (quest.photoResults == null || index >= quest.photoResults.Count)
            return label + ": Sin foto - +0";

        PhotoResultType result = quest.photoResults[index];

        switch (result)
        {
            case PhotoResultType.Perfect:
                return label + ": Perfecta - +5";

            case PhotoResultType.Good:
                return label + ": Buena - +2";

            case PhotoResultType.Bad:
                return label + ": Mala - +0";

            default:
                return label + ": Mala - +0";
        }
    }
}