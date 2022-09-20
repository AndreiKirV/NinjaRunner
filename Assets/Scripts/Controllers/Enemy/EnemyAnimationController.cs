namespace game.controllers.enemy
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

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
    }
}