using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellInHand : MonoBehaviour
{
    public Spell spell;
    public PlayerInfo playerInfo;
    public float sumAttackDamage;
    public float boostMultiplier;
    public ActiveEnemy enemy;

    public void Start()
    {
        sumAttackDamage = spell.spellDamage + playerInfo.itemAttackBonus;
        if (playerInfo.currentElementalStormBoost != null && playerInfo.currentElementalStormBoost!= gameObject)
        {
            sumAttackDamage *= playerInfo.currentElementalStormBoost.GetComponent<SpellInHand>().spell.elementalStormBoost;
            playerInfo.currentElementalStormBoost.GetComponent<ElementalStorm>().DestroyElementalStorm();
        }
    }

    public void BoostSpell(float multiplier)
    {
        boostMultiplier *= multiplier;
    }
}
