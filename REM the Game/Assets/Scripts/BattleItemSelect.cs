using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class BattleItemSelect : MonoBehaviour
{
    public string itemName;
    public Image buttonImage;
    public TMP_Text amountText;
    public int buttonValue;


    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    public void Press()
    {     
        if (BattleManager.instance.itemMenu.activeInHierarchy)
        {
            if (GameManager.instance.itemsHeld[buttonValue] != "")
            {              
                BattleManager.instance.SelectItem(GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[buttonValue]));
            }
        } 
    }
}
