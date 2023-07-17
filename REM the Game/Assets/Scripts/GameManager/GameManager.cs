using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public CharStats[] playerStats;
    public bool gameMenuOpen, dialogueActive, fadingBetweenAreas, shopActive, battleActive;

    [Header("Inventory")]
    public string[] itemsHeld;
    public int[] numberOfItems;
    public Item[] referenceItems;

    public int currentGold;
    

    
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            if(instance != this)
            {
                Destroy(gameObject);
            }
        }

        DontDestroyOnLoad(gameObject);

        SortItems();
    }

    
    void Update()
    {
        if(gameMenuOpen || dialogueActive || fadingBetweenAreas || shopActive || battleActive)
        {
            PlayerController.instance.canMove = false;
        }
        else
        {
            PlayerController.instance.canMove = true;
        }

        if(Input.GetKeyDown(KeyCode.O))
        {
            SaveData();
        }
        if(Input.GetKeyDown(KeyCode.P))
        {
            LoadData();
        }
    }

    //I'll tell you what, this function is a pain in the ass because there's always issues with it. What's worse is that it's key to the shop menu and inventory game menu so its used alot.
    public Item GetItemDetails(string itemToGrab)
    {

        for(int i = 0; i < referenceItems.Length; i++)
        {
            if(referenceItems[i].itemName == itemToGrab)
            {
                return referenceItems[i];
            }
        }

        return null;
        //if we made it to the end of this function without finding an item, return info that it is empty.
    }

    //when there are gaps in our inventory, the items will be sorted to the front of the item buttons array
    public void SortItems()
    {
        bool itemAfterSpace = true;

        //while basically means: While a bool = true, keep doing the loop. This can create endless loops very easily so BE CAREFUL
        while(itemAfterSpace)
        {
            itemAfterSpace = false;

            //Length - 1 because we don't need to shift anything into the last space because there is no 40 space
            for(int i = 0; i < itemsHeld.Length - 1; i++)
            {
                //basically this loop goes through all of the item buttons, checks if they are empty and if they are moves the next item back a space.
                if(itemsHeld[i] == "")
                {
                    itemsHeld[i] = itemsHeld[i + 1];
                    itemsHeld[i + 1] = "";

                    numberOfItems[i] = numberOfItems[i + 1];
                    numberOfItems[i + 1] = 0; 

                    //this will only be done if there's still space left to sort. If not, the loop finally ends with itemAfterSpace = false.
                    if(itemsHeld[i] != "")
                    {
                        itemAfterSpace = true;
                    }
                }
            }
        }
    }

    //checks if one of that specific item is already in inventory, if so add +1. If not, move to the next open slot and add the item and value.
    public void AddItem(string itemToAdd)
    {
        int newItemPosition = 0;
        bool foundSpace = false;

        for(int i = 0; i < itemsHeld.Length; i++)
        {
            if(itemsHeld[i] == "" || itemsHeld[i] == itemToAdd)
            {
                newItemPosition = i;
                i = itemsHeld.Length;
                foundSpace = true;
            }
        }

        if(foundSpace)
        {
            bool itemExists = false;

            for(int i = 0; i < referenceItems.Length; i++)
            {
                if(referenceItems[i].itemName == itemToAdd)
                {
                    itemExists = true;
                    //stops searching array, stops endless loop
                    i = referenceItems.Length;
                }
            }

            if(itemExists)
            {
                itemsHeld[newItemPosition] = itemToAdd;
                numberOfItems[newItemPosition]++;
            }
            else
            {
                //this checks error will show if there is an error with the item not existing. Probably an issue with reference items not being entered properly.
                Debug.LogError(itemToAdd + " Does Not Exist!!");
            }
        }
        //makes the added items appear in the inventory window
        GameMenu.instance.ShowItems();
    }

    public void RemoveItem(string itemToRemove)
    {
        bool foundItem = false;
        int itemPosition = 0;

        for(int i = 0; i < itemsHeld.Length; i++)
        {
            if(itemsHeld[i] == itemToRemove)
            {
                foundItem = true;
                itemPosition = i;

                i = itemsHeld.Length;
            }
        }

        if(foundItem)
        {
            numberOfItems[itemPosition]--;

            if(numberOfItems[itemPosition] <= 0)
            {
                itemsHeld[itemPosition] = "";
            }

            GameMenu.instance.ShowItems();
        }
        else
        {
            Debug.LogError("Couldn't find " + itemToRemove);
        }
    }
    

    //!!!!!! So there seems to be an issue with saving and loading Party Member stats and equipped items. It should be an easy fix but for now I'll leave this note !!!!!
    //there's also an issue with saving and loading different scenes. Probably has to do with LoadData()
    //theres also a problem where when the scene loads, the party members change.
    public void SaveData()
    {
        //Save current scene and position
        PlayerPrefs.SetString("Current_Scene", SceneManager.GetActiveScene().name);
        PlayerPrefs.SetFloat("Player_Position_x", PlayerController.instance.transform.position.x);
        PlayerPrefs.SetFloat("Player_Position_y", PlayerController.instance.transform.position.y);
        PlayerPrefs.SetFloat("Player_Position_z", PlayerController.instance.transform.position.z);

        //Save Character Info
        for(int i = 0; i < playerStats.Length; i++)
        {
            if(playerStats[i].gameObject.activeInHierarchy)
            {
                PlayerPrefs.SetInt("Player_" + playerStats[i].characterName + "_active", 1);
            }
            else
            {
                PlayerPrefs.SetInt("Player_" + playerStats[i].characterName + "_active", 0);
            }
            //if we need to add more stats to the player, they will need to be added to the save section too.
            PlayerPrefs.SetInt("Player_" + playerStats[i].characterName + "_Level", playerStats[i].playerLevel);
            PlayerPrefs.SetInt("Player_" + playerStats[i].characterName + "_CurrentExp", playerStats[i].currentEXP);
            PlayerPrefs.SetInt("Player_" + playerStats[i].characterName + "_CurrentHP", playerStats[i].currentHP);
            PlayerPrefs.SetInt("Player_" + playerStats[i].characterName + "_maxHP", playerStats[i].maxHP);
            PlayerPrefs.SetInt("Player_" + playerStats[i].characterName + "_CurrentMP", playerStats[i].currentMP);
            PlayerPrefs.SetInt("Player_" + playerStats[i].characterName + "_MaxMP", playerStats[i].maxMP);
            PlayerPrefs.SetInt("Player_" + playerStats[i].characterName + "_Strength", playerStats[i].strength);
            PlayerPrefs.SetInt("Player_" + playerStats[i].characterName + "_Defense", playerStats[i].defense);
            PlayerPrefs.SetInt("Player_" + playerStats[i].characterName + "_WeaponPower", playerStats[i].wpnPwr);
            PlayerPrefs.SetInt("Player_" + playerStats[i].characterName + "_ArmorPower", playerStats[i].armorPwr);
            PlayerPrefs.SetString("Player_" + playerStats[i].characterName + "_EquippedWeapon", playerStats[i].equippedWpn);
            PlayerPrefs.SetString("Player_" + playerStats[i].characterName + "_EquippedArmor", playerStats[i].equippedArmor);
        }

        //Saves Inventory Data
        for(int i = 0; i < itemsHeld.Length; i++)
        {
            PlayerPrefs.SetString("ItemInInventory_" + i, itemsHeld[i]);
            PlayerPrefs.SetInt("ItemAmount_" + i, numberOfItems[i]);
        }
    
    }

    public void LoadData()
    {
        //Load Player Location
        PlayerController.instance.transform.position = new Vector3(PlayerPrefs.GetFloat("Player_Position_x"), PlayerPrefs.GetFloat("Player_Position_y"), PlayerPrefs.GetFloat("Player_Position_z"));

        //Load player stats
        for(int i = 0; i < playerStats.Length; i++)
        {
            if(PlayerPrefs.GetInt("Player_" + playerStats[i].characterName + "_active") == 0)
            {
                playerStats[i].gameObject.SetActive(false);
            }
            else
            {
                playerStats[i].gameObject.SetActive(true);
            }

            playerStats[i].playerLevel = PlayerPrefs.GetInt("Player_" + playerStats[i].characterName + "_Level");
            playerStats[i].currentEXP = PlayerPrefs.GetInt("Player_" + playerStats[i].characterName + "_CurrentExp");
            playerStats[i].currentHP = PlayerPrefs.GetInt("Player_" + playerStats[i].characterName + "_CurrentHP");
            playerStats[i].maxHP = PlayerPrefs.GetInt("Player_" + playerStats[i].characterName + "_maxHP");
            playerStats[i].currentMP = PlayerPrefs.GetInt("Player_" + playerStats[i].characterName + "_CurrentMP");
            playerStats[i].maxMP = PlayerPrefs.GetInt("Player_" + playerStats[i].characterName + "_MaxMP");
            playerStats[i].strength = PlayerPrefs.GetInt("Player_" + playerStats[i].characterName + "_Strength");
            playerStats[i].defense = PlayerPrefs.GetInt("Player_" + playerStats[i].characterName + "_Defense");
            playerStats[i].wpnPwr = PlayerPrefs.GetInt("Player_" + playerStats[i].characterName + "_WeaponPower");
            playerStats[i].armorPwr = PlayerPrefs.GetInt("Player_" + playerStats[i].characterName + "_ArmorPower");
            playerStats[i].equippedWpn = PlayerPrefs.GetString("Player_" + playerStats[i].characterName + "_EquippedWeapon");
            playerStats[i].equippedArmor = PlayerPrefs.GetString("Player_" + playerStats[i].characterName + "_EquippedArmor");
        }
        //Load Inventory
        for(int i = 0; i < itemsHeld.Length; i++)
        {
            itemsHeld[i] = PlayerPrefs.GetString("ItemInInventory_" + i);
            numberOfItems[i] = PlayerPrefs.GetInt("ItemAmount_" + i);
        }
    }
}
