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


}
