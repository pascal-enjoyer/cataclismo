using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBars : MonoBehaviour
{
    [SerializeField] private ActiveEnemy enemy;
    [SerializeField] private Image healthLine;
                    public Image attackLine;
    public Text enemyName;
    public Text enemyHealthAmount;
    [SerializeField] private float attackTime;

    [SerializeField] private float updateSpeedSeconds = 0.5f; // —корость обновлени€ полоски здоровь€
    private Coroutine updateCoroutine;

    public void SetupEnemy(ActiveEnemy enemy)
    {
        this.enemy = enemy;
        enemyName.text = enemy.enemy.enemyName;
        transform.position = Camera.main.WorldToScreenPoint(enemy.transform.position + new Vector3(0, enemy.transform.localScale.y * 2));
        enemy.OnEnemyTakedDamage.AddListener(RefreshHealthLine);
    }
    private void Update()
    {
        if (enemy && !enemy.isDead)
        {
            if (!enemy.isAttackLineRefresh)
            {
                attackTime += Time.deltaTime * enemy.currentAttackSpeedMultiplier;
            }
            else
            {
                attackTime = 0;
                enemy.isAttackLineRefresh = false;
            }

            attackLine.fillAmount = attackTime / enemy.currentAttackSpeed;

            if (attackLine.fillAmount >= 1)
            {
                attackLine.fillAmount = 0;
                attackTime = 0;
                //тут атака
                enemy.attackPlayer();
            }
        }
    }

    

    public void RefreshHealthLine()
    {
        
        if (healthLine != null && enemy != null)
        {

            if (updateCoroutine != null)
            {
                StopCoroutine(updateCoroutine);
            }
            updateCoroutine = StartCoroutine(UpdateHealthBar());
        }
    }

    private IEnumerator UpdateHealthBar()
    {
        
        float preChangePercent = healthLine.fillAmount;
        float targetFillAmount = enemy.GetCurrentHealth() / enemy.GetMaxHealth();
        float elapsed = 0f;
        enemyHealthAmount.text = enemy.GetCurrentHealth().ToString();

        while (elapsed < updateSpeedSeconds)
        {
            elapsed += Time.deltaTime;
            healthLine.fillAmount = Mathf.Lerp(preChangePercent, targetFillAmount, elapsed / updateSpeedSeconds);
            yield return null;
        }
        healthLine.fillAmount = targetFillAmount;
    }
}
