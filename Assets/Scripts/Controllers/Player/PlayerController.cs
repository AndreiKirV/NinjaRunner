namespace game.controllers.player
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using dictionaries;

    public class PlayerController
    {
        private Player _player;
        private GameObject _playerObject;
        private PlayerMovementController _movementController = new PlayerMovementController();
        private PlayerAnimationController _animationController = new PlayerAnimationController();

        public void SetPlayer(GameObject player)
        {
            _playerObject = player;
            _player = _playerObject.GetComponent<Player>();
            
            if (_player == null)
            _player = _playerObject.AddComponent<Player>();
        }

        public int GiveWallet()
        {
            return _player.Gold;
        }

        public void TageGold(int value)
        {
            _player.TakeGold(value);
        }

        public void SetButtonRun(Button button)
        {
            button.onClick.AddListener(_player.SetRunningState);
        }

        public void SetButtonJump(Button button)
        {
            button.onClick.AddListener(_player.SetJumpingState);
        }

        public void SetButtonHit(Button button)
        {
            button.onClick.AddListener(_player.SetHitState);
        }

        public void SetButtonSlide(Button button)
        {
            button.onClick.AddListener(_player.TrySetSlideState);
        }

        public void SetCamera(Camera camera)
        {
            _movementController.SetCamera(camera);
        }

        public void SetEventResetRunning(UnityEngine.Events.UnityAction call)
        {
            _player.ResetRunning.AddListener(call);
        }

        public void SetEventTouchHit(Button button, UnityEngine.Events.UnityAction call)
        {
            _movementController.TouchHit.AddListener(delegate {
                if (button.gameObject.activeSelf)
                {
                    call.Invoke();
                }
            });
        }

        public void Init() 
        {
            _player.StartedRunning.AddListener(_animationController.Run);
            _player.TrickWorked.AddListener(_animationController.JumpObstacle);
            _player.StartedJumping.AddListener(_animationController.Jump);
            _player.StartedJumping.AddListener(_movementController.Jump);
            _player.Crashed.AddListener(_animationController.CrashedJump);
            _player.ResetRunning.AddListener(_animationController.StopRun);
            _player.DeathByObstacle.AddListener(_animationController.DeathByObstacle);
            _player.StartedHit.AddListener(_animationController.Hit);
            _player.StartedSlide.AddListener(_animationController.Slide);
            _player.ResetSlide.AddListener(_animationController.StopSlide);
            _player.StartedIdle.AddListener(_animationController.StopDeath);
            _player.StartTrickDeath.AddListener(_animationController.StartTrickDeath);
            _player.ResetJumping.AddListener(delegate() {_animationController.ChangeFlag(PlayerStates.IsJump, false);});
            _player.ResetHit.AddListener(delegate() {_animationController.ChangeFlag(PlayerStates.Hit, false);});
            _player.Init();

            _movementController.SetPlayer(_player.gameObject);
            _movementController.Init();

            _animationController.SetPlayer(_playerObject);
            _animationController.Init();
        }

        public void Update()
        {
            _movementController.Update();
        }
    }
}