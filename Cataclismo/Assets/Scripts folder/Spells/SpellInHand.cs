using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellInHand : MonoBehaviour
{
    public Spell spell;


    private void OnCollisionEnter(Collision collision)
    {
        ActiveEnemy enemy = collision.transform.GetComponent<ActiveEnemy>();
        if (enemy != null)
        {
            enemy.takeDamage(spell.spellDamage);
        }
        Destroy(gameObject);
    }
}
