using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideFacingStairs : MonoBehaviour
{
    public bool facingRight;


    private void OnTriggerEnter2D(Collider2D other) 
    {
        PlayerController.instance.onSideFacingStairs = true;

        if(facingRight && other.gameObject.CompareTag("Player"))
        {
            PlayerController.instance.stairsFacingRight = true;
        }

        if(!facingRight && other.gameObject.CompareTag("Player"))
        {
            PlayerController.instance.stairsFacingRight = false;
        }
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        PlayerController.instance.onSideFacingStairs = false;
    }
}
