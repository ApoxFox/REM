using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;

    private bool battleActive;
    public GameObject battleScene;

    public Transform[] playerPositions;
    public Transform[] enemyPositions;

    public BattleChar[] playerPrefabs;
    public BattleChar[] enemyPrefabs;

    //lists are like arrays but they are much more flexible, especially when it comes to changing factors within it. For example, if a character dies, they can be removed from the list, but also added back if revived.
    public List<BattleChar> activeBattlers = new List<BattleChar>();

    [Header("Turns")]
    public int currentTurn;
    public bool turnWaiting;
    public GameObject uiButtonsHolder;

    public BattleMove[] movesList;
    public GameObject enemyAttackEffect;
    public DamageNumbers damageNumbers;

    [Header("UI Updating Stuff")]
    public TMP_Text[] playerName;
    public TMP_Text[] playerHP;
    public TMP_Text[] playerMP;
    
    public GameObject targetMenu;
    public BattleTargetButton[] targetButtons;

    public GameObject magicMenu;
    public BattleMagicSelect[] magicButtons;

    public BattleNotification battleNotice;

    public int chanceToFlee = 35;

    private bool fleeing;

    [Header("Item Stuff")]
    public GameObject itemMenu;
    public BattleItemSelect[] itemButtons;
    public Item activeItem;
    public TMP_Text itemName, itemDescription, useButtonText;
    public GameObject itemCharChoiceMenu;
    public TMP_Text[] itemCharChoiceNames;
    public CharStats[] playerStats;  //this is to reference GameManager's stats

    public string gameOverScene;
    public int rewardExp;
    public string[] rewardItems;

    public bool cannotFlee;




    
    void Start()
    {
        instance = this;

        DontDestroyOnLoad(gameObject);
    }

    
    void Update()
    {
        //FOR TESTING BATTLES
        if(Input.GetKeyDown(KeyCode.N))
        {
            NextTurn();
        }

        if(battleActive)
        {
            if(turnWaiting)
            {
                if(activeBattlers[currentTurn].isPlayer) //PLAYER TURN
                {
                    uiButtonsHolder.SetActive(true);
                }
                else // ENEMY TURN
                {
                    uiButtonsHolder.SetActive(false);

                    //Enemy should attack
                    StartCoroutine(EnemyMoveCo());
                }
            }
        }
    }

    public void BattleStart(string[] enemiesToSpawn, bool setCannotFlee)
    {
        if(!battleActive)
        {
            cannotFlee = setCannotFlee;

            battleActive = true;

            GameManager.instance.battleActive = true;

            transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, transform.position.z);
            battleScene.SetActive(true);

            AudioManager.instance.PlayBGM(0);

            //this is for setting players and stats for players.
            for(int i = 0; i < playerPositions.Length; i++)
            {
                if(GameManager.instance.playerStats[i].gameObject.activeInHierarchy)
                {
                    //you can't use the same for loop variable name within the same for loop, so make the next one the next letter in the alphabet (i , j , k ect.)
                    for(int j = 0; j < playerPrefabs.Length; j++)
                    {
                        if(playerPrefabs[j].charName == GameManager.instance.playerStats[i].characterName)
                        {
                            //Setting characters
                            BattleChar newPlayer = Instantiate(playerPrefabs[j], playerPositions[i].position, playerPositions[i].rotation);
                            newPlayer.transform.parent = playerPositions[i];
                            activeBattlers.Add(newPlayer);

                            //Setting Stats
                            CharStats thePlayer = GameManager.instance.playerStats[i];

                            activeBattlers[i].currentHP = thePlayer.currentHP;
                            activeBattlers[i].maxHP = thePlayer.maxHP;
                            activeBattlers[i].currentMP = thePlayer.currentMP;
                            activeBattlers[i].MaxMP = thePlayer.maxMP;
                            activeBattlers[i].strength = thePlayer.strength;
                            activeBattlers[i].defence = thePlayer.defense;
                            activeBattlers[i].wpnPower = thePlayer.wpnPwr;
                            activeBattlers[i].armorPower = thePlayer.armorPwr;
                        }
                    }
                }
            }

            for(int i = 0; i < enemiesToSpawn.Length; i++)
            {
                if(enemiesToSpawn[i] != "")
                {
                    for(int j = 0; j < enemyPrefabs.Length; j++)
                    {
                        if(enemyPrefabs[j].charName == enemiesToSpawn[i])
                        {
                            BattleChar newEnemy = Instantiate(enemyPrefabs[j], enemyPositions[i].position, enemyPositions[i].rotation);
                            newEnemy.transform.parent = enemyPositions[i];
                            activeBattlers.Add(newEnemy);
                        }
                    }
                }
            }

            turnWaiting = true;
            //!!!!!!!!!!! This is currently the way that turns are being set. In the future, it might be cool to set them via random number based on dice roll for every character.
            currentTurn = Random.Range(0, activeBattlers.Count);
        }
        UpdateUIStats();
    }

    public void NextTurn()
    {
        currentTurn++;

        if(currentTurn >= activeBattlers.Count)
        {
            currentTurn = 0;
        }

        turnWaiting = true;

        UpdateBattle();
        UpdateUIStats();
    }

    public void UpdateBattle()
    {
        bool allEnemiesDead = true;
        bool allPlayersDead = true;

        for(int i = 0; i < activeBattlers.Count; i ++)
        {
            if(activeBattlers[i].currentHP < 0)
            {
                activeBattlers[i].currentHP = 0;
            }

            if(activeBattlers[i].currentHP == 0)
            {
                //Handle dead battler
                if(activeBattlers[i].isPlayer)
                {
                    activeBattlers[i].spriteRenderer.sprite = activeBattlers[i].deadSprite;
                }
                else
                {
                    activeBattlers[i].EnemyFade();
                }
            }
            else
            {
                if(activeBattlers[i].isPlayer)
                {
                    allPlayersDead = false;
                    activeBattlers[i].spriteRenderer.sprite = activeBattlers[i].aliveSprite;
                }
                else
                {
                    allEnemiesDead = false;

                }
            }
        }

        if(allEnemiesDead || allPlayersDead)
        {
            if(allEnemiesDead)
            {
                //End battle in victory
                StartCoroutine(EndBattleCo());
            }
            else
            {
                //End battle in Defeat
                StartCoroutine(GameOverCo());
            }

            /*battleScene.SetActive(false);
            GameManager.instance.battleActive = false;
            battleActive = false;*/
        }
        else
        {
            //this section is for checking if the current turn player is already dead, and if so it will skip their turn.
            while(activeBattlers[currentTurn].currentHP == 0)
            {
                currentTurn++;
                if(currentTurn >= activeBattlers.Count)
                {
                    currentTurn = 0;
                }
            }
        }
    }

    //Once a coroutine is called, unity keeps running the other functions in the list instead of stopping til it's done.
    public IEnumerator EnemyMoveCo()
    {
        turnWaiting = false;

        yield return new WaitForSeconds(1f);

        EnemyAttack();

        yield return new WaitForSeconds(1f);
        NextTurn();
    }

    public void EnemyAttack()
    {
        //this is for choosing which character to attack
        List<int> players = new List<int>();

        for(int i = 0; i < activeBattlers.Count; i++)
        {
            if(activeBattlers[i].isPlayer && activeBattlers[i].currentHP > 0)
            {
                players.Add(i);
            }
        }
        int selectedTarget = players[Random.Range(0, players.Count)];

        //choose from a certain amount of moves which one to use. Also set the move power.
        
        int selectAttack = Random.Range(0, activeBattlers[currentTurn].movesAvailable.Length);
        int movePower = 0;
        for(int i = 0; i < movesList.Length; i++)
        {
            if(movesList[i].moveName == activeBattlers[currentTurn].movesAvailable[selectAttack])
            {
                Instantiate(movesList[i].theEffect, activeBattlers[selectedTarget].transform.position, activeBattlers[selectedTarget].transform.rotation);
                movePower = (movesList[i].movePower);
            }
        }
        Instantiate(enemyAttackEffect, activeBattlers[currentTurn].transform.position, activeBattlers[currentTurn].transform.rotation);

        DealDamage(selectedTarget, movePower);
    }

    public void DealDamage(int target, int movePower)
    {
        //This is all for calculating the amount of damage to deal based on attack power and weapon power, as well as the targets defence and armor power. This all gets calculated and then a random number is divided or multiplied to that number.
        float attackPower = activeBattlers[currentTurn].strength + activeBattlers[currentTurn].wpnPower;
        float defensePower = activeBattlers[target].defence + activeBattlers[target].armorPower;

        float damageCalc = (attackPower / defensePower) * movePower * Random.Range(.9f, 1.1f);
        int damageToGive = Mathf.RoundToInt(damageCalc);

        Debug.Log(activeBattlers[currentTurn].charName + " is dealing " + damageCalc + "(" + damageToGive + ") damage to " + activeBattlers[target].charName);

        
        if(activeBattlers[target].isPlayer)
        {
            CharStats selectedChar = GameManager.instance.playerStats[target];                 ////////

            selectedChar.currentHP -= damageToGive;

            activeBattlers[target].currentHP -= damageToGive;
        }       

        if(!activeBattlers[target].isPlayer)
        {
            activeBattlers[target].currentHP -= damageToGive;
        }   

        Instantiate(damageNumbers, activeBattlers[target].transform.position, activeBattlers[target].transform.rotation).SetDamage(damageToGive);

        UpdateUIStats();
    }

    public void UpdateUIStats()
    {
        for(int i = 0; i < playerName.Length; i++)
        {
            if(activeBattlers.Count > i)
            {
                if(activeBattlers[i].isPlayer)
                {
                    BattleChar playerData = activeBattlers[i];

                    playerName[i].gameObject.SetActive(true);
                    playerName[i].text = playerData.charName;
                    playerHP[i].text = Mathf.Clamp(playerData.currentHP, 0, int.MaxValue) + "/" + playerData.maxHP;
                    playerMP[i].text = Mathf.Clamp(playerData.currentMP, 0, int.MaxValue) + "/" + playerData.MaxMP;
                }
                else
                {
                    playerName[i].gameObject.SetActive(false);
                }
            }
            else
            {
                playerName[i].gameObject.SetActive(false);
            }
        }
    }

    public void PlayerAttack(string moveName, int selectedTarget)
    {

        int movePower = 0;
        for(int i = 0; i < movesList.Length; i++)
        {
            if(movesList[i].moveName == moveName)
            {
                Instantiate(movesList[i].theEffect, activeBattlers[selectedTarget].transform.position, activeBattlers[selectedTarget].transform.rotation);
                movePower = (movesList[i].movePower);
            }
        }
        Instantiate(enemyAttackEffect, activeBattlers[currentTurn].transform.position, activeBattlers[currentTurn].transform.rotation);

        DealDamage(selectedTarget, movePower);

        uiButtonsHolder.SetActive(false);
        targetMenu.SetActive(false);

        NextTurn();

    }

    public void OpenTargetMenu(string moveName)
    {
        targetMenu.SetActive(true);

        List<int> Enemies = new List<int>();

        for(int i = 0; i < activeBattlers.Count; i++)
        {
            if(!activeBattlers[i].isPlayer)
            {
                Enemies.Add(i);
            }
        }

        for(int i = 0; i < targetButtons.Length; i++)
        {
            if(Enemies.Count > i && activeBattlers[Enemies[i]].currentHP > 0)
            {
                targetButtons[i].gameObject.SetActive(true);

                targetButtons[i].moveName = moveName;
                targetButtons[i].activeBattlerTarget = Enemies[i];
                targetButtons[i].targetName.text = activeBattlers[Enemies[i]].charName;
            }
            else
            {
                targetButtons[i].gameObject.SetActive(false);
            }
        }
    }

    public void OpenMagicMenu()
    {
        magicMenu.SetActive(true);

        for(int i = 0; i < magicButtons.Length; i++)
        {
            if(activeBattlers[currentTurn].movesAvailable.Length > i)
            {
                magicButtons[i].gameObject.SetActive(true);

                magicButtons[i].spellName = activeBattlers[currentTurn].movesAvailable[i];
                magicButtons[i].nameText.text = magicButtons[i].spellName;
                
                for(int j = 0; j < movesList.Length; j++)
                {
                    if(movesList[j].moveName == magicButtons[i].spellName)
                    {
                        magicButtons[i].spellCost = movesList[j].moveCost;
                        magicButtons[i].costText.text = magicButtons[i].spellCost.ToString();
                    }
                }
            }
            else
            {
                magicButtons[i].gameObject.SetActive(false);
            }
        }
    }

    public void Flee()
    {
        if(cannotFlee)
        {
            battleNotice.theText.text = "YOU CANNOT FLEE THIS BATTLE";
            battleNotice.Activate();
        }
        else
        {
            //there is a 35%(chanceToFlee) chance to flee, and so a random number is picked. If it's above 35 then you fell, if not then flee fails. This same method can be used for randomizing other things like attacks and misses.
            int fleeSuccess = Random.Range(0, 100);

            if(fleeSuccess < chanceToFlee)
            {
                //end the battle
                //battleActive = false;
                //battleScene.SetActive(false);
                fleeing = true;
                StartCoroutine(EndBattleCo());
            }
            else
            {
                NextTurn();
                battleNotice.theText.text = "YOU COULDN'T ESCAPE";
                battleNotice.Activate();
            }
        }
        
    }

//Below is not my code. Someone on course 105 provided it but it basically handles all of the item menu functionality.
     public void ShowItem()
    {
        itemMenu.SetActive(true);

        GameManager.instance.SortItems();

        for (int i = 0; i < itemButtons.Length; i++)
        {
            itemButtons[i].buttonValue = i;

            if (GameManager.instance.itemsHeld[i] != "")
            {
                itemButtons[i].buttonImage.gameObject.SetActive(true);

                itemButtons[i].buttonImage.sprite = GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[i]).itemSprite;

                itemButtons[i].amountText.text = GameManager.instance.numberOfItems[i].ToString();
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

        if (activeItem.isItem)
        {
            useButtonText.text = "Use";

        }

        if (activeItem.isWeapon || activeItem.isArmor)
        {
            useButtonText.text = "Equip";

        }

        itemName.text = activeItem.itemName;

        itemDescription.text = activeItem.description;

    }



    public void OpenItemCharChoice()
    {
        itemCharChoiceMenu.SetActive(true);

        for (int i = 0; i < itemCharChoiceNames.Length; i++)
        {
            itemCharChoiceNames[i].text = GameManager.instance.playerStats[i].characterName;

            itemCharChoiceNames[i].transform.parent.gameObject.SetActive(GameManager.instance.playerStats[i].gameObject.activeInHierarchy);
        }
    }

    public void CloseItemCharChoice()
    {
        itemCharChoiceMenu.SetActive(false);
    }



    public void Useitem(int selectChar)
    {
        activeItem.Use(selectChar);       

        UpdateUIStats();
        UpdateCharacterAndBattleStats(selectChar);

        CloseItemCharChoice();

        itemMenu.SetActive(false);       

        NextTurn();
    }
    //Course Code End

    //Going rogue for the first time, wish me luck
    public void UpdateCharacterAndBattleStats(int target)
    {
        CharStats selectedChar = GameManager.instance.playerStats[target];

        activeBattlers[target].currentHP = selectedChar.currentHP;
        activeBattlers[target].currentMP = selectedChar.currentMP;

        //LETS FUCKING GO THIS WORKS!
        //So this only functions during the battle. When the battle ends we use a different function. Depending on checkpoints and stuff like that we may want to change this.
    }

    //Okay so here's the deal. There is no solution in the course for the items menu. For the most part, I have figured it out, but there's one major flaw. When items are used they only update
    //the game manager but not the actual Battle Manager UI itself. I'll need to find a way to connect the 2 and make them update together. For another day perhaps.
    //One solution could be updating both stats in the same place, like when an Item is used or something like that, it gets updated to two different places at the same time
    //ideally the solution would be to make these stats connect into one stat for each. It would be hard to go back through it all but that might need to happen.

    public IEnumerator EndBattleCo()
    {
        battleActive = false;
        uiButtonsHolder.SetActive(false);
        targetMenu.SetActive(false);
        magicMenu.SetActive(false);
        itemMenu.SetActive(false);

        yield return new WaitForSeconds(0.5f);

        UIFade.instance.FadeToBlack();

        yield return new WaitForSeconds(1.5f);

        for(int i = 0; i < activeBattlers.Count; i++)
        {
            if(activeBattlers[i].isPlayer)
            {
                for(int j = 0; j < GameManager.instance.playerStats.Length; j++)
                {
                    if(activeBattlers[i].charName == GameManager.instance.playerStats[j].characterName)
                    {
                        GameManager.instance.playerStats[j].currentHP = activeBattlers[i].currentHP;
                        GameManager.instance.playerStats[j].currentMP = activeBattlers[i].currentMP;
                    }
                }
            }
            Destroy(activeBattlers[i].gameObject);
        }

        UIFade.instance.FadeFromBlack();
        battleScene.SetActive(false);
        activeBattlers.Clear();
        currentTurn = 0;
        //GameManager.instance.battleActive = false;
        if(fleeing)
        {
            GameManager.instance.battleActive = false;
            fleeing = false;
        }
        else
        {
            //open reward screen
            BattleReward.instance.OpenRewardScreen(rewardExp, rewardItems);
        }

        AudioManager.instance.PlayBGM(FindObjectOfType<CameraController>().musicToPlay);
    }

    public IEnumerator GameOverCo()
    {
        battleActive = false;
        UIFade.instance.FadeToBlack();

        yield return new WaitForSeconds(1.5f);
        battleScene.SetActive(false);

        SceneManager.LoadScene(gameOverScene);
    }
}
