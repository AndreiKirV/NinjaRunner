namespace game.controllers.player
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using dictionaries;

    public class PlayerAnimationController
    {
        private Animator _animator;

        public void SetPlayer(GameObject player)
        {
            _animator = player.GetComponent<Animator>();
        }

        private void ChangeFlag(string name)
        {
            _animator.SetBool(name, !_animator.GetBool(name));
        }
    }
}