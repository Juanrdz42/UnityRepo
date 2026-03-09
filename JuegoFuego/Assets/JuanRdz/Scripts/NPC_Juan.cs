using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class NPC : MonoBehaviour, IInteractable
{
    [Header("Dialogues")]
    public NPCDialogue firstDialogueData;
    public NPCDialogue readyDialogueTerrestre;
    public NPCDialogue readyDialogueAcuatico;

    private NPCDialogue currentDialogueData;
    private DialogueController_JuanRdz dialogueUI;

    private int dialogueIndex;
    private bool isTyping;
    private bool isDialogueActive;

    private bool hasChosenBiome = false;
    private string selectedBiome = "";

    private bool shouldStartMissionAfterDialogue = false;
    private string pendingBiomeScene = "";

    void Start()
    {
        dialogueUI = DialogueController_JuanRdz.Instance;
    }

    public bool CanInteract()
    {
        return true;
    }

    public void Interact()
    {
        if (dialogueUI == null)
            return;

        if (!isDialogueActive)
        {
            SelectDialogueToUse();
            StartDialogue();
            return;
        }

        if (HasChoiceAtCurrentLine())
            return;

        if (isTyping)
        {
            StopAllCoroutines();
            dialogueUI.SetDialogueText(currentDialogueData.dialogueLines[dialogueIndex]);
            isTyping = false;
            return;
        }

        NextLine();
    }

    private void SelectDialogueToUse()
    {
        if (!hasChosenBiome)
        {
            currentDialogueData = firstDialogueData;
        }
        else
        {
            if (selectedBiome == "terrestre")
                currentDialogueData = readyDialogueTerrestre;
            else if (selectedBiome == "acuatico")
                currentDialogueData = readyDialogueAcuatico;
        }
    }

    private void StartDialogue()
    {
        if (currentDialogueData == null)
            return;

        isDialogueActive = true;
        dialogueIndex = 0;

        if (!hasChosenBiome && QuestController_JuanRdz.Instance != null)
        {
            QuestController_JuanRdz.Instance.OnTalkExplorer();
        }

        dialogueUI.SetNPCInfo(currentDialogueData.npcName, currentDialogueData.npcPortrait);
        dialogueUI.ShowDialogueUI(true);
        dialogueUI.ClearChoices();

        DisplayCurrentLine();
    }

    private void NextLine()
    {
        dialogueUI.ClearChoices();

        if (IsEndLine(dialogueIndex))
        {
            EndDialogue();
            return;
        }

        dialogueIndex++;

        if (dialogueIndex < currentDialogueData.dialogueLines.Length)
        {
            DisplayCurrentLine();
        }
        else
        {
            EndDialogue();
        }
    }

    private void DisplayCurrentLine()
    {
        StopAllCoroutines();
        dialogueUI.ClearChoices();
        StartCoroutine(TypeLine());
    }

    private IEnumerator TypeLine()
    {
        isTyping = true;
        dialogueUI.SetDialogueText("");

        string currentLine = currentDialogueData.dialogueLines[dialogueIndex];

        foreach (char letter in currentLine)
        {
            dialogueUI.SetDialogueText(dialogueUI.dialogueText.text + letter);
            yield return new WaitForSeconds(currentDialogueData.typingSpeed);
        }

        isTyping = false;

        DialogueChoice currentChoice = GetChoiceForCurrentLine();
        if (currentChoice != null)
        {
            DisplayChoices(currentChoice);
            yield break;
        }

        if (IsEndLine(dialogueIndex))
        {
            yield break;
        }

        if (currentDialogueData.autoProgressLines != null &&
            currentDialogueData.autoProgressLines.Length > dialogueIndex &&
            currentDialogueData.autoProgressLines[dialogueIndex])
        {
            yield return new WaitForSeconds(currentDialogueData.autoProgressDelay);
            NextLine();
        }
    }

    private DialogueChoice GetChoiceForCurrentLine()
    {
        if (currentDialogueData.Choices == null)
            return null;

        foreach (DialogueChoice dialogueChoice in currentDialogueData.Choices)
        {
            if (dialogueChoice.dialogueIndex == dialogueIndex)
            {
                return dialogueChoice;
            }
        }

        return null;
    }

    private bool HasChoiceAtCurrentLine()
    {
        return GetChoiceForCurrentLine() != null;
    }

    private bool IsEndLine(int index)
    {
        return currentDialogueData.endDialogueLines != null &&
               index < currentDialogueData.endDialogueLines.Length &&
               currentDialogueData.endDialogueLines[index];
    }

    private void DisplayChoices(DialogueChoice dialogueChoice)
    {
        dialogueUI.ClearChoices();

        for (int i = 0; i < dialogueChoice.choices.Length; i++)
        {
            int nextIndex = dialogueChoice.nextDialogueIndexes[i];
            string choiceText = dialogueChoice.choices[i];

            dialogueUI.CreateChoiceButton(choiceText, () => ChooseOption(choiceText, nextIndex));
        }
    }

    private void ChooseOption(string choiceText, int nextIndex)
    {
        string lowerChoice = choiceText.ToLower();

        if (!hasChosenBiome)
        {
            if (lowerChoice.Contains("terrest"))
            {
                selectedBiome = "terrestre";
                hasChosenBiome = true;

                if (QuestController_JuanRdz.Instance != null)
                {
                    QuestController_JuanRdz.Instance.OnBiomeChosen("terrestre");
                }
            }
            else if (lowerChoice.Contains("acuat"))
            {
                selectedBiome = "acuatico";
                hasChosenBiome = true;

                if (QuestController_JuanRdz.Instance != null)
                {
                    QuestController_JuanRdz.Instance.OnBiomeChosen("acuatico");
                }
            }

            dialogueIndex = nextIndex;
            dialogueUI.ClearChoices();
            DisplayCurrentLine();
            return;
        }
        else
        {
            if (lowerChoice.Contains("sí") || lowerChoice.Contains("si"))
            {
                if (QuestController_JuanRdz.Instance != null)
                {
                    QuestController_JuanRdz.Instance.OnMissionStart(selectedBiome);
                }

                shouldStartMissionAfterDialogue = true;

                if (selectedBiome == "terrestre")
                {
                    pendingBiomeScene = "Mini2_Bosque";
                }
                else if (selectedBiome == "acuatico")
                {
                    pendingBiomeScene = "Mini2_Lago";
                }

                dialogueIndex = nextIndex;
                dialogueUI.ClearChoices();
                DisplayCurrentLine();
                return;
            }
        }

        dialogueIndex = nextIndex;
        dialogueUI.ClearChoices();
        DisplayCurrentLine();
    }

    public void EndDialogue()
    {
        StopAllCoroutines();
        isDialogueActive = false;
        isTyping = false;
        dialogueUI.ClearChoices();
        dialogueUI.SetDialogueText("");
        dialogueUI.ShowDialogueUI(false);

        if (shouldStartMissionAfterDialogue && !string.IsNullOrEmpty(pendingBiomeScene))
        {
            string sceneToLoad = pendingBiomeScene;

            shouldStartMissionAfterDialogue = false;
            pendingBiomeScene = "";

            SceneManager.LoadScene(sceneToLoad);
        }
    }
}