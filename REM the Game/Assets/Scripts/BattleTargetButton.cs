using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleTargetButton : MonoBehaviour
{
    public string moveName;
    public int activeBattlerTarget;
    public TMP_Text targetName;
    
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    public void Press()
    {
        BattleManager.instance.PlayerAttack(moveName, activeBattlerTarget);
    }
}
