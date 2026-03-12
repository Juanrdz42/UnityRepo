using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueController_JuanRdz : MonoBehaviour
{
    public static DialogueController_JuanRdz Instance { get; private set; }

    [Header("UI")]
    public GameObject dialoguePanel;
    public TMP_Text dialogueText;
    public TMP_Text nameText;
    public Image portraitImage;
    public Transform choiceContainer;
    public GameObject choiceButtonPrefab;

    [Header("Player")]
    public PlayerMove playerMove;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);
    }

    public void ShowDialogueUI(bool show)
    {
        if (dialoguePanel != null)
            dialoguePanel.SetActive(show);

        if (playerMove != null)
            playerMove.SetMovementEnabled(!show);
    }

    public void SetNPCInfo(string npcName, Sprite npcPortrait)
    {
        if (nameText != null)
            nameText.SetText(npcName);

        if (portraitImage != null)
            portraitImage.sprite = npcPortrait;
    }

    public void SetDialogueText(string text)
    {
        if (dialogueText != null)
            dialogueText.text = text;
    }

    public void ClearChoices()
    {
        foreach (Transform child in choiceContainer)
        {
            Destroy(child.gameObject);
        }
    }

    public GameObject CreateChoiceButton(string choiceText, UnityEngine.Events.UnityAction onClick)
    {
        GameObject buttonObj = Instantiate(choiceButtonPrefab, choiceContainer);
        buttonObj.GetComponentInChildren<TMP_Text>().text = choiceText;
        buttonObj.GetComponent<Button>().onClick.AddListener(onClick);
        return buttonObj;
    }
}