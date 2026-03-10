using UnityEngine;
using TMPro;

public class ResultsController_JuanRdz : MonoBehaviour
{
    public TextMeshProUGUI photo1Text;
    public TextMeshProUGUI photo2Text;
    public TextMeshProUGUI photo3Text;
    public TextMeshProUGUI bonusText;
    public TextMeshProUGUI totalText;
    public TextMeshProUGUI failText;
    public TextMeshProUGUI accuracyText;
    public TextMeshProUGUI bestTimeText;

    void Start()
    {
        if (QuestController_JuanRdz.Instance == null)
        {
            Debug.LogWarning("QuestController_JuanRdz.Instance es null en la escena de resultados.");
            return;
        }

        QuestController_JuanRdz quest = QuestController_JuanRdz.Instance;
        bool failedByTime = quest.currentPhotos < quest.targetPhotos;

        int goodShots = quest.perfectPhotos + quest.goodPhotos;
        int totalShots = quest.perfectPhotos + quest.goodPhotos + quest.badPhotos;

        float accuracy = 0f;
        if (totalShots > 0)
        {
            accuracy = (float)goodShots / totalShots * 100f;
        }

        if (failedByTime)
        {
            if (failText != null)
                failText.gameObject.SetActive(true);

            if (photo1Text != null)
                photo1Text.gameObject.SetActive(false);

            if (photo2Text != null)
                photo2Text.gameObject.SetActive(false);

            if (photo3Text != null)
                photo3Text.gameObject.SetActive(false);

            if (bonusText != null)
                bonusText.gameObject.SetActive(false);

            if (totalText != null)
            {
                totalText.gameObject.SetActive(true);
                totalText.text = "Total: 0";
            }

            if (accuracyText != null)
            {
                accuracyText.gameObject.SetActive(true);
                accuracyText.text = "Precisión: " + accuracy.ToString("0") + "%";
            }

            if (bestTimeText != null)
            {
                bestTimeText.gameObject.SetActive(true);
                bestTimeText.text = "Mejor tiempo: " + quest.bestTime.ToString("0") + "s";
            }

            return;
        }

        if (failText != null)
            failText.gameObject.SetActive(false);

        if (photo1Text != null)
        {
            photo1Text.gameObject.SetActive(true);
            photo1Text.text = GetPhotoLine("Primera foto", 0, quest);
        }

        if (photo2Text != null)
        {
            photo2Text.gameObject.SetActive(true);
            photo2Text.text = GetPhotoLine("Segunda foto", 1, quest);
        }

        if (photo3Text != null)
        {
            photo3Text.gameObject.SetActive(true);
            photo3Text.text = GetPhotoLine("Tercera foto", 2, quest);
        }

        if (bonusText != null)
        {
            bonusText.gameObject.SetActive(true);
            bonusText.text = "Bono de tiempo: " + (quest.speedBonusEarned ? "+5" : "+0");
        }

        if (totalText != null)
        {
            totalText.gameObject.SetActive(true);
            totalText.text = "Total: " + quest.totalScore;
        }

        if (accuracyText != null)
        {
            accuracyText.gameObject.SetActive(true);
            accuracyText.text = "Precisión: " + accuracy.ToString("0") + "%";
        }

        if (bestTimeText != null)
        {
            bestTimeText.gameObject.SetActive(true);
            bestTimeText.text = "Mejor tiempo: " + quest.bestTime.ToString("0") + "s";
        }
    }

    private string GetPhotoLine(string label, int index, QuestController_JuanRdz quest)
    {
        if (quest.photoResults == null || index >= quest.photoResults.Count)
            return label + ": Sin foto +0";

        PhotoResultType result = quest.photoResults[index];

        switch (result)
        {
            case PhotoResultType.Perfect:
                return label + ": Perfecta +5";

            case PhotoResultType.Good:
                return label + ": Buena +2";

            case PhotoResultType.Bad:
                return label + ": Podría ser mejor +0";

            default:
                return label + ": Podría ser mejor +0";
        }
    }
}