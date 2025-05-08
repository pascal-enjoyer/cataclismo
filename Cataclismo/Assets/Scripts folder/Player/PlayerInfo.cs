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
    public int itemHealthBonus;
    public int itemAttackBonus;
    

    public GameObject currentShield;
    public GameObject currentElementalStormBoost;
    public GameObject currentSoftGround;
    public GameObject currentSwampFog;

    public UnityEvent OnPlayerDied;

    private void Start()
    {
        maxHealth = player.maxHealth;
        currentHealth = maxHealth;
        inventory = GameManager.Instance.inventory;
        if (inventory != null)
            AddItemBonus();
        
    }


    public void AddItemBonus()
    {
        foreach (InventoryItem item in inventory.items)
        {
            if (item.isEquiped == true)
            {
                switch (item.BonusType)
                {
                    case BonusType.Defense:

                        maxHealth += item.bonusValue;
                        currentHealth = maxHealth;
                        break;
                    case BonusType.Attack:
                        itemAttackBonus += item.bonusValue;
                        break;
                }
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
                OnPlayerDied.Invoke();
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
