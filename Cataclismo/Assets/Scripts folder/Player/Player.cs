using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
[CreateAssetMenu(fileName = "New Player", menuName = "Player", order = 51)]
public class Player : ScriptableObject
{
    [Header("Required")]
    public string playerName;
    public float maxHealth;
    
}
