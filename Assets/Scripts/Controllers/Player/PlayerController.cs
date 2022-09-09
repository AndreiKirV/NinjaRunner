namespace game.controllers.player
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

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

        public void SetButtonRun(Button button)
        {
            button.onClick.AddListener(_player.SetRunningState);
        }

        public void SetButtonJump(Button button)
        {
            button.onClick.AddListener(_player.SetJumpingState);
        }

        public void Init() 
        {
            _player.Init();
            _player.StartedRunning.AddListener(_animationController.Run);
            _player.TrickWorked.AddListener(_animationController.JumpObstacle);
            _player.StartedJumping.AddListener(_animationController.Jump);
            _player.StartedJumping.AddListener(_movementController.Jump);
            _player.Crashed.AddListener(_animationController.CrashedJump);
            _player.ResetRunning.AddListener(_animationController.StopRun);

            _movementController.SetPlayer(_player.gameObject);
            _movementController.Init();

            _animationController.SetPlayer(_playerObject);
        }

        public void Update()
        {
            _movementController.Update();
        }
    }
}