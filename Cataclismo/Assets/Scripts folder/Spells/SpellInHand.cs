using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class SpellInHand : MonoBehaviour
{
    public Spell spell;
    public PlayerInfo playerInfo;
    public float sumAttackDamage;
    public float boostMultiplier;
    public ActiveEnemy enemy;


    public void Start()
    {
        enemy = playerInfo.currentEnemy.GetComponent<ActiveEnemy>();
        sumAttackDamage = spell.spellDamage + playerInfo.itemAttackBonus;
        if (playerInfo.currentElementalStormBoost != null && playerInfo.currentElementalStormBoost != gameObject && spell.isShield == false)
        {
            sumAttackDamage *= playerInfo.currentElementalStormBoost.GetComponent<SpellInHand>().spell.elementalStormBoost;
            playerInfo.currentElementalStormBoost.GetComponent<ElementalStorm>().DestroyElementalStorm();
        }
        if (transform.GetComponent<SoftGround>() != null)
        {
            transform.GetComponent<SoftGround>().StartSpell();
        }
    }

    public void BoostSpell(float multiplier)
    {
        boostMultiplier *= multiplier;
    }
}
