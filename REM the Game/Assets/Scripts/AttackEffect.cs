using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEffect : MonoBehaviour
{
    public float effectLength;
    public int sfx;
    
    void Start()
    {
        AudioManager.instance.PlaySFX(sfx);
    }

    
    void Update()
    {
        Destroy(gameObject, effectLength);
    }
}
