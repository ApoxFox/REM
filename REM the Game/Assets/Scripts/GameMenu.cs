using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    public static GameMenu instance;

    public GameObject theMenu;
    public GameObject[] windows;

    private CharStats[] playerStats;

    public TMP_Text[] nameText, hpText, mpText, lvlText, expText;
    public Slider[] expSlider;
    public Image[] characterPortrait;
    public GameObject[] charStatHolder;

    public GameObject[] statusButtons;

    public TMP_Text statusName, statusHP, statusMP, statusStrength, statusDefense, statusWpnEqpd, statusWpnPower, statusArmorEqpd, statusArmorPower, statusEXP;
    public Image statusImage;

    [Header("Inventory/Items Menu")]
    public ItemButton[] itemButtons;
    public string selectedItem;
    public Item activeItem;
    public TMP_Text itemName, itemDescription, useButtonText;

    public GameObject itemCharacterChoiceMenu;
    public TMP_Text[] itemCharacterChoiceNames;

    public TMP_Text goldText;

    public string mainMenuName;


    
    void Start()
    {
        instance = this;
    }

    
    void Update()
    {
        //this opens and closes the menu. THE BUTTON WILL CHANGE
        if(Input.GetButtonDown("Fire2") && PlayerController.instance.canMove)
        {
            if(theMenu.activeInHierarchy)
            {
                //theMenu.SetActive(false);
                //GameManager.instance.gameMenuOpen = false;

                CloseMenu();
            }
            else
            {
                theMenu.SetActive(true);
                UpdateMainStats();
                GameManager.instance.gameMenuOpen = true;
            }

            AudioManager.instance.PlaySFX(5);
        }
    }

    public void UpdateMainStats()
    {
        playerStats = GameManager.instance.playerStats;

        //just a reminder that [i] in the for loop is the current number in the array.
        for(int i = 0; i < playerStats.Length; i++)
        {
            //this is for updating the stats of current party members aswell as seeing which party members are active. It's basically deciding which characters will be shown in the menu and which will not.
            if(playerStats[i].gameObject.activeInHierarchy)
            {
                charStatHolder[i].SetActive(true);

                nameText[i].text = playerStats[i].characterName;
                hpText[i].text = "HP: " + playerStats[i].currentHP + "/" + playerStats[i].maxHP;
                mpText[i].text = "MP: " + playerStats[i].currentMP + "/" + playerStats[i].maxMP;
                lvlText[i].text = "Lvl: " + playerStats[i].playerLevel;
                expText[i].text = "" + playerStats[i].currentEXP + "/" + playerStats[i].expToNextLevel[playerStats[i].playerLevel];
                expSlider[i].maxValue = playerStats[i].expToNextLevel[playerStats[i].playerLevel];
                expSlider[i].value = playerStats[i].currentEXP;
                characterPortrait[i].sprite = playerStats[i].characterPortrait;
            }
            else
            {
                charStatHolder[i].SetActive(false);
            }
        }

        goldText.text = GameManager.instance.currentGold.ToString() + "g";
    }

    //this is the part of the script for window toggling buttons. When one window is opened, all the rest are closed.
    public void ToggleWindow(int windowNumber)
    {
        UpdateMainStats();

        for(int i = 0; i < windows.Length; i++)
        {
            if(i == windowNumber)
            {
                windows[i].SetActive(!windows[i].activeInHierarchy);
            }
            else
            {
                windows[i].SetActive(false);
            }
        }

        itemCharacterChoiceMenu.SetActive(false);
    }

    public void CloseMenu()
    {
        for(int i = 0; i < windows.Length; i++)
        {
            windows[i].SetActive(false);
        }

        theMenu.SetActive(false);

        GameManager.instance.gameMenuOpen = false;

        itemCharacterChoiceMenu.SetActive(false);
    }

    public void OpenStatus()
    {
        UpdateMainStats();
        //update stats and look at character in position 1 first
        StatusCharacter(0);

        //updates the information that is shown

        for(int i = 0; i < statusButtons.Length; i++)
        {
            //if the player stats is active in the hierarchy, then the status buttons should too. It also sets the current party member buttons based on the playerstats array. Thats the point i think.
            statusButtons[i].SetActive(playerStats[i].gameObject.activeInHierarchy);
            statusButtons[i].GetComponentInChildren<TMP_Text>().text = playerStats[i].characterName;
        }
    }

    //this is for selecting different characters in the stats window
    public void StatusCharacter(int selected)
    {
        statusName.text = playerStats[selected].characterName;
        statusHP.text = "" + playerStats[selected].currentHP + "/" + playerStats[selected].maxHP;
        statusMP.text = "" + playerStats[selected].currentMP + "/" + playerStats[selected].maxMP;
        statusStrength.text = playerStats[selected].strength.ToString();
        statusDefense.text = playerStats[selected].defense.ToString();
        //if there is no equipped weapon
        if(playerStats[selected].equippedWpn != "")
        {
            statusWpnEqpd.text = playerStats[selected].equippedWpn;
        }
        else
        {
            //I added both these else statements to the equipped weapon and armor sections because the armor was being set to both characters in the status window. This should work to fix that.
            statusWpnEqpd.text = "None";
        }
        statusWpnPower.text = playerStats[selected].wpnPwr.ToString();

        if(playerStats[selected].equippedArmor != "")
        {
            statusArmorEqpd.text = playerStats[selected].equippedArmor;
        }
        else
        {
            statusArmorEqpd.text = "None";
        }
        statusArmorPower.text = playerStats[selected].armorPwr.ToString();
        //subtracting current EXP from EXP to next level
        statusEXP.text = (playerStats[selected].expToNextLevel[playerStats[selected].playerLevel] - playerStats[selected].currentEXP).ToString();
        statusImage.sprite = playerStats[selected].characterPortrait;

    }

    public void NumberItemButtons() //I actually don't think this is being used anywhere. I've been numbering the buttons mannually. Oh well.
    {
        for(int i = 0; i < itemButtons.Length; i++)
        {
            //supposed to assign a number to all item buttons 0-39
            itemButtons[i].buttonValue = i;
        }
    }

    public void ShowItems()
    {
        GameManager.instance.SortItems();

        for(int i = 0; i < itemButtons.Length; i++)
        {
            //theres a problem where the section above is not running on time before the rest, i believe thats the problem
            //there has to be some issue with the code below because the above code only works when i comment the below out. "Index was out of range", meaning its counting past the point of the arrays end.
            //I moved the numbering code into a function above to keep it seperate for now. Still only works alone but I'll figure it out.
            //turns out I was spawning multiple game managers in the scene. Gotta fix that but it should work now.
            //now there's an issue with line 177 because it can't find the sprite image.

            if(GameManager.instance.itemsHeld[i] != "")
            {
                itemButtons[i].buttonImage.gameObject.SetActive(true);
                
                itemButtons[i].amountText.text = GameManager.instance.numberOfItems[i].ToString();
                
                //going in and calling the item function in game manager to find the item, then going to the item script to find the sprite image.
                //so it's getting to this line below and just stopping completely. Something is wrong with this line and it can't find the itemSprite. I have a feeling it's an issue with GetItemDetails().
                //Well I am a major idiot. in the for loop i put i > 0 instead of i < 0. Whoops. IT WORKS GOD DAMN IT!
                itemButtons[i].buttonImage.sprite = GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[i]).itemSprite;
            }
            else
            {
                itemButtons[i].buttonImage.gameObject.SetActive(false);
                itemButtons[i].amountText.text = "";
            }
        }
    }

    public void SelectItem(Item newItem)
    {
        activeItem = newItem;

        if(activeItem.isItem)
        {
            useButtonText.text = "USE";
        }

        if(activeItem.isWeapon || activeItem.isArmor)
        {
            useButtonText.text = "EQUIP"; 
        }

        itemName.text = activeItem.itemName;
        itemDescription.text = activeItem.description;
    }

    public void DiscardItem()
    {
        if(activeItem != null)
        {
            GameManager.instance.RemoveItem(activeItem.itemName);
        }
    }

    //this is when you click to use an item to choose who will recieve it
    public void OpenItemCharacterChoice()
    {
        itemCharacterChoiceMenu.SetActive(true);

        for(int i = 0; i < itemCharacterChoiceNames.Length; i++)
        {
            //sets the character names to the buttons in the item character choice menu
            itemCharacterChoiceNames[i].text = GameManager.instance.playerStats[i].characterName;
            itemCharacterChoiceNames[i].transform.parent.gameObject.SetActive(GameManager.instance.playerStats[i].gameObject.activeInHierarchy);
        }
    }

    public void CloseItemCharacterChoice()
    {
        itemCharacterChoiceMenu.SetActive(false);
    }

    public void UseItem(int selectChar)
    {
        if(activeItem != null)
        {
            activeItem.Use(selectChar);

            CloseItemCharacterChoice();
        }
    }

    public void SaveGame()
    {
        GameManager.instance.SaveData();
        QuestManager.instance.SaveQuestData();
    }

    public void PlayButtonSound()
    {
        AudioManager.instance.PlaySFX(4);
    }

    public void QuitGame()
    {
        SceneManager.LoadScene(mainMenuName);

        Destroy(GameManager.instance.gameObject);
        Destroy(PlayerController.instance.gameObject);
        Destroy(AudioManager.instance.gameObject);
        Destroy(gameObject);
    }
}
