using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoftGround : MonoBehaviour
{
    public Vector3 targetScale = new Vector3(1, 1, 1); // Целевой размер
    public float scaleDuration = 1f; // Время на увеличение/уменьшение масштаба
    public float stayDuration = 2f; // Время, в течение которого объект остается на месте перед уменьшением


    
    public void StartSpell()
    {

        // При спавне объект увеличивается до targetScale, затем ждёт и уменьшается
        stayDuration = transform.GetComponent<SpellInHand>().spell.duration;

        
        if (transform.GetComponent<SpellInHand>() != null && transform.GetComponent<SpellInHand>().enemy != null)
        {
            transform.GetComponent<SpellInHand>().enemy.SlowDownEnemy(transform.GetComponent<SpellInHand>().spell.slowingDownPercentage);
        }
        StartCoroutine(ScaleUpAndWait());
    }


    private IEnumerator ScaleUpAndWait()
    {
        // Увеличение объекта
        yield return StartCoroutine(ScaleUp(targetScale, scaleDuration));

        // Ожидание в полном размере
        yield return new WaitForSeconds(stayDuration);

        // Уменьшение объекта и его уничтожение
        yield return StartCoroutine(ScaleDown(Vector3.zero, scaleDuration));
        BuffEnemyAttackSpeedBack();
        Destroy(gameObject); // Уничтожить объект после уменьшения
    }

    public IEnumerator ScaleUp(Vector3 finalScale, float duration)
    {
        Vector3 initialScale = Vector3.zero; // Начальный размер
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            transform.localScale = Vector3.Lerp(initialScale, finalScale, t);
            yield return null;
        }

        transform.localScale = finalScale; // Установить точный финальный размер
    }

    public IEnumerator ScaleDown(Vector3 finalScale, float duration)
    {
        Vector3 initialScale = transform.localScale; // Текущий размер объекта
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            transform.localScale = Vector3.Lerp(initialScale, finalScale, t);
            yield return null;
        }

        transform.localScale = finalScale; // Установить финальный размер (0)

    }

    public void BuffEnemyAttackSpeedBack()
    {
        transform.GetComponent<SpellInHand>().enemy.BoostUpEnemy(transform.GetComponent<SpellInHand>().spell.slowingDownPercentage);
    }
}
