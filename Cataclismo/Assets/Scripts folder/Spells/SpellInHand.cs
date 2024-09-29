using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class SpellInHand : MonoBehaviour
{
    public Spell spell;
    public PlayerInfo playerInfo;
    public int sumAttackDamage;
    public float boostMultiplier;
    public ActiveEnemy enemy;


    public void Start()
    {
        enemy = playerInfo.currentEnemy.GetComponent<ActiveEnemy>();
        sumAttackDamage = spell.spellDamage + playerInfo.itemAttackBonus;
        if (playerInfo.currentElementalStormBoost != null && playerInfo.currentElementalStormBoost != gameObject && !transform.GetComponent<SoftGround>() 
            && !transform.GetComponent<SwampFog>() && !transform.GetComponent<SteamExplosion>())
        {
            sumAttackDamage = (int)(sumAttackDamage * playerInfo.currentElementalStormBoost.GetComponent<SpellInHand>().spell.elementalStormBoost)/100;
            playerInfo.currentElementalStormBoost.GetComponent<ElementalStorm>().DestroyElementalStorm();
        }
        if (transform.GetComponent<SoftGround>() != null)
        {
            transform.GetComponent<SoftGround>().StartSpell();
        }
        if (enemy.ResistToSpells.ContainsKey(spell.spellType))
        {
            int tempResistPercentage = enemy.ResistToSpells[spell.spellType];
            if (tempResistPercentage > 100)
            {
                tempResistPercentage = 100;

            }
            else if (tempResistPercentage < 0)
            {
                tempResistPercentage = 0; 
            }
            sumAttackDamage = (sumAttackDamage * ((100 -  tempResistPercentage)))/100;
      
        }
    }

    public void BoostSpell(int multiplier)
    {
        boostMultiplier *= multiplier;
    }
}
