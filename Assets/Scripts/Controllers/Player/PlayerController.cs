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
        PlayerMovementController _playerMovementController = new PlayerMovementController();
        PlayerAnimationController _playerAnimationController = new PlayerAnimationController();

        public void SetPlayer(GameObject player)
        {
            _playerObject = player;
        }

        public void SetButtonRun(Button button)
        {
            button.onClick.AddListener(_playerMovementController.StartRunning);
        }

        public void Init() 
        {
            _playerObject.AddComponent<Player>();
            _player = _playerObject.GetComponent<Player>();
            _player.GroundReached += _playerMovementController.SetGroundReached;

            _playerMovementController.SetPlayer(_player.gameObject);
            _playerMovementController.SetStartSpeed(_player.Speed);
            _playerMovementController.Init();

            _playerAnimationController.SetPlayer(_playerObject);
        }

        public void Update()
        {
            _playerMovementController.Update();
        }
    }
}