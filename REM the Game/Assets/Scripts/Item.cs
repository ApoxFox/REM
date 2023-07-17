using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("Item Type")]
    public bool isItem;
    public bool isWeapon;
    public bool isArmor;

    [Header("Item Info")]
    public string itemName;
    public string description;
    public int value;
    public Sprite itemSprite;

    [Header("Item Affects")]
    //this is for determining added health, mp, armor, strength ect.
    public int amountToChange;
    public bool affectHP, affectMP, affectStrength;

    [Header("Weapon/Armor Details")]
    public int weaponStrength;
    public int armorStrength;
    
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    //this is for hitting the use button in the inventory, it picks the character to use it on and changes the stats accordingly based on this script.
    public void Use(int charToUseOn)
    {
        CharStats selectedChar = GameManager.instance.playerStats[charToUseOn];

        if(isItem)
        {
            if(affectHP)
            {
                selectedChar.currentHP += amountToChange;

                if(selectedChar.currentHP > selectedChar.maxHP)
                {
                    selectedChar.currentHP = selectedChar.maxHP;
                }
            }

            if(affectMP)
            {
                selectedChar.currentMP += amountToChange;

                if(selectedChar.currentMP > selectedChar.maxMP)
                {
                    selectedChar.currentMP = selectedChar.maxMP;
                }
            }

            if(affectStrength)
            {
                selectedChar.strength += amountToChange;
            }
        }

        if(isWeapon)
        {
            if(selectedChar.equippedWpn != "")
            {
                GameManager.instance.AddItem(selectedChar.equippedWpn);
            }

            selectedChar.equippedWpn = itemName;
            selectedChar.wpnPwr = weaponStrength;
        }

        if(isArmor)
        {
            if(selectedChar.equippedArmor != "")
            {
                GameManager.instance.AddItem(selectedChar.equippedArmor);
            }

            selectedChar.equippedArmor = itemName;
            selectedChar.armorPwr = armorStrength;
        }

        GameManager.instance.RemoveItem(itemName);
    }
}
