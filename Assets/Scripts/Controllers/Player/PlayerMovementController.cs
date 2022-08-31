namespace game.controllers.player
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Events;

    public class PlayerMovementController
    {
        private GameObject _playerObject;
        private Rigidbody2D _rigidbody;
        private float _startSpeed;
        private float _currentSpeed;
        private float _jumpForce = 250;
        private int _maxJump = 1;
        private int _currentValueJump = 0;
        private bool _isRunning = false;
        private bool _groundReached = false;

        public void Update()
        {
            TryRun();
            
            if (Input.GetKeyDown(KeyCode.Space) && _currentValueJump < _maxJump)
                {
                    if (_groundReached)
                    {
                        ChangeGroundFlag();
                    }

                    Jump();
                }
        }

        public void SetPlayer(GameObject player)
        {
            _playerObject = player;
        }

        public void SetStartSpeed(float speed)
        {
            _startSpeed = speed;
        }

        public void Init()
        {
            _currentSpeed = _startSpeed;
            _rigidbody = _playerObject.GetComponent<Rigidbody2D>();
        }

        public void StartRunning()
        {
            if (!_isRunning)
            _isRunning = true;
        }

        public void SetGroundReached()
        {
            _groundReached = true;
            _currentValueJump = 0;
        }

        private void TryRun()
        {
            if (_isRunning)
            _playerObject.transform.Translate(_startSpeed * Time.deltaTime, 0, 0);
        }

        private void Jump()
        {
            _rigidbody.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
            _currentValueJump ++;
        }

        private void ChangeGroundFlag()
        {
            _groundReached = !_groundReached;
        }
    }
}