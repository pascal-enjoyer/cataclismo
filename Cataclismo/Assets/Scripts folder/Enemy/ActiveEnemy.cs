using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActiveEnemy : MonoBehaviour
{
    public Enemy enemy;
    public PlayerInfo playerInfo;
    public GameObject EnemyGameobject;

    [SerializeField] private float currentHealth;
    [SerializeField] private float maxHealth;

    [SerializeField] private float currentDamage;
    [SerializeField] private float maxDamage;


    public float currentAttackSpeed;
    public float maxAttackSpeed;

    public float currentAttackSpeedMultiplier = 1f;
    public List<float> attackSpeedMultipliers;

    public bool isAttackLineRefresh = false;

    public bool isSwampFogged = false;

    [SerializeField] public UnityEvent OnEnemyTakedDamage;
    [SerializeField] public UnityEvent OnEnemyDied;
     
    [SerializeField] public bool isDead;
    public void Start()
    {
        currentHealth = enemy.currentHealth;
        maxHealth = enemy.maxHealth;
        currentDamage = enemy.currentDamage;
        maxDamage = enemy.maxDamage;
        currentAttackSpeed = enemy.currentAtackSpeed;
        maxAttackSpeed = enemy.maxAtackSpeed;
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
            isDead = true;
            DestroyEnemy();
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

        System.Random r = new System.Random();
        bool isHit = true;
        if (isSwampFogged)
        {
            isHit = r.Next(0, 2) == 0;
        }
        if (isHit) 
        {
            playerInfo.takeDamage(currentDamage);
        }
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

    public void DestroyEnemy()
    {
        Destroy(EnemyGameobject);
    }

    public void SlowDownEnemy(float attackDebuff)
    {
        attackSpeedMultipliers.Add(attackDebuff);
        RefreshEnemyMultiplier();


    }

    public void BoostUpEnemy(float attackDebuff)
    {
        attackSpeedMultipliers.Remove(attackDebuff);
        RefreshEnemyMultiplier();
    }

    public void RefreshEnemyMultiplier()
    {
        currentAttackSpeedMultiplier = 1f;
        foreach (float temp in attackSpeedMultipliers)
        {
            if (currentAttackSpeedMultiplier - temp >= 0)
                if (temp >= 1)
                {
                    currentAttackSpeedMultiplier -= temp/100;
                }
                else
                    currentAttackSpeedMultiplier -= temp;
        }
    }
}
