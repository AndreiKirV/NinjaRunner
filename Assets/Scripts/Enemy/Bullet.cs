namespace game.enemy
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using dictionaries;
    using game.controllers.player;
    using UnityEngine.Events;

    public class Bullet : MonoBehaviour
    {
        [SerializeField] Animator _animator;
        private float _speed = 65;
        private bool _rebounded = false;
        public UnityEvent <GameObject> Destroyed = new UnityEvent<GameObject>();

        private void OnCollisionEnter2D(Collision2D other) 
        {
            if(other.gameObject.name == ObjectNames.Character)
            {
                other.gameObject.GetComponent<Player>().TryStartedClash();
                Ruin();
            }
            else if (other.gameObject.name == ObjectNames.EnemyRanged)
            {
                other.gameObject.GetComponent<EnemyRanged>().Death();
                Ruin();
            }
            else if (other.gameObject.name == ObjectNames.Bullet)
            {
                if (_rebounded)
                Ruin();
                else
                ReverseDirection();
            }
        }

        private void Start() 
        {
            Destroy(gameObject, 15);
        }

        private void OnTriggerEnter2D(Collider2D other) 
        {
            if(other.gameObject.name == ObjectNames.AttackBox)
            {
                _speed = -other.gameObject.transform.parent.GetComponent<Player>().CurrentSpeed - _speed;
                _rebounded = true;
            }
        }

        private void Update() 
        {
            transform.position = Vector2.Lerp(transform.position, new Vector2(transform.position.x - _speed, transform.position.y), Time.deltaTime);
        }

        private void ReverseDirection()
        {
            _speed *= - 1;
        }

        private void Ruin()
        {
            _animator.SetTrigger("Blast");
            Destroyed.Invoke(gameObject);
            Destroy(gameObject, 5);
        }
    }
}