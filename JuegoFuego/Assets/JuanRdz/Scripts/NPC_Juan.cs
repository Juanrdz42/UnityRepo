using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class NPC : MonoBehaviour, IInteractable
{
    [Header("Dialogues")]
    public NPCDialogue firstDialogueData;
    public NPCDialogue readyDialogueTerrestre;
    public NPCDialogue readyDialogueAcuatico;
    public NPCDialogue postGameDialogueData;

    private NPCDialogue currentDialogueData;
    private DialogueController_JuanRdz dialogueUI;

    private int dialogueIndex;
    private bool isTyping;
    private bool isDialogueActive;

    private bool hasChosenBiome = false;
    private string selectedBiome = "";

    private bool shouldStartMissionAfterDialogue = false;
    private string pendingBiomeScene = "";

    private Coroutine autoCloseCoroutine;

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

        if (isTyping)
        {
            StopAllCoroutines();
            dialogueUI.SetDialogueText(currentDialogueData.dialogueLines[dialogueIndex]);
            isTyping = false;

            if (HasChoiceAtCurrentLine())
            {
                DialogueChoice currentChoice = GetChoiceForCurrentLine();
                if (currentChoice != null)
                    DisplayChoices(currentChoice);
            }

            if (IsEndLine(dialogueIndex))
            {
                autoCloseCoroutine = StartCoroutine(CloseDialogueAfterDelay(2f));
            }

            return;
        }

        if (HasChoiceAtCurrentLine())
            return;

        if (IsEndLine(dialogueIndex))
        {
            EndDialogue();
            return;
        }

        NextLine();
    }

    private void SelectDialogueToUse()
    {
        if (QuestController_JuanRdz.Instance != null && QuestController_JuanRdz.Instance.IsPostGameActive())
        {
            if (postGameDialogueData != null)
            {
                currentDialogueData = postGameDialogueData;
                return;
            }
        }

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

        shouldStartMissionAfterDialogue = false;
        pendingBiomeScene = "";

        if (QuestController_JuanRdz.Instance != null && QuestController_JuanRdz.Instance.IsPostGameActive())
        {
            QuestController_JuanRdz.Instance.ApplyPostGameObjective();
        }
        else if (!hasChosenBiome && QuestController_JuanRdz.Instance != null)
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
        StopTypingAndAutoClose();
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

            if (SFXManager_JuanRdz.Instance != null)
            {
                SFXManager_JuanRdz.Instance.PlayVoice(
                    currentDialogueData.voiceSound,
                    currentDialogueData.voicePitch
                );
            }

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
            autoCloseCoroutine = StartCoroutine(CloseDialogueAfterDelay(2f));
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

    private IEnumerator CloseDialogueAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        EndDialogue();
    }

    private void StopTypingAndAutoClose()
    {
        StopAllCoroutines();
        isTyping = false;
        autoCloseCoroutine = null;
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

        if (QuestController_JuanRdz.Instance != null && QuestController_JuanRdz.Instance.IsPostGameActive())
        {
            if (lowerChoice.Contains("terrest"))
            {
                selectedBiome = "terrestre";
                pendingBiomeScene = "Mini2_Bosque";
                shouldStartMissionAfterDialogue = true;
            }
            else if (lowerChoice.Contains("acu"))
            {
                selectedBiome = "acuatico";
                pendingBiomeScene = "Mini2_Lago";
                shouldStartMissionAfterDialogue = true;
            }

            dialogueIndex = nextIndex;
            dialogueUI.ClearChoices();
            DisplayCurrentLine();
            return;
        }

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
            else if (lowerChoice.Contains("acu"))
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
        StopTypingAndAutoClose();
        isDialogueActive = false;
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