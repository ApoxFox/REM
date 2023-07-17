using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestObjectActivator : MonoBehaviour
{
    public GameObject objectToActivate;
    public string questToCheck;
    public bool activeIfComplete;

    private bool initialCheckDone;
    
    void Start()
    {
        
    }

    
    void Update()
    {
        //this will happen after the start happens, but on the very first frame that way nothing gets instantiated out of order
        if(!initialCheckDone)
        {
            initialCheckDone = true;

            CheckCompletion();
        }
    }

    //sets object active or inactive depending on quest completion
    public void CheckCompletion()
    {
        if(QuestManager.instance.CheckIfComplete(questToCheck))
        {
            objectToActivate.SetActive(activeIfComplete);
        }
    }
}
