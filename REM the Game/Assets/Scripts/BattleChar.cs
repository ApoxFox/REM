using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleChar : MonoBehaviour
{
    public bool isPlayer;
    public string[] movesAvailable;


    public string charName;
    public int currentHP, maxHP, currentMP, MaxMP, strength, defence, wpnPower, armorPower;
    public bool hasDied;

    public SpriteRenderer spriteRenderer;
    public Sprite aliveSprite;
    public Sprite deadSprite;

    private bool shouldFade;
    public float fadeSpeed = 1f;

    void Start()
    {
        
    }

    
    void Update()
    {
        if(shouldFade == true)
        {
            spriteRenderer.color = new Color(Mathf.MoveTowards(spriteRenderer.color.r, 0, fadeSpeed * Time.deltaTime), Mathf.MoveTowards(spriteRenderer.color.g, 0, fadeSpeed * Time.deltaTime), Mathf.MoveTowards(spriteRenderer.color.b, 0, fadeSpeed * Time.deltaTime), Mathf.MoveTowards(spriteRenderer.color.a, 0, fadeSpeed * Time.deltaTime));

            if(spriteRenderer.color.a == 0)
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void EnemyFade()
    {
        shouldFade = true;
    }
}
