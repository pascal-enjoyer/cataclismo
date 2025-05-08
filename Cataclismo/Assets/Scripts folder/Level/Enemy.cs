using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyData enemyData;

    public EnemyData EnemyData => enemyData;

    public PlayerInfo playerInfo;

    // сделать события и подписать интерфейс на них

    [SerializeField] private float currentHealth;
    [SerializeField] private float maxHealth;

    [SerializeField] private float currentDamage;


    public float currentAttackSpeed;


    public List<float> attackSpeedMultipliers;

    [SerializeField] public UnityEvent OnEnemyTakedDamage;
    [SerializeField] public UnityEvent OnEnemyDied;

    [SerializeField] public bool isDead;

    public void Start()
    {
        RefreshEnemyStats();
    }

    public void RefreshEnemyStats()
    {
        maxHealth = enemyData.health;
        currentHealth = maxHealth;
        currentDamage = enemyData.damage;

        currentAttackSpeed = enemyData.attackSpeed;
    }

    public void TakeDamage(int damage)
    {
        int tempDmg = damage;
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

    public void TakeHealth(int heal)
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

    public void AttackPlayer()
    {

/*        System.Random r = new System.Random();
        bool isHit = true;
        if (isSwampFogged)
        {
            isHit = r.Next(0, 2) == 0;
        }
        if (isHit)
        {
            playerInfo.takeDamage(currentDamage);
        }*/
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



    public void DestroyEnemy()
    {
        Destroy(gameObject);
    }



}
