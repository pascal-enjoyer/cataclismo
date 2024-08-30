using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoilingStream : MonoBehaviour
{

    [SerializeField] private float _moveSpeed = 0.1f;
    [SerializeField] private Transform target;
    [SerializeField] private float delay = 2f;
    [SerializeField] private float timer = 0f;
    [SerializeField] private Transform secondSphere;
    [SerializeField] private Transform thirdSphere;
    private Rigidbody boilindStream;


    void Start()
    {
        target = transform.parent.GetComponent<PlayerInfo>().currentEnemy;
    }

    void FixedUpdate()
    {
        if (timer < delay)
        {
            timer += Time.deltaTime;
            if (secondSphere != null)
            {
                transform.Rotate(new Vector3(0, Time.deltaTime * 750f, 0));
            }
        }
        else if (timer >= delay)
        {
            ResetAll();
        }
    }


    private void ResetAll()
    {
        target.GetComponent<ActiveEnemy>().takeDamage(transform.GetComponent<SpellInHand>().sumAttackDamage);
        timer = 0;
        Destroy(gameObject);
    }
}
