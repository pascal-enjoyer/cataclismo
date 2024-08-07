using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBars : MonoBehaviour
{
    [SerializeField] private ActiveEnemy enemy;
    [SerializeField] private Transform healthLine;
    [SerializeField] private Transform attackBar;
    [SerializeField] private float attackTime;

    private void Start()
    {
        enemy.OnEnemyTakedDamage.AddListener(RefreshHealthLine);
    }
    private void Update()
    {

       transform.position = Camera.main.WorldToScreenPoint(enemy.transform.position + new Vector3(0, enemy.transform.localScale.y * 2));
        if (enemy != null)
        {
            Image img = attackBar.GetComponent<Image>();
            if (img != null)
            {

                attackTime += Time.deltaTime;
                img.fillAmount = attackTime / enemy.GetEnemy().currentAtackSpeed;
                if (img.fillAmount >= 1)
                {
                    img.fillAmount = 0;
                    attackTime = 0;
                    //тут атака
                    enemy.attackPlayer();
                }
            }
            
        }
    }


    public void RefreshHealthLine()
    {
        Image img = healthLine.GetComponent<Image>();
        if (img != null && enemy != null)
        {
            img.fillAmount = enemy.GetCurrentHealth() / enemy.GetMaxHealth();
        }
    }
}
