namespace game.controllers.player
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Events;
    using dictionaries;

    public class Player : MonoBehaviour
    {
        private int _gold = 0;
        private int _lives = 1;
        private float _startSpeed = 75;
        private float _currentSpeed;
        private float _jumpForce = 250;
        private float _trickTeleportDistance = 21;
        private float _crashedDistance = 41;
        private int _maxJump = 1;
        private int _currentValueJump = 0;
        private List<string> _states = new List<string>();
        public float JumpForce => _jumpForce;
        public float StartSpeed => _startSpeed;
        public float CurrentSpeed => _currentSpeed;
        public int Gold => _gold;
        public int Lives => _lives;
        public UnityEvent StartedRunning = new UnityEvent();
        public UnityEvent ResetRunning = new UnityEvent();
        public UnityEvent StartedJumping = new UnityEvent();
        public UnityEvent TrickWorked = new UnityEvent();
        public UnityEvent Crashed = new UnityEvent();
        public UnityEvent TrickDone = new UnityEvent();
        public UnityEvent StartedIdle = new UnityEvent();
        public UnityEvent DeathByObstacle = new UnityEvent();

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
                TrySetState(PlayerStates.IsTrickZone);
            }

            if (other.gameObject.name == ObjectNames.StopZone && _lives > 0)
            {
                TryStartedCrashed();
            }
            else if (other.gameObject.name == ObjectNames.StopZone && _lives <= 0)
            {
                TryEventInvoke(DeathByObstacle);
                TrySetState(PlayerStates.DeathByObstacle);
                ResetRunningState();
                TryEventInvoke(ResetRunning);
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
            if (!CheckForState(PlayerStates.JumpObstacle))
            {
                transform.position = new Vector3(transform.position.x - _crashedDistance, transform.position.y, transform.position.z);
                ResetRunningState();
                TrySetState(PlayerStates.CrashedJump);
                TryEventInvoke(Crashed);
                _lives --;
            }
        }

        private void TryStartedTrick()
        {
            TryResetState(PlayerStates.IsTrickZone);

            if (CheckForState(PlayerStates.JumpObstacle))
            {
                transform.position = new Vector3(transform.position.x + _trickTeleportDistance, transform.position.y, transform.position.z);
                ResetRunningState();
                TryEventInvoke(TrickWorked);
            }
        }

        private void ResetJump()
        {
            TryResetState(PlayerStates.IsJump);
            _currentValueJump = 0;
        }

        private void ResetTrickWorked()
        {
            TryResetState(PlayerStates.JumpObstacle);
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
            TryResetState(PlayerStates.IsRun);
        }

        private void SetIdleState()
        {
            TryEventInvoke(ResetRunning);
            TryEventInvoke(StartedIdle);
            _states.Clear();
        }

        public void SetJumpingState()
        {
            if (_currentValueJump < _maxJump && !CheckForState(PlayerStates.IsTrickZone) && !CheckForState(PlayerStates.JumpObstacle) && !CheckForState(PlayerStates.CrashedJump) && !CheckForState(PlayerStates.DeathByObstacle))
            {
                TrySetState(PlayerStates.IsJump);
                TryEventInvoke(StartedJumping);
                _currentValueJump ++;
            }
            else if (CheckForState(PlayerStates.IsTrickZone))
            {
                TrySetState(PlayerStates.JumpObstacle);
            }
        }

        public void SetRunningState()
        {
            if (_states.Count < 1)
            {
                TrySetState(PlayerStates.IsRun);
                TryEventInvoke(StartedRunning);
            }
        }

        public void Init()
        {
            SetCurrentSpeed(_startSpeed);
        }

        public bool CheckForState(string currentState)
        {
            return _states.Contains(currentState);
        }

        public void ResetPlayer()
        {
            _states.Clear();
            TryEventInvoke(StartedIdle);
            _lives = 1;
        }
    }
}