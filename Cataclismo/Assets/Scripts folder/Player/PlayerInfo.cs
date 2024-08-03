using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInfo : MonoBehaviour
{
    public Player player;
    [SerializeField] public Transform currentEnemy;


    [SerializeField] private float maxHealth;
    [SerializeField] private float currentHealth;
    [SerializeField] public UnityEvent OnPlayerGetDamage;

    private void Start()
    {
        maxHealth = player.maxHealth;
        currentHealth = maxHealth;
    }

    public void takeDamage(float damage)
    {

        if (currentHealth - damage <= 0)
        {
            currentHealth = 0;

        }
        else
            currentHealth -= damage;
        OnPlayerGetDamage.Invoke();
    }

    public void takeHealth(float heal) 
    {
        if (currentHealth + heal >= maxHealth)
        {
            currentHealth = maxHealth;
        }
        else
        {   
            currentHealth += heal; 
        }
    }

    public float  GetCurrentHealth()
    {
        return currentHealth;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }


    
}
