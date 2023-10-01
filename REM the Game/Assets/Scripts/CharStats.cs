using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharStats : MonoBehaviour
{
    public string characterName;
    public int playerLevel = 1;
    public int currentEXP;
    public int[] expToNextLevel;
    public int maxLevel = 100;
    public int baseEXP = 1000;


    public int currentHP;
    public int maxHP = 100;
    public int currentMP;
    public int maxMP = 30;
    public int[] mpLvlBonus;
    public int strength;
    public int defense;
    public int wpnPwr;
    public int armorPwr;
    public string equippedWpn;
    public string equippedArmor;
    public Sprite characterPortrait;
    public Sprite spriteHead;

    [Header("Portrait Expressions")]
    public Sprite neutralPortrait;
    public Sprite smugPortrait;
    public Sprite surprisedPortrait;
    public Sprite happyPortrait;
    public Sprite blushingPortrait;
    public Sprite impressedPortrait;
    public Sprite annoyedPortrait;
    public Sprite angryPortrait;
    public Sprite sadPortrait;
    public Sprite cryingPortrait;
    public Sprite fearPortrait;

    
    void Start()
    {
        //Below including the for loop is the setup for tracking how much experience you need to level up for every level. This math creates a nice exp curve.
        expToNextLevel = new int[maxLevel];
        expToNextLevel[1] = baseEXP;

        //you can multiply ints with floats using Mathf.FloorToInt(), which basically just rounds the number to a whole number.
        for(int i = 2; i < expToNextLevel.Length; i++)
        {
            expToNextLevel[i] = Mathf.FloorToInt(expToNextLevel[i - 1] * 1.05f);
        }
    }

    
    void Update()
    {
        //for testing exp system
        if(Input.GetKeyDown(KeyCode.K))
        {
            AddEXP(1000);
        }
    }


    //expToAdd is applied by whatever is giving you exp. I'm assuming this script will be used by battle encounters and quests. Maybe for the friendship levels we will do something similar.
    public void AddEXP(int expToAdd)
    {
        currentEXP += expToAdd;

        if(playerLevel < maxLevel)
        {
            if(currentEXP > expToNextLevel[playerLevel])
            {
                currentEXP -= expToNextLevel[playerLevel];

                playerLevel++;

                //determine whether to add to strength of defense based on odd or even level - this may change depending on my own game
                //percent % is called modulo, if you divide a number by another number, it'll be whatever remaineder is left. This is used to find whether or not it's odd or even.
                if(playerLevel%2 == 0)
                {
                    strength++;
                }
                else
                {
                    defense++;
                }

                //HP is raised during level up, and current HP is raised back to max
                maxHP = Mathf.FloorToInt(maxHP * 1.05f);
                currentHP = maxHP;

                //MP is raised manually through what we input in the mpLvlBonus inspector array, this is more work because it's manual scaling but it makes sense instead of being multiplied.
                maxMP += mpLvlBonus[playerLevel];
                currentMP = maxMP;
            }
        }
        
        if(playerLevel >= maxLevel)
        {
            currentEXP = 0;
        }
    }
}
