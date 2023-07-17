using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleReward : MonoBehaviour
{
    public static BattleReward instance;

    public TMP_Text expText, itemText;
    public GameObject rewardScreen;

    public string[] rewardItems;
    public int expEarned;

    public bool markQuestComplete;
    public string questToMark;

    
    void Start()
    {
        instance = this;
    }

    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Y))
        {
            OpenRewardScreen(54, new string[] {"Iron Sword", "Iron Armor"});
        }
    }

    public void OpenRewardScreen(int exp, string[] rewards)
    {
        expEarned = exp;
        rewardItems = rewards;

        expText.text = "EVERYONE EARNED " + expEarned + " EXP!";
        itemText.text = "";

        for(int i = 0; i < rewardItems.Length; i++)
        {
            itemText.text += rewards[i] + "\n";
        }

        rewardScreen.SetActive(true);
    }

    public void CloseRewardScreen()
    {
        for(int i = 0; i < GameManager.instance.playerStats.Length; i++)
        {
            if(GameManager.instance.playerStats[i].gameObject.activeInHierarchy)
            {
                GameManager.instance.playerStats[i].AddEXP(expEarned);
            }
        }

        for(int i = 0; i < rewardItems.Length; i++)
        {
            GameManager.instance.AddItem(rewardItems[i]);
        }

        rewardScreen.SetActive(false);
        GameManager.instance.battleActive = false;

        if(markQuestComplete)
        {
            QuestManager.instance.MarkQuestComplete(questToMark);
        }
    }
}
