using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    public TMP_Text dialogueText;
    public TMP_Text nameText;
    public GameObject dialogueBox;
    public GameObject nameBox;

    public string[] dialogueLines;
    private string restOfSentence;

    public int currentLine;
    public bool justStarted;

    private string questToMark;
    private bool markQuestComplete;
    private bool shouldMarkQuest;


    void Start()
    {
        instance = this;

        //dialogueText.text = dialogLines[currentLine];
    }

    
    void Update()
    {
        if(dialogueBox.activeInHierarchy)
        {
            if(Input.GetButtonUp("Fire1"))
            {
                if(!justStarted)
                {
                    currentLine++;

                    if(currentLine >= dialogueLines.Length)
                    {
                        //this is the end of the dialogue
                        dialogueBox.SetActive(false);

                        GameManager.instance.dialogueActive = false;

                        //if quests need to be updated after dialogue
                        if(shouldMarkQuest)
                        {
                            shouldMarkQuest = false;

                            if(markQuestComplete)
                            {
                                QuestManager.instance.MarkQuestComplete(questToMark);
                            }
                            else
                            {
                                QuestManager.instance.MarkQuestIncomplete(questToMark);
                            }
                        }
                    }
                    else
                    {
                        //this is moving through the dialogue
                        CheckIfName();

                        dialogueText.text = dialogueLines[currentLine];
                    }
                }
                else
                {
                    justStarted = false;
                }
            }
        }
    }

    public void ShowDialogue(string[] newLines, bool isPerson)
    {
        //The dialogue begins. First it sets up the new lines from the activator, then it sets the current line to 0, then the textbox is set active, then it checks if it is a name line, then it sets justStarted for the dialogue sequence, then it sets the dialogue from zero to current line, then shuts off character controller movement.

        dialogueLines = newLines;

        currentLine = 0;

        dialogueBox.SetActive(true);

        //space for function timing

        CheckIfName();

        justStarted = true;
        
        dialogueText.text = dialogueLines[currentLine];

        nameBox.SetActive(isPerson);

        GameManager.instance.dialogueActive = true;
    }

    public void CheckIfName()
    {
        if(dialogueLines[currentLine].StartsWith("n-"))
        {
            nameText.text = dialogueLines[currentLine].Replace("n-", "");
            currentLine++;
        }
    }

    public void ShouldActivateQuestAtEnd(string questName, bool markComplete)
    {
        questToMark = questName;
        markQuestComplete = markComplete;

        shouldMarkQuest = true;
    }
}
