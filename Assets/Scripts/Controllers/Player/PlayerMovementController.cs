namespace game.controllers.player
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Events;
    using dictionaries;
    using UnityEngine.UI;

    public class PlayerMovementController
    {
        private GameObject _playerObject;
        private Player _player;
        private Rigidbody2D _rigidbody;
        private Vector3 _touch;
        private Camera _camera;
        private Button _buttonRun;
        public UnityEvent TouchJump = new UnityEvent();
        public UnityEvent TouchSlide = new UnityEvent();
        public UnityEvent TouchHit = new UnityEvent();

        private void TryTouchMove() 
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (UIController.PanelRequest(ObjectNames.Panel) == false && (_camera.ScreenToViewportPoint(Input.mousePosition).x <= 0.8f || _camera.ScreenToViewportPoint(Input.mousePosition).y >= 0.35f))
                {
                    _touch = _camera.ScreenToViewportPoint(Input.mousePosition);
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                Vector3 touchEnd = _camera.ScreenToViewportPoint(Input.mousePosition);
                
                if (touchEnd.y > _touch.y)
                {
                    TouchJump.Invoke();
                }
                else if (touchEnd.y < _touch.y)
                {
                    TouchSlide.Invoke();
                }
                else
                {
                    TouchHit.Invoke();
                }
            }
        }

        public void SetCamera(Camera camera)
        {
            _camera = camera;
        }

        public void Update()
        {
            TryRun();
            TryTouchMove();
        }

        public void SetPlayer(GameObject player)
        {
            _playerObject = player;
        }

        public void Init()
        {
            _player = _playerObject.GetComponent<Player>();
            _rigidbody = _playerObject.GetComponent<Rigidbody2D>();

            TouchHit.RemoveAllListeners();
            TouchSlide.RemoveAllListeners();
            TouchHit.RemoveAllListeners();

            TouchSlide.AddListener(_player.TrySetSlideState);
            TouchJump.AddListener(_player.SetJumpingState);

            TouchHit.AddListener(delegate {
                if (_player.CheckForState(PlayerStates.IsRun))
                _player.SetHitState();
                });
                
            TouchHit.AddListener(_player.SetRunningState);
        }

        private void TryRun()
        {
            if (_player.CheckForState(PlayerStates.IsRun))
            _playerObject.transform.position = Vector2.Lerp(_playerObject.transform.position, new Vector2(_playerObject.transform.position.x + _player.CurrentSpeed,_playerObject.transform.position.y), Time.deltaTime);
        }

        public void Jump()
        {
            _rigidbody.AddForce(Vector2.up * _player.JumpForce, ForceMode2D.Impulse);
        }
    }
}