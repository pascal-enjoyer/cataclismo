
using UnityEngine;
using UnityEngine.VFX;

namespace FireballMovement
{    public class FireballHorizontalMovement : MonoBehaviour
    {
        [SerializeField] private Vector3 _startingPos;
        [SerializeField] private float _moveSpeed = 0.1f;
        [SerializeField] private Transform target;
        [SerializeField] private float delay = 6f;
        [SerializeField] private float timer = 0f;
        [SerializeField] private Transform secondSphere;
        private Rigidbody _fireball;

        private const string _fireballTrailsActiveString = "FireballTrailsActive";

        void Start()
        {           
            _fireball = gameObject.GetComponent<Rigidbody>();
            _startingPos = _fireball.transform.localPosition;
            target = transform.parent.GetComponent<PlayerInfo>().currentEnemy;
        }

        void FixedUpdate()
        {
            if (timer < delay)
            {
                timer += Time.deltaTime;
                _fireball.AddForce((new Vector3(target.position.x, target.position.y+2f, target.position.z) - transform.position) * _moveSpeed);
                if (secondSphere != null)
                {
                    secondSphere.Rotate(new Vector3(0, 0, Time.deltaTime * 1500f));
                }
            }
            else if (timer >= delay)
            {
                ResetAll();
            }
        }

        private void ResetAll()
        {
            _fireball.velocity = Vector3.zero;
            _fireball.transform.localPosition = _startingPos;
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
}