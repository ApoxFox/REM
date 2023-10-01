using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;


[System.Serializable]
public class DialogueLines
{
    public string lineText;
    public bool twoCharacters;
    public bool leftTurn = true;

    [Header("Enums")]
    public CharacterToShow currentCharacter;
    public CharacterToShow rightCharacter;
    public CharStats selectedCharacter;
    public CharStats rightSelectedCharacter;


    public PortraitExpressions currentExpression;
    public PortraitExpressions rightCurrentExpression;

    void Start()
    {
        
    }
    
    void Update()
    {
        
    }
}

public enum CharacterToShow
{
    Junebug,
    Talula,
    Grey
}

public enum PortraitExpressions
{
    neutral,
    smug,
    surprised,
    happy,
    blushing,
    impressed,
    annoyed,
    angry,
    sad,
    crying,
    fear
}
