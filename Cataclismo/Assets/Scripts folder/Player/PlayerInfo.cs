using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public Player player;
    [SerializeField] public Transform currentEnemy;


    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;

    private void Start()
    {
        maxHealth = player.maxHealth;
        currentHealth = maxHealth;
    }


}
