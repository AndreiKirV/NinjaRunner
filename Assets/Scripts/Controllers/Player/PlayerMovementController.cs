namespace game.controllers.player
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Events;
    using dictionaries;

    public class PlayerMovementController
    {
        private GameObject _playerObject;
        private Player _player;
        private Rigidbody2D _rigidbody;

        public void Update()
        {
            TryRun();
        }

        public void SetPlayer(GameObject player)
        {
            _playerObject = player;
        }

        public void Init()
        {
            _player = _playerObject.GetComponent<Player>();
            _rigidbody = _playerObject.GetComponent<Rigidbody2D>();
        }

        private void TryRun()
        {
            if (_player.CheckForState(PlayerStates.IsRun))
            _playerObject.transform.Translate(_player.CurrentSpeed * Time.deltaTime, 0, 0);
        }

        public void Jump()
        {
            _rigidbody.AddForce(Vector2.up * _player.JumpForce, ForceMode2D.Impulse);
        }
    }
}