using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    private Spell spell;

    public void Start()
    {
        spell = GetComponent<SpellInHand>().spell;
    }
    
    public void DestroyShield()
    {
        Destroy(gameObject);
    }
}
