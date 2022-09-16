namespace game.controllers.player
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Events;
    using dictionaries;
    using game.item;

    public class Player : MonoBehaviour
    {
        private int _gold = 0;
        private int _lives = 99;
        private int _trickValue = 0;
        private int _fragValue = 0;
        private int _maxJump = 1;
        private int _currentValueJump = 0;
        private float _startSpeed = 75;
        private float _currentSpeed;
        private float _jumpForce = 250;
        private float _trickTeleportDistance = 21;
        private float _crashedDistance = 41;
        private GameObject _trickEffect;
        private GameObject _attackBox;

        private List<string> _states = new List<string>();

        public float JumpForce => _jumpForce;
        public float StartSpeed => _startSpeed;
        public float CurrentSpeed => _currentSpeed;
        public int Gold => _gold;
        public int Lives => _lives;

        public UnityEvent StartedRunning = new UnityEvent();
        public UnityEvent StartedJumping = new UnityEvent();
        public UnityEvent StartedHit = new UnityEvent();
        public UnityEvent StartedSlide = new UnityEvent();
        public UnityEvent StartedIdle = new UnityEvent();
        public UnityEvent ResetRunning = new UnityEvent();
        public UnityEvent ResetSlide = new UnityEvent();
        public UnityEvent ResetJumping = new UnityEvent();
        public UnityEvent ResetHit = new UnityEvent();
        public UnityEvent TrickWorked = new UnityEvent();
        public UnityEvent Crashed = new UnityEvent();
        public UnityEvent TrickDone = new UnityEvent();
        public UnityEvent DeathByObstacle = new UnityEvent();

        public static UnityEvent<int> ValueTrickChanged = new UnityEvent<int>();
        public static UnityEvent<int> ValueLivesChanged = new UnityEvent<int>();
        public static UnityEvent<int> ValueGoldChanged = new UnityEvent<int>();
        public static UnityEvent<int> ValueFragChanged = new UnityEvent<int>();
        public static UnityEvent<float> ValueCurrentSpeedChanged = new UnityEvent<float>();

        private void OnCollisionEnter2D (Collision2D other) 
        {
            ResetJumpState();
        }

        private void OnTriggerEnter2D(Collider2D other) 
        {
            if (other.gameObject.name == ObjectNames.TrickZone)
            {
                TrySetState(PlayerStates.IsTrickZone);
                _trickEffect.SetActive(true);
            }

            if (other.gameObject.name == ObjectNames.StopZone && !_attackBox.activeSelf)
            {
                ResetSlideState();
                ResetJumpState();
                ResetHitState();
                TrySetState(PlayerStates.IsStopZone);

                if (_lives <= 1)
                {
                    TryStartedDeath();
                }
                else if (_lives > 1)
                {
                    TryStartedCrashed();
                }
            }

            if (!CheckForState(PlayerStates.Hit) && other.gameObject.name != ObjectNames.Ground && other.gameObject.transform.parent.TryGetComponent<Trunk>(out Trunk trunk))
            {
                trunk.AddListenerEvent(AddGold);
            }
        }

        private void OnTriggerExit2D(Collider2D other) 
        {
            if (other.gameObject.name == ObjectNames.TrickZone)
            {
                TryStartedTrick();
            }

            if (other.gameObject.name == ObjectNames.StopZone)
            {
                TryResetState(PlayerStates.IsStopZone);
            }

            if (other.gameObject.name == ObjectNames.EndZone)
            {
                ResetSlideState();
            }
        }

        private void TryStartedDeath()
        {
            if (!CheckForState(PlayerStates.JumpObstacle) && !CheckForState(PlayerStates.IsTrickZone))
            {
                TryEventInvoke(DeathByObstacle);
                TrySetState(PlayerStates.DeathByObstacle);
                ResetRunningState();
                TryEventInvoke(ResetRunning);
                TakeLivesValue();
            }
        }

        private void TryStartedCrashed()
        {
            if (!CheckForState(PlayerStates.JumpObstacle) && !CheckForState(PlayerStates.IsTrickZone))
            {
                transform.position = new Vector3(transform.position.x - _crashedDistance, transform.position.y, transform.position.z);
                ResetRunningState();
                ResetJumpState();
                TryResetState(PlayerStates.IsStopZone);
                TrySetState(PlayerStates.CrashedJump);
                TakeLivesValue();
                TryEventInvoke(Crashed);
            }
        }

        private void TakeLivesValue()
        {
            _lives --;
            TryEventInvoke(ValueLivesChanged, _lives);
        }

        private void TryStartedTrick()
        {
            TryResetState(PlayerStates.IsTrickZone);
            _trickEffect.SetActive(false);

            if (CheckForState(PlayerStates.JumpObstacle) && !CheckForState(PlayerStates.IsStopZone))
            {
                ResetRunningState();
                TryEventInvoke(TrickWorked);
                transform.position = new Vector3(transform.position.x + _trickTeleportDistance, transform.position.y, transform.position.z);
            }
        }

        private void ResetJumpState()
        {
            TryResetState(PlayerStates.IsJump);
            TryEventInvoke(ResetJumping);
            _currentValueJump = 0;
        }

        private void ResetTrickWorked()
        {
            TryResetState(PlayerStates.JumpObstacle);
            SetRunningState();
            AddTrickValue();
        }

        private void AddTrickValue()
        {
            TryEventInvoke(TrickDone);
            _trickValue ++;
            TryEventInvoke(ValueTrickChanged, _trickValue);
        }

        private void SetCurrentSpeed(float speed)
        {
            _currentSpeed = speed;
        }
        int i = 0;
        private void TrySetState(string currentState)
        {
            if (!CheckForState(currentState))
            _states.Add(currentState);
            i++;
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

        private void TryEventInvoke(UnityEvent<int> targetEvent, int targetValue)
        {
            if (targetEvent != null)
            targetEvent.Invoke(targetValue);
        }

        private void TryEventInvoke(UnityEvent<float> targetEvent, float targetValue)
        {
            if (targetEvent != null)
            targetEvent.Invoke(targetValue);
        }

        private void ResetRunningState()
        {
            TryResetState(PlayerStates.IsRun);
        }

        private void ResetHitState()
        {
            TryResetState(PlayerStates.Hit);
            TryEventInvoke(ResetHit);
        }

        private void SetIdleState()
        {
            TryEventInvoke(ResetRunning);
            TryEventInvoke(StartedIdle);
            _states.Clear();
        }

        private void AddGold(int value)
        {
            _gold += value;
            TryEventInvoke(ValueGoldChanged, _gold);
        }

        private GameObject CreateObject(string path)
        {
            GameObject tempObject = Resources.Load<GameObject>(path);
            GameObject targetObject = GameMain.InstantiateObject(tempObject, gameObject.transform);
            targetObject.name = tempObject.name;
            return targetObject;
        }

        public void TrySetSlideState()
        {
            if (CheckForState(PlayerStates.IsRun))
            {
                TrySetState(PlayerStates.Slide);
                TryEventInvoke(StartedSlide);
            }
        }

        private void ResetSlideState()
        {
            TryResetState(PlayerStates.Slide);
            TryEventInvoke(ResetSlide);
        }

        public void SetJumpingState()
        {
            if (_currentValueJump < _maxJump && !CheckForState(PlayerStates.IsTrickZone) && !CheckForState(PlayerStates.JumpObstacle) && !CheckForState(PlayerStates.CrashedJump) && !CheckForState(PlayerStates.DeathByObstacle) && _states.Count <= 1)
            {
                TrySetState(PlayerStates.IsJump);
                TryEventInvoke(StartedJumping);
                _currentValueJump ++;
            }
            else if (CheckForState(PlayerStates.IsTrickZone) && !CheckForState(PlayerStates.IsStopZone))
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

        public void SetHitState()
        {
            if (_states.Count <= 1 && CheckForState(PlayerStates.IsRun))
            {
                TrySetState(PlayerStates.Hit);
                TryEventInvoke(StartedHit);
            }
        }

        public void Init()
        {
            _attackBox = gameObject.transform.Find(ObjectNames.AttackBox).gameObject;

            SetCurrentSpeed(_startSpeed);
            TryEventInvoke(ValueTrickChanged, _trickValue);
            TryEventInvoke(ValueLivesChanged, _lives);
            TryEventInvoke(ValueGoldChanged, _gold);
            TryEventInvoke(ValueCurrentSpeedChanged, _currentSpeed);
            TryEventInvoke(ValueFragChanged, _fragValue);

            _trickEffect = CreateObject($"{Path.PREFABS_EFFECTS}TrickZoneEffect");
            _trickEffect.SetActive(false);
            Resources.UnloadUnusedAssets();
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
            TryEventInvoke(ValueLivesChanged, _lives);
        }
    }
}