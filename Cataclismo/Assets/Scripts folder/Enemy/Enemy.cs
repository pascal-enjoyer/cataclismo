using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy", order = 51)]
public class Enemy : ScriptableObject
{
    public string enemyName;

    public float currentHealth;
    public float maxHealth;

    public float currentAtackSpeed;
    public float maxAtackSpeed;

    public float currentDamage;
    public float maxDamage;

    public List<SpellsResist> spellTypesResist;


}

[System.Serializable]
public class SpellsResist
{
    public SpellType spellType;
    public int ResistPercentage;
}