using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueActivator : MonoBehaviour
{
    public DialogueLines[] lines;

    private bool canActivate;
    public GameObject emote;
    public Animator emoteAnim;
    public SpriteRenderer emoteSprite;
    public bool hasBeenChecked;

    public bool isPerson = true;

    public bool shouldActivateQuest;
    public string questToMark;
    public bool markComplete;
    
    void Start()
    {
        
    }

    
    void Update()
    {
        if(canActivate && Input.GetButtonDown("Fire1") && !DialogueManager.instance.dialogueBox.activeInHierarchy && GameMenu.instance.theMenu.activeInHierarchy == false)
        {
            emoteAnim.SetBool("hasChecked", true);
            hasBeenChecked = true;

            
            DialogueManager.instance.ShowDialogue(lines, isPerson);
            DialogueManager.instance.ShouldActivateQuestAtEnd(questToMark, markComplete);
        }
        
        if(DialogueManager.instance.emoteVisible)
        {
            if(hasBeenChecked == true)
            {
                emoteSprite.color = new Vector4(1, 1, 1, 0.5f);
            }
            else
            {
                emoteSprite.color = new Vector4(1, 1, 1, 1f);
            }
        }
        else
        {
            emoteSprite.color = new Vector4(1, 1, 1, 0f);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.tag == "Player")
        {
            canActivate = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if(other.tag == "Player")
        {
            canActivate = false;
        }
    }
}
