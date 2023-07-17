using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    private bool canPickUp;
    
    void Start()
    {
        
    }

    
    void Update()
    {
        if(canPickUp && Input.GetButtonDown("Fire1") && PlayerController.instance.canMove == true)
        {
            GameManager.instance.AddItem(GetComponent<Item>().itemName);

            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.tag == "Player")
        {
            canPickUp = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if(other.tag == "Player")
        {
            canPickUp = false;
        }
    }
}
