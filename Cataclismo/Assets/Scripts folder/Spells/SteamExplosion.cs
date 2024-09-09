using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamExplosion : MonoBehaviour
{
    Transform target;
    public void Start()
    {
        target = transform.parent.GetComponent<PlayerInfo>().currentEnemy;
        DebuffEnemy();
    }

    public void  DebuffEnemy()
    {
        ActiveEnemy tempEnemy = target.GetComponent<ActiveEnemy>();
        tempEnemy.isAttackLineRefresh = true;
    }
}
