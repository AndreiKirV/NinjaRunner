namespace game.controllers.player
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Events;
    using dictionaries;

    public class Player : MonoBehaviour
    {
        private int _lives = 2;
        private float _startSpeed = 75;
        private float _trickTeleportDistance = 21;
        private float _crashedDistance = 41;
        private float _currentSpeed;
        private float _jumpForce = 250;
        private int _maxJump = 1;
        private int _currentValueJump = 0;
        private List<string> _states = new List<string>();
        public float JumpForce => _jumpForce;
        public float StartSpeed => _startSpeed;
        public float CurrentSpeed => _currentSpeed;
        public UnityEvent StartedRunning = new UnityEvent();
        public UnityEvent ResetRunning = new UnityEvent();
        public UnityEvent StartedJumping = new UnityEvent();
        public UnityEvent TrickWorked = new UnityEvent();
        public UnityEvent Crashed = new UnityEvent();
        public UnityEvent TrickDone = new UnityEvent();

        private void OnCollisionEnter2D (Collision2D other) 
        {
            if (other.gameObject.name == ObjectNames.Ground)
            {
                ResetJump();
            }
        }

        private void OnTriggerEnter2D(Collider2D other) 
        {
            if (other.gameObject.name == ObjectNames.TrickZone)
            {
                TrySetState(PlayerStates.IS_TRICK_ZONE);
            }

            if (other.gameObject.name == ObjectNames.StopZone)
            {
                TryStartedCrashed();
            }
        }

        private void OnTriggerExit2D(Collider2D other) 
        {
            if (other.gameObject.name == ObjectNames.TrickZone)
            {
                TryStartedTrick();
            }
        }

        private void TryStartedCrashed()
        {
            if (!CheckForState(PlayerStates.IS_TRICK_WORKED) && _lives > 0)
                {
                    transform.position = new Vector3(transform.position.x - _crashedDistance, transform.position.y, transform.position.z);
                    ResetRunningState();
                    TrySetState(PlayerStates.IS_STOP_ZONE);
                    TryEventInvoke(Crashed);
                    _lives --;
                }
        }

        private void TryStartedTrick()
        {
            TryResetState(PlayerStates.IS_TRICK_ZONE);

            if (CheckForState(PlayerStates.IS_TRICK_WORKED))
            {
                transform.position = new Vector3(transform.position.x + _trickTeleportDistance, transform.position.y, transform.position.z);
                ResetRunningState();
                TryEventInvoke(TrickWorked);
            }
        }

        private void ResetJump()
        {
            TryResetState(PlayerStates.IS_JUMPING);
            _currentValueJump = 0;
        }

        private void ResetTrickWorked()
        {
            TryResetState(PlayerStates.IS_TRICK_WORKED);
            SetRunningState();
            TryEventInvoke(TrickDone);
        }

        private void SetCurrentSpeed(float speed)
        {
            _currentSpeed = speed;
        }

        private void TrySetState(string currentState)
        {
            if (!CheckForState(currentState))
            _states.Add(currentState);
        }

        private void TryResetState(string currentState)
        {
            if (CheckForState(currentState))
            _states.Remove(currentState);
        }

        private void TryEventInvoke(UnityEvent targetEvent)
        {
            if (targetEvent != null)
            targetEvent.Invoke();
        }

        private void ResetRunningState()
        {
            TryResetState(PlayerStates.IS_RUNNING);
        }

        public void SetJumpingState()
        {
            if (_currentValueJump < _maxJump && !CheckForState(PlayerStates.IS_TRICK_ZONE) && !CheckForState(PlayerStates.IS_TRICK_WORKED) && !CheckForState(PlayerStates.IS_STOP_ZONE))
            {
                TrySetState(PlayerStates.IS_JUMPING);
                TryEventInvoke(StartedJumping);
                _currentValueJump ++;
            }
            else if (CheckForState(PlayerStates.IS_TRICK_ZONE))
            {
                TrySetState(PlayerStates.IS_TRICK_WORKED);
            }
        }

        public void SetRunningState()
        {
            TrySetState(PlayerStates.IS_RUNNING);
            TryEventInvoke(StartedRunning);
        }

        public void Init()
        {
            SetCurrentSpeed(_startSpeed);
        }

        private void SetIdleState()
        {
            TryEventInvoke(ResetRunning);
            _states.Clear();
        }

        public bool CheckForState(string currentState)
        {
            return _states.Contains(currentState);
        }
    }
}