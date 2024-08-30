using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInfo : MonoBehaviour
{
    public Player player;
    public Transform currentEnemy;

    [SerializeField] private Inventory inventory;

    [SerializeField] private float maxHealth;
    [SerializeField] private float currentHealth;

    public UnityEvent OnPlayerGetDamage;
    public float itemHealthBonus;
    public float itemAttackBonus;
    

    public GameObject currentShield;
    public GameObject currentElementalStormBoost;

    private void Start()
    {
        maxHealth = player.maxHealth;
        currentHealth = maxHealth;
        inventory = GameManager.inventory;
        if (inventory != null)
            AddItemBonus();
        
    }


    public void AddItemBonus()
    {
        if (inventory.ring != null)
        {
            switch (inventory.ring.BonusType)
            {
                case BonusType.Defense:

                    maxHealth += inventory.ring.bonusValue;
                    currentHealth = maxHealth;
                    break;
                case BonusType.Attack:
                    itemAttackBonus += inventory.ring.bonusValue;
                    break;
            }
        }
        if (inventory.bracelet != null)
        {
            switch (inventory.bracelet.BonusType)
            {
                case BonusType.Defense:

                    maxHealth += inventory.bracelet.bonusValue;
                    currentHealth = maxHealth;
                    break;
                case BonusType.Attack:
                    itemAttackBonus += inventory.bracelet.bonusValue;
                    break;
            }
        }
        if (inventory.glove != null)
        {
            switch (inventory.glove.BonusType)
            {
                case BonusType.Defense:

                    maxHealth += inventory.glove.bonusValue;
                    currentHealth = maxHealth;
                    break;
                case BonusType.Attack:
                    itemAttackBonus += inventory.glove.bonusValue;
                    break;
            }
        }
    }



    public void takeDamage(float damage)
    {
        if (currentShield != null)
        {
            Destroy(currentShield);
        }
        else
        {
            if (currentHealth - damage <= 0)
            {
                currentHealth = 0;

            }
            else
                currentHealth -= damage;
        }

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
