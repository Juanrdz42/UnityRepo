using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueController_JuanRdz : MonoBehaviour
{
    public static DialogueController_JuanRdz Instance { get; private set; }
    
    public GameObject dialoguePanel;
    public TMP_Text dialogueText;
    public TMP_Text nameText;
    public Image portraitImage;
    public  Transform choiceContainer;
    public GameObject choiceButtonPrefab;

    private object choiceButton;

    void Start()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        
    }
    
    public void ShowDialogueUI(bool show)
    {
        dialoguePanel.SetActive(show);
    }

    public void SetNPCInfo(string npcName, Sprite npcPortrait)
    {
        nameText.SetText(npcName);
        portraitImage.sprite = npcPortrait;
    }

    public void SetDialogueText(string text)
    {
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
