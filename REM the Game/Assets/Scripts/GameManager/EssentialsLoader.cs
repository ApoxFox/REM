using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssentialsLoader : MonoBehaviour
{
    public GameObject uIScreen;
    public GameObject player;
    public GameObject gameManager;
    public GameObject audioManager;
    public GameObject battleManager;


    private void Start()
    {
        //This script is for instantiating important objects incase they are somehow destroyed between scenes. Object instances are set beforehand in their respective scripts.

        if(PlayerController.instance == null)
        {
            Instantiate(player);
        }

        if(UIFade.instance == null)
        {
            Instantiate(uIScreen);
        }

        if(GameManager.instance == null)
        {
            Instantiate(gameManager);
        }

        if(AudioManager.instance == null)
        {
            Instantiate(audioManager);
        }

        if(BattleManager.instance == null)
        {
            //Instantiate(battleManager);
        }
    }

    
    void Update()
    {
        
    }
}
