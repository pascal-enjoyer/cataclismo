using UnityEngine;

public class Enemy : MonoBehaviour
{
    private EnemyData enemyData;

    public EnemyData EnemyData;

    public void Start()
    {
        Debug.Log("Hello from " + enemyData.enemyName);
    }
}
