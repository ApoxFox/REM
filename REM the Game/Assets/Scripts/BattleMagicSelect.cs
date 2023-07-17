using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleMagicSelect : MonoBehaviour
{
    public string spellName;
    public int spellCost;
    public TMP_Text nameText;
    public TMP_Text costText;
    
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    public void Press()
    {
        if(BattleManager.instance.activeBattlers[BattleManager.instance.currentTurn].currentMP >= spellCost)
        {
            BattleManager.instance.magicMenu.SetActive(false);
            BattleManager.instance.OpenTargetMenu(spellName);
            BattleManager.instance.activeBattlers[BattleManager.instance.currentTurn].currentMP -= spellCost;
        }
        else
        {
            //let player know there is not enough MP (this could be either changing the alpha of the text or something like that)
            BattleManager.instance.battleNotice.theText.text = "NOT ENOUGH MP!";
            BattleManager.instance.battleNotice.Activate();
            BattleManager.instance.magicMenu.SetActive(false);
        }
    }
}
