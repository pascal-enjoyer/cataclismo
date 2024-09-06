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

    [SerializeField] private float updateSpeedSeconds = 0.5f; // —корость обновлени€ полоски здоровь€

    private Coroutine updateCoroutine;

    private void Start()
    {
        enemy.OnEnemyTakedDamage.AddListener(RefreshHealthLine);
    }
    private void Update()
    {
        if (!enemy.isDead)
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
    }

    

    public void RefreshHealthLine()
    {
        Image img = healthLine.GetComponent<Image>();
        if (img != null && enemy != null)
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
        Image img = healthLine.GetComponent<Image>();
        float preChangePercent = img.fillAmount;
        float targetFillAmount = enemy.GetCurrentHealth() / enemy.GetMaxHealth();
        float elapsed = 0f;

        while (elapsed < updateSpeedSeconds)
        {
            elapsed += Time.deltaTime;
            img.fillAmount = Mathf.Lerp(preChangePercent, targetFillAmount, elapsed / updateSpeedSeconds);
            yield return null;
        }
        img.fillAmount = targetFillAmount;
    }
}
