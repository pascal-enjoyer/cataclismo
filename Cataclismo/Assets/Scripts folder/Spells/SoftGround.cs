using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoftGround : MonoBehaviour
{
    public Vector3 targetScale = new Vector3(1, 1, 1); // ������� ������
    public float scaleDuration = 1f; // ����� �� ����������/���������� ��������
    public float stayDuration = 2f; // �����, � ������� �������� ������ �������� �� ����� ����� �����������


    
    public void StartSpell()
    {

        // ��� ������ ������ ������������� �� targetScale, ����� ��� � �����������
        stayDuration = transform.GetComponent<SpellInHand>().spell.duration;

        
        if (transform.GetComponent<SpellInHand>() != null && transform.GetComponent<SpellInHand>().enemy != null)
        {
            transform.GetComponent<SpellInHand>().enemy.SlowDownEnemy(transform.GetComponent<SpellInHand>().spell.slowingDownPercentage);
        }
        StartCoroutine(ScaleUpAndWait());
    }


    private IEnumerator ScaleUpAndWait()
    {
        // ���������� �������
        yield return StartCoroutine(ScaleUp(targetScale, scaleDuration));

        // �������� � ������ �������
        yield return new WaitForSeconds(stayDuration);

        // ���������� ������� � ��� �����������
        yield return StartCoroutine(ScaleDown(Vector3.zero, scaleDuration));
        BuffEnemyAttackSpeedBack();
        Destroy(gameObject); // ���������� ������ ����� ����������
    }

    public IEnumerator ScaleUp(Vector3 finalScale, float duration)
    {
        Vector3 initialScale = Vector3.zero; // ��������� ������
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            transform.localScale = Vector3.Lerp(initialScale, finalScale, t);
            yield return null;
        }

        transform.localScale = finalScale; // ���������� ������ ��������� ������
    }

    public IEnumerator ScaleDown(Vector3 finalScale, float duration)
    {
        Vector3 initialScale = transform.localScale; // ������� ������ �������
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            transform.localScale = Vector3.Lerp(initialScale, finalScale, t);
            yield return null;
        }

        transform.localScale = finalScale; // ���������� ��������� ������ (0)

    }

    public void BuffEnemyAttackSpeedBack()
    {
        transform.GetComponent<SpellInHand>().enemy.BoostUpEnemy(transform.GetComponent<SpellInHand>().spell.slowingDownPercentage);
    }
}
