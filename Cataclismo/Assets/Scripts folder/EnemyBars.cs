using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBars : MonoBehaviour
{
    [SerializeField] private Transform EnemyHead;


    private void Update()
    {

       transform.position = Camera.main.WorldToScreenPoint(EnemyHead.position + new Vector3(0,0.4f));
    }
}
