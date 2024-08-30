using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spell", menuName = "Spell", order = 51)]
public class Spell : ScriptableObject
{
    public string spellName;
    public Element[] requiredElements;
    public string spellDescription;
    //если тип дамага подходит под слабости врага то 150% урона, если такой же как враг, то 25% урона и т.д
    public SpellType spellType;
    public bool isShield;
    public int spellDamage;

    public float elementalStormBoost;

}
public enum SpellType
{
    Water,
    Fire,
    Earth,
    unique
}
