using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DamageNumbers : MonoBehaviour
{
    public TMP_Text damageText;

    public float lifetime = 0.5f;
    public float moveSpeed = 1;
    public float placementJitter = .5f;
    
    void Start()
    {
        
    }

    
    void Update()
    {
        Destroy(gameObject, lifetime);

        transform.position += new Vector3(0f, moveSpeed * Time.deltaTime, 0f);

    }

    public void SetDamage(int damageAmount)
    {
        damageText.text = damageAmount.ToString();
        transform.position += new Vector3(Random.Range(-placementJitter, placementJitter), Random.Range(-placementJitter, placementJitter), 0);
    }
}
