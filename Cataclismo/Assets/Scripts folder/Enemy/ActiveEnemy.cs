using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActiveEnemy : MonoBehaviour
{
    public Enemy enemy;
    public PlayerInfo playerInfo;

    [SerializeField] private float currentHealth;
    [SerializeField] private float maxHealth;

    [SerializeField] private float currentDamage;
    [SerializeField] private float maxDamage;

    [SerializeField] public UnityEvent OnEnemyTakedDamage;
    [SerializeField] public UnityEvent OnEnemyDied;


    public void Start()
    {
        currentHealth = enemy.currentHealth;
        maxHealth = enemy.maxHealth;
        currentDamage = enemy.currentDamage;
        maxDamage = enemy.maxDamage;
    }

    public void RefreshEnemyStats()
    {
        currentHealth = enemy.currentHealth;
        maxHealth = enemy.maxHealth;
        currentDamage = enemy.currentDamage;
        maxDamage = enemy.maxDamage;
    }
    public void takeDamage(float damage)
    {
        float tempDmg = damage;
        if (currentHealth - damage <= 0)
        {
            currentHealth = 0;
            OnEnemyDied.Invoke();
        }
        else
            currentHealth -= damage;
        transform.GetComponent<PopUpDamage>().PopUp(tempDmg);
        OnEnemyTakedDamage.Invoke();
    }

    public void takeHealth(int heal)
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

    public void attackPlayer()
    {
        playerInfo.takeDamage(currentDamage);
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public Enemy GetEnemy()
    {
        return enemy;
    }




}
