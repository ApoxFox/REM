using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    //can't be used as a component on an object, it is for storing data on a specifc thing. In this case, attacks or moves.
    [System.Serializable]
public class BattleMove
{
    public string moveName;
    public bool isAttack, isBuff, isDebuff, isSupport;
    public string elementalType;
    public int movePower;
    public int moveCost;
    public AttackEffect theEffect;
    
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
}
