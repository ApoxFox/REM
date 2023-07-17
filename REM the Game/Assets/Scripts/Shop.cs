using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Shop : MonoBehaviour
{
    public static Shop instance;

    public GameObject shopMenu;
    public GameObject buyMenu;
    public GameObject sellMenu;

    public TMP_Text goldText;

    public string[] itemsForSale;

    public ItemButton[] buyItemButtons;
    public ItemButton[] sellItemButtons;

    public Item selectedItem;
    public TMP_Text buyItemName, buyItemDescription, buyItemValue;
    public TMP_Text sellItemName, sellItemDescription, sellItemValue;
    
    void Start()
    {
        instance = this;
    }

    
    void Update()
    {
        
    }

    public void OpenShop()
    {
        shopMenu.SetActive(true);
        OpenBuyMenu();

        GameManager.instance.shopActive = true;

        goldText.text = GameManager.instance.currentGold.ToString() + "g";
    }

    public void CloseShop()
    {
        shopMenu.SetActive(false);

        GameManager.instance.shopActive = false;
    }

    public void OpenBuyMenu()
    {
        buyItemButtons[0].Press();

        buyMenu.SetActive(true);
        sellMenu.SetActive(false);

        for(int i = 0; i < buyItemButtons.Length; i++)
        {

            if(itemsForSale[i] != "")
            {
                buyItemButtons[i].buttonImage.gameObject.SetActive(true);
                
                buyItemButtons[i].amountText.text = "";
                
                buyItemButtons[i].buttonImage.sprite = GameManager.instance.GetItemDetails(itemsForSale[i]).itemSprite;
            }
            else
            {
                buyItemButtons[i].buttonImage.gameObject.SetActive(false);
                buyItemButtons[i].amountText.text = "";
            }
        }
    }

//this menu just accesses your menu inventory directly the same way we do it for the menu itself.
    public void OpenSellMenu()
    {
        sellItemButtons[0].Press();

        sellMenu.SetActive(true);
        buyMenu.SetActive(false);

        ShowSellItems();
    }

    //the 2 functions below are similar to the SelectItem functions in the GameMenu script. It's for when you click on an item in shop to show it's details and give you the option to buy or sell.
    public void SelectBuyItem(Item buyItem)
    {
        selectedItem = buyItem;

        buyItemName.text = selectedItem.itemName;
        buyItemDescription.text = selectedItem.description;
        buyItemValue.text = "PRICE: " + selectedItem.value + "g";
    }

    public void SelectSellItem(Item sellItem)
    {
        selectedItem = sellItem;

        sellItemName.text = selectedItem.itemName;
        sellItemDescription.text = selectedItem.description;
        sellItemValue.text = "PRICE: " + Mathf.FloorToInt(selectedItem.value * 0.5f).ToString() + "g";
    }

    public void BuyItem()
    {
        if(selectedItem != null)
        {
            if(GameManager.instance.currentGold >= selectedItem.value)
            {
                GameManager.instance.currentGold -= selectedItem.value;

                GameManager.instance.AddItem(selectedItem.itemName);
            }
        }

        goldText.text = GameManager.instance.currentGold.ToString() + "g";
    }

    public void SellItem()
    {
        if(selectedItem != null)
        {
            GameManager.instance.currentGold += Mathf.FloorToInt(selectedItem.value * 0.5f);

            GameManager.instance.RemoveItem(selectedItem.itemName);
        }

        goldText.text = GameManager.instance.currentGold.ToString() + "g";

        ShowSellItems();
    }

    //updates sell menu upon opening and selling each item
    private void ShowSellItems()
    {
        GameManager.instance.SortItems();

        for(int i = 0; i < sellItemButtons.Length; i++)
        {
            if(GameManager.instance.itemsHeld[i] != "")
            {
                sellItemButtons[i].buttonImage.gameObject.SetActive(true);
                
                sellItemButtons[i].amountText.text = GameManager.instance.numberOfItems[i].ToString();
                
                sellItemButtons[i].buttonImage.sprite = GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[i]).itemSprite;
            }
            else
            {
                sellItemButtons[i].buttonImage.gameObject.SetActive(false);
                sellItemButtons[i].amountText.text = "";
            }
        }
    }

}
