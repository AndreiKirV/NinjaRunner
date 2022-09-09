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

        public void Jump()
        {
            ChangeFlag("IsJump");
        }

        public void Run()
        {
            if (!_animator.GetBool("IsRun"))
            {
                ChangeFlag("IsRun");
            }
        }

        public void StopRun()
        {
            _animator.SetBool("IsRun", false);
        }

        public void JumpObstacle()
        {
            int randomAnim = Random.Range(1,6);
            ChangeFlag($"JumpObstacle{randomAnim}");
        }

        public void CrashedJump()
        {
            int randomAnim = Random.Range(1,2);
            ChangeFlag($"CrashedJump1");
        }
    }
}