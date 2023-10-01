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
    public GameObject dialogueBoxSprite;
    public GameObject nameBox;
    public Image portrait;
    public Image rightPortrait;

    public DialogueLines[] newDialogueLines;
    
    private string restOfSentence;
    public CharStats[] characterList;

    public int currentLine;
    public bool justStarted;

    private string questToMark;
    private bool markQuestComplete;
    private bool shouldMarkQuest;

    public Image vignetteImage;
    public bool isInDialogue;

    public float textSpeed = 0.01f;
    [SerializeField] [TextArea] private string[] textInfo;
    private bool textScrollComplete = true;

    void Start()
    {
        instance = this;
    }

    
    void Update()
    {
        if(dialogueBox.activeInHierarchy)
        {
            if(Input.GetButtonUp("Fire1") && textScrollComplete)
            {
                if(!justStarted)
                {
                    currentLine++;

                    if(currentLine >= newDialogueLines.Length)
                    {
                        //this is the end of the dialogue
                        dialogueBox.SetActive(false);
                        portrait.gameObject.SetActive(false);
                        rightPortrait.gameObject.SetActive(false);

                        isInDialogue = false;

                        if(!PlayerController.instance.isIndoors)
                        {
                            vignetteImage.gameObject.SetActive(false);
                        }

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
                        
                        CheckCharacter();
                        CheckExpressions();

                        dialogueText.text = newDialogueLines[currentLine].lineText;
                        textInfo[currentLine] = dialogueText.text;
                        ActiveTextScrolling();

                        if(newDialogueLines[currentLine].twoCharacters)
                        {
                            rightPortrait.gameObject.SetActive(true);
                        }

                        //Handles box flipping
                        if(newDialogueLines[currentLine].leftTurn == false)
                        {
                            dialogueBoxSprite.transform.eulerAngles = new Vector3(0, 180, 0);
                            dialogueText.transform.localPosition = new Vector3(-60, 1, 0);
                        }
                        else
                        {
                            dialogueBoxSprite.transform.eulerAngles = new Vector3(0, 0, 0);
                            dialogueText.transform.localPosition = new Vector3(42, 1, 0);
                        }
                    }
                }
                else
                {
                    justStarted = false;
                }
            }
        }

        //Handles Vignette
        if(PlayerController.instance.isIndoors)
        {
            vignetteImage.gameObject.SetActive(true);
        }

        if(!PlayerController.instance.isIndoors && !isInDialogue)
        {
            vignetteImage.gameObject.SetActive(false);
        }
    }

    public void ShowDialogue(DialogueLines[] newLines, bool isPerson)
    {
        //The dialogue begins. First it sets up the new lines from the activator, then it sets the current line to 0, then the textbox is set active, then it checks if it is a name line, then it sets justStarted for the dialogue sequence, then it sets the dialogue from zero to current line, then shuts off character controller movement.
        isInDialogue = true;

        newDialogueLines = newLines;

        currentLine = 0;

        dialogueBox.SetActive(true);
        portrait.gameObject.SetActive(true);
        vignetteImage.gameObject.SetActive(true);
        //Later we can set a bool for whether or not there are two people in a scene
        if(newDialogueLines[currentLine].twoCharacters)
        {
            rightPortrait.gameObject.SetActive(true);
        }

        //space for function timing
        CheckCharacter();
        CheckExpressions();


        if(newDialogueLines[currentLine].leftTurn == false)
        {
            dialogueBoxSprite.transform.eulerAngles = new Vector3(0, 180, 0);
            dialogueText.transform.localPosition = new Vector3(-60, 1, 0);
        }
        else
        {
            dialogueBoxSprite.transform.eulerAngles = new Vector3(0, 0, 0);
            dialogueText.transform.localPosition = new Vector3(42, 1, 0);
        }

        justStarted = true;
        
        dialogueText.text = newDialogueLines[currentLine].lineText;
        textInfo[currentLine] = dialogueText.text;
        ActiveTextScrolling();

        nameBox.SetActive(isPerson);

        GameManager.instance.dialogueActive = true;
    }

    public void ActiveTextScrolling()
    {
        StartCoroutine(AnimateText());
    }

    IEnumerator AnimateText()
    {
        //When text is scrolling, a player can't progress through the dialogue. I also want to set it so a noise plays for every letter.
        textScrollComplete = false;
        for (int i = 0; i < textInfo[currentLine].Length + 1; i++)
        {
            dialogueText.text = textInfo[currentLine].Substring(0, i);
            
            if(nameText.text == "Junebug")
            {
                AudioManager.instance.PlaySFX(9);
            }

            if(nameText.text == "Talula")
            {
                AudioManager.instance.PlaySFX(10);
            }

            yield return new WaitForSeconds(textSpeed);
        }
        textScrollComplete = true;
    }
    

    public void ShouldActivateQuestAtEnd(string questName, bool markComplete)
    {
        questToMark = questName;
        markQuestComplete = markComplete;

        shouldMarkQuest = true;
    }

    public void CheckCharacter()
    {
        //SET THE LEFT CHARACTER
        switch (newDialogueLines[currentLine].currentCharacter)
        {
            case CharacterToShow.Junebug :

                for(int i = 0; i < characterList.Length; i++)
                {
                    if(characterList[i].characterName == "Junebug")
                    {
                        newDialogueLines[currentLine].selectedCharacter = characterList[i];
                        nameText.text = newDialogueLines[currentLine].selectedCharacter.characterName;
                    }
                }
                break;
            case CharacterToShow.Talula :

                for(int i = 0; i < characterList.Length; i++)
                {
                    if(characterList[i].characterName == "Talula")
                    {
                        newDialogueLines[currentLine].selectedCharacter = characterList[i];
                        nameText.text = newDialogueLines[currentLine].selectedCharacter.characterName;
                    }
                }
                break;
            case CharacterToShow.Grey :

                for(int i = 0; i < characterList.Length; i++)
                {
                    if(characterList[i].characterName == "Grey")
                    {
                        newDialogueLines[currentLine].selectedCharacter = characterList[i];
                        nameText.text = newDialogueLines[currentLine].selectedCharacter.characterName;
                    }
                }
                break;
            default :
                Debug.LogError("Character Does Not Exist!");
                break;
        }

        //SET THE RIGHT CHARACTER
        switch (newDialogueLines[currentLine].rightCharacter)
        {
            case CharacterToShow.Junebug :

                for(int i = 0; i < characterList.Length; i++)
                {
                    if(characterList[i].characterName == "Junebug")
                    {
                        newDialogueLines[currentLine].rightSelectedCharacter = characterList[i];

                        if(newDialogueLines[currentLine].leftTurn == false)
                        {
                            nameText.text = newDialogueLines[currentLine].rightSelectedCharacter.characterName;
                        }
                    }
                }
                break;
            case CharacterToShow.Talula :

                for(int i = 0; i < characterList.Length; i++)
                {
                    if(characterList[i].characterName == "Talula")
                    {
                        newDialogueLines[currentLine].rightSelectedCharacter = characterList[i];

                        if(newDialogueLines[currentLine].leftTurn == false)
                        {
                            nameText.text = newDialogueLines[currentLine].rightSelectedCharacter.characterName;
                        }
                    }
                }
                break;
            case CharacterToShow.Grey :

                for(int i = 0; i < characterList.Length; i++)
                {
                    if(characterList[i].characterName == "Grey")
                    {
                        newDialogueLines[currentLine].rightSelectedCharacter = characterList[i];

                        if(newDialogueLines[currentLine].leftTurn == false)
                        {
                            nameText.text = newDialogueLines[currentLine].rightSelectedCharacter.characterName;
                        }
                    }
                }
                break;
            default :
                Debug.LogError("Character Does Not Exist!");
                break;
        }
    }

    public void CheckExpressions()
    {
        //LEFT CHARACTER EXPRESSIONS;
        switch (newDialogueLines[currentLine].currentExpression)
        {
            case PortraitExpressions.neutral :
                portrait.sprite = newDialogueLines[currentLine].selectedCharacter.neutralPortrait; 
                break;
            case PortraitExpressions.smug :
                portrait.sprite = newDialogueLines[currentLine].selectedCharacter.smugPortrait; 
                break;
            case PortraitExpressions.surprised :
                portrait.sprite = newDialogueLines[currentLine].selectedCharacter.surprisedPortrait; 
                break;
            case PortraitExpressions.happy :
                portrait.sprite = newDialogueLines[currentLine].selectedCharacter.happyPortrait; 
                break;
            case PortraitExpressions.blushing :
                portrait.sprite = newDialogueLines[currentLine].selectedCharacter.blushingPortrait; 
                break;
            case PortraitExpressions.impressed :
                portrait.sprite = newDialogueLines[currentLine].selectedCharacter.impressedPortrait; 
                break;
            case PortraitExpressions.annoyed :
                portrait.sprite = newDialogueLines[currentLine].selectedCharacter.annoyedPortrait; 
                break;
            case PortraitExpressions.angry :
                portrait.sprite = newDialogueLines[currentLine].selectedCharacter.angryPortrait; 
                break;
            case PortraitExpressions.sad :
                portrait.sprite = newDialogueLines[currentLine].selectedCharacter.sadPortrait; 
                break;
            case PortraitExpressions.crying :
                portrait.sprite = newDialogueLines[currentLine].selectedCharacter.cryingPortrait; 
                break;
            case PortraitExpressions.fear :
                portrait.sprite = newDialogueLines[currentLine].selectedCharacter.fearPortrait; 
                break;
            default :
                Debug.LogError("Expression does not exist!");
                break;
        }

        //RIGHT CHARACTER EXPRESSION
        switch (newDialogueLines[currentLine].rightCurrentExpression)
        {
            case PortraitExpressions.neutral :
                rightPortrait.sprite = newDialogueLines[currentLine].rightSelectedCharacter.neutralPortrait; 
                break;
            case PortraitExpressions.smug :
                rightPortrait.sprite = newDialogueLines[currentLine].rightSelectedCharacter.smugPortrait; 
                break;
            case PortraitExpressions.surprised :
                rightPortrait.sprite = newDialogueLines[currentLine].rightSelectedCharacter.surprisedPortrait; 
                break;
            case PortraitExpressions.happy :
                rightPortrait.sprite = newDialogueLines[currentLine].rightSelectedCharacter.happyPortrait; 
                break;
            case PortraitExpressions.blushing :
                rightPortrait.sprite = newDialogueLines[currentLine].rightSelectedCharacter.blushingPortrait; 
                break;
            case PortraitExpressions.impressed :
                rightPortrait.sprite = newDialogueLines[currentLine].rightSelectedCharacter.impressedPortrait; 
                break;
            case PortraitExpressions.annoyed :
                rightPortrait.sprite = newDialogueLines[currentLine].rightSelectedCharacter.annoyedPortrait; 
                break;
            case PortraitExpressions.angry :
                rightPortrait.sprite = newDialogueLines[currentLine].rightSelectedCharacter.angryPortrait; 
                break;
            case PortraitExpressions.sad :
                rightPortrait.sprite = newDialogueLines[currentLine].rightSelectedCharacter.sadPortrait; 
                break;
            case PortraitExpressions.crying :
                rightPortrait.sprite = newDialogueLines[currentLine].rightSelectedCharacter.cryingPortrait; 
                break;
            case PortraitExpressions.fear :
                rightPortrait.sprite = newDialogueLines[currentLine].rightSelectedCharacter.fearPortrait; 
                break;
            default :
                Debug.LogError("Expression does not exist!");
                break;
        }
    }
}
