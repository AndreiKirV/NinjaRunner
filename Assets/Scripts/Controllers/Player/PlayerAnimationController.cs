namespace game.controllers.player
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using dictionaries;

    public class PlayerAnimationController
    {
        private Animator _animator;
        private int _valueJumpObstacle;
        private int _valueCrashedJump;
        private int _valueDeathByObstacle;

        private int CheckedValueAnimation(string targetAnim)
        {
            bool isChecked = false;
            int  i = 0;

            while (isChecked == false)
            {
                AnimationClip temp = Resources.Load<AnimationClip>($"{Path.ANIMATIONS_PLAYER}{targetAnim}{i+1}");

                if (temp == null)
                {
                    isChecked = true;
                    break;
                }

                i++;
            }

            Resources.UnloadUnusedAssets();
            return i;
        }

        private void ChangeFlag(string name, bool targetState)
        {
            _animator.SetBool(name, targetState);
        }

        public void SetPlayer(GameObject player)
        {
            _animator = player.GetComponent<Animator>();
        }

        public void Jump()
        {
            ChangeFlag(PlayerStates.IsJump, true);
        }

        public void Run()
        {
            if (!_animator.GetBool(PlayerStates.IsRun))
            {
                ChangeFlag(PlayerStates.IsRun, true);
            }
        }

        public void Hit()
        {
            ChangeFlag(PlayerStates.Hit, true);
        }

        public void StopRun()
        {
            ChangeFlag(PlayerStates.IsRun, false);
        }

        public void JumpObstacle()
        {
            int randomAnim = Random.Range(1,_valueJumpObstacle+1);
            ChangeFlag($"{PlayerStates.JumpObstacle}{randomAnim}", true);
        }

        public void CrashedJump()
        {
            int randomAnim = Random.Range(1,_valueCrashedJump+1);
            ChangeFlag($"{PlayerStates.CrashedJump}{randomAnim}", true);
        }

        public void DeathByObstacle()
        {
            int randomAnim = Random.Range(1,_valueDeathByObstacle+1);
            ChangeFlag($"{PlayerStates.DeathByObstacle}{randomAnim}", true);
        }

        public void Init()
        {
            _valueCrashedJump = CheckedValueAnimation(PlayerStates.CrashedJump);
            _valueDeathByObstacle = CheckedValueAnimation(PlayerStates.DeathByObstacle);
            _valueJumpObstacle = CheckedValueAnimation(PlayerStates.JumpObstacle);
        }
    }
}