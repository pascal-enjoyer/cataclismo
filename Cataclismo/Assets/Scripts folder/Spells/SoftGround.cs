using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoftGround : MonoBehaviour
{
    public Vector3 targetScale = new Vector3(1, 1, 1); // Целевой размер
    public float scaleDuration = 1f; // Время на увеличение/уменьшение масштаба
    public float stayDuration = 2f; // Время, в течение которого объект остается на месте перед уменьшением

    public void StartSoftGround()
    {
        // При спавне объект увеличивается до targetScale, затем ждёт и уменьшается
        StartCoroutine(ScaleUpAndWait());
        if (transform.GetComponent<SpellInHand>() && transform.GetComponent<SpellInHand>().enemy != null) 
        {
            transform.GetComponent<SpellInHand>().enemy.SlowDownEnemy(transform.GetComponent<SpellInHand>().spell.slowingDownPercentage);
        }
    }


    private IEnumerator ScaleUpAndWait()
    {
        // Увеличение объекта
        yield return StartCoroutine(ScaleUp(targetScale, scaleDuration));

        // Ожидание в полном размере
        yield return new WaitForSeconds(stayDuration);

        // Уменьшение объекта и его уничтожение
        yield return StartCoroutine(ScaleDown(Vector3.zero, scaleDuration));
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
}
