using UnityEngine;
using System.Collections;

public class NPC : MonoBehaviour, IInteractable
{
    public NPCDialogue dialogueData;
    private DialogueController_JuanRdz dialogueUI;

    private int dialogueIndex;
    private bool isTyping;
    private bool isDialogueActive;

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
        if (dialogueData == null || dialogueUI == null)
            return;

        if (!isDialogueActive)
        {
            StartDialogue();
            return;
        }

        // Si hay una decisión en esta línea, no avanzar con Z
        if (HasChoiceAtCurrentLine())
        {
            return;
        }

        // Si todavía se está escribiendo, completar la línea
        if (isTyping)
        {
            StopAllCoroutines();
            dialogueUI.SetDialogueText(dialogueData.dialogueLines[dialogueIndex]);
            isTyping = false;
            return;
        }

        // Si no hay decisión y ya terminó de escribirse, avanzar
        NextLine();
    }

    private void StartDialogue()
    {
        isDialogueActive = true;
        dialogueIndex = 0;

        dialogueUI.SetNPCInfo(dialogueData.npcName, dialogueData.npcPortrait);
        dialogueUI.ShowDialogueUI(true);
        dialogueUI.ClearChoices();

        DisplayCurrentLine();
    }

    private void NextLine()
    {
        dialogueUI.ClearChoices();

        dialogueIndex++;

        if (dialogueIndex < dialogueData.dialogueLines.Length)
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

        string currentLine = dialogueData.dialogueLines[dialogueIndex];

        foreach (char letter in currentLine)
        {
            dialogueUI.SetDialogueText(dialogueUI.dialogueText.text + letter);
            yield return new WaitForSeconds(dialogueData.typingSpeed);
        }

        isTyping = false;

        // Cuando termina de escribir, revisar si esta línea tiene decisiones
        DialogueChoice currentChoice = GetChoiceForCurrentLine();
        if (currentChoice != null)
        {
            DisplayChoices(currentChoice);
            yield break;
        }

        // Si no hay decisiones, revisar auto avance
        if (dialogueData.autoProgressLines != null &&
            dialogueData.autoProgressLines.Length > dialogueIndex &&
            dialogueData.autoProgressLines[dialogueIndex])
        {
            yield return new WaitForSeconds(dialogueData.autoProgressDelay);
            NextLine();
        }
    }

    private DialogueChoice GetChoiceForCurrentLine()
    {
        if (dialogueData.Choices == null)
            return null;

        foreach (DialogueChoice dialogueChoice in dialogueData.Choices)
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

    private void DisplayChoices(DialogueChoice dialogueChoice)
    {
        dialogueUI.ClearChoices();

        for (int i = 0; i < dialogueChoice.choices.Length; i++)
        {
            int nextIndex = dialogueChoice.nextDialogueIndexes[i];
            string choiceText = dialogueChoice.choices[i];

            dialogueUI.CreateChoiceButton(choiceText, () => ChooseOption(nextIndex));
        }
    }

    private void ChooseOption(int nextIndex)
    {
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
    }
}