using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenBuildings : MonoBehaviour
{
    public GameObject exterior;
    public GameObject interior;
    

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.CompareTag("Player"))
        {
            exterior.SetActive(false);
            interior.SetActive(true);
            
            PlayerController.instance.isIndoors = true;
        }
        
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if(other.gameObject.CompareTag("Player"))
        {
            interior.SetActive(false);
            exterior.SetActive(true);

            PlayerController.instance.isIndoors = false;
        }
        
    }
}
