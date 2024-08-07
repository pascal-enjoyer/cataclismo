using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBars : MonoBehaviour
{
    [SerializeField] private PlayerInfo player;
    [SerializeField] private Transform healthLine;

    [SerializeField] private float updateSpeedSeconds = 0.5f; // Скорость обновления полоски здоровья

    private Coroutine updateCoroutine;

    private void Start()
    {
        player.OnPlayerGetDamage.AddListener(RefreshHealthLine);   
    }

    public void RefreshHealthLine()
    {
        Image img = healthLine.GetComponent<Image>();
        if (img != null && player != null)
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
        float targetFillAmount = player.GetCurrentHealth() / player.GetMaxHealth();
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
