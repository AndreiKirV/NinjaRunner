namespace game.controllers.enemy
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using dictionaries;

    public class EnemyAnimationController
    {
        private Animator _animator;
        public EnemyAnimationController(GameObject gameObject)
        {
            _animator = gameObject.GetComponent<Animator>();
        }

        public void StartHit()
        {
            _animator.SetBool("Hit", true);
        }

        public void StartDeath()
        {
            _animator.SetTrigger("Death");
        }

        public void StartEscape()
        {
            _animator.SetTrigger("Escape");
        }

        public void StartTrickDeath(int value)
        {
            _animator.SetTrigger($"{PlayerStates.TrickDeath}{value}");
        }
    }
}