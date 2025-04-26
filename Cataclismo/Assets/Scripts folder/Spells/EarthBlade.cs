using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthBlade : MonoBehaviour
{
        [SerializeField] private Vector3 _startingPos;
        [SerializeField] private float _moveSpeed = 0.1f;
        [SerializeField] private Transform target;
        [SerializeField] private float delay = 6f;
        [SerializeField] private float timer = 0f;
        private Rigidbody _earthBlade;


        void Start()
        {
            _earthBlade = gameObject.GetComponent<Rigidbody>();
            _startingPos = _earthBlade.transform.localPosition;
            target = transform.parent.GetComponent<PlayerInfo>().currentEnemy;
        }

        void FixedUpdate()
        {
            if (timer < delay)
            {
                timer += Time.deltaTime;
                 _earthBlade.AddForce((new Vector3(target.position.x, target.position.y + 2f, target.position.z) - transform.position) * _moveSpeed);
                transform.Rotate(new Vector3(0,Time.deltaTime * 300f,0));
            }
            else if (timer >= delay)
            {
                ResetAll();
            }
        }

        private void ResetAll()
        {
             _earthBlade.linearVelocity = Vector3.zero;
            _earthBlade.transform.localPosition = _startingPos;
            timer = 0;
        }

        private void OnCollisionEnter(Collision collision)
        {
            ActiveEnemy enemy = collision.transform.GetComponent<ActiveEnemy>();
            if (enemy != null)
            {
                enemy.takeDamage(transform.GetComponent<SpellInHand>().sumAttackDamage);
            }
            Destroy(gameObject);
        }
}
