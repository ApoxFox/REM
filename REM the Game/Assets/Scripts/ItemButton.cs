using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemButton : MonoBehaviour
{
    public Image buttonImage;
    public TMP_Text amountText;
    public int buttonValue;


    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    //This script is for the item slots in your inventory aswell as shop menus etc.

    public void Press()
    {
        //this first section is for finding item info in the game menu inventory
        if(GameMenu.instance.theMenu.activeInHierarchy)
        {
            if(GameManager.instance.itemsHeld[buttonValue] != "")
            {
                GameMenu.instance.SelectItem(GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[buttonValue]));
            }
        }

        //this part is for the shop menu inventory item info.
        //currently it is struggling to use the GetItemDetails() from the game manager and outputting every item as Health Potion.
        //So here's some progress. I figured out that it is only finding the reference for whatever is in the first slot. If there's nothing there, I get an error. This means it is only searching the first slot for some reason.
        //Fucking A. So the button value's are never being set to the specific buttons in the ItemButton script. AKA every button is 0. So that's gotta change.
        if(Shop.instance.shopMenu.activeInHierarchy)
        {
            if(Shop.instance.buyMenu.activeInHierarchy)
            {
                if(Shop.instance.itemsForSale[buttonValue] != "")
                {
                    Shop.instance.SelectBuyItem(GameManager.instance.GetItemDetails(Shop.instance.itemsForSale[buttonValue]));
                }
            }

                
            if(Shop.instance.sellMenu.activeInHierarchy)
            {
                Shop.instance.SelectSellItem(GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[buttonValue]));
            }
        }
    }
}
