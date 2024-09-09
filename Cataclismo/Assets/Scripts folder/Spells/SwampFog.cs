using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwampFog : MonoBehaviour
{
    public ActiveEnemy enemy;
    public void StartFog()
    {
        enemy = transform.parent.GetComponent<PlayerInfo>().currentEnemy.GetComponent<ActiveEnemy>();

        enemy.isSwampFogged = true;
        StartCoroutine(DestroyAfterTime());
    }

    // Корутине, которая ждет заданное количество времени перед уничтожением
    IEnumerator DestroyAfterTime()
    {
        // Ждем указанное время
        yield return new WaitForSeconds(transform.GetComponent<SpellInHand>().spell.duration);
        DestroyFog();

    }

    public void DestroyFog()
    {

        // Уничтожаем объект
        enemy.isSwampFogged = false;
        Destroy(gameObject);
    }


}
