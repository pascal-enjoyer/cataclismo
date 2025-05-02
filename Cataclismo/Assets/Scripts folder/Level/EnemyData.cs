using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy", order = 51)]
public class EnemyData : ScriptableObject
{
    public string enemyName;

    public float maxHealth;

    public float maxAtackSpeed;

    public float maxDamage;

    public List<SpellsResist> spellTypesResist;


}

[System.Serializable]
public class SpellsResist
{
    public SpellType spellType;
    public int ResistPercentage;
}