using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
using UnityEngine.UI;

public class SpellInfoInGrimoire : MonoBehaviour
{
    public Spell spell;

    public Element[] elements => spell.requiredElements;
    public string spellName => spell.spellName;
    public string spellDescription => spell.spellDescription;
    public SpellType spellType => spell.spellType;

    public Image[] elementSlots;
    public Text spellNameSlot;
    public Text spellDescriptionSlot;
    public Text spellTypeSlot;

    public void SetupSpellInfo(Spell spell)
    {
        this.spell = spell;
        for (int i = 0; i < elements.Length; i++) 
        {
            elementSlots[i].sprite = elements[i].icon;
        }
        spellNameSlot.text = spellName;
        spellDescriptionSlot.text = spellDescription;
        spellTypeSlot.text = spellType.ToString() + " type of damage";
    }
}
