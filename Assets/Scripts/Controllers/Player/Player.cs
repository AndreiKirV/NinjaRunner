namespace game.controllers.player
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Events;
    using dictionaries;
    using game.item;
    using game.controllers.shop;

    public class Player : MonoBehaviour
    {
        private int _gold = 0;
        private int _lives = 2;
        private int _trickValue = 0;
        private int _fragValue = 0;
        private int _fragPrice = 2;
        private int _trickPrice = 1;
        private int _maxJump = 1;
        private int _currentValueJump = 0;
        private int _stepSpeed = 1;
        private int _startSpeed = 75;
        private int _currentSpeed;
        private int _maxSpeed = 135;
        private float _jumpForce = 250;
        private float _trickTeleportDistance = 21;
        private float _crashedDistance = 41;
        private float _timePreviousJump = 0;
        private float _timePreviousHit = 0;
        private float _timePreviousSlide = 0;
        private float _coolDownJump = 1.5f;
        private float _coolDownHit = 3;
        private float _coolDownSlide = 2.5f;
        private float _slideTime = 2;
        private float _timePreviousSpeedIncrease = 0;
        private float _stepSpeedIncrease = 5;
        private GameObject _trickEffect;
        private GameObject _attackBox;
        private ParticleSystem _fatigueEffect;
        private SpriteRenderer _weapon = new SpriteRenderer();
        private AudioSource _audioSource;

        private Dictionary<string, AudioClip> _sounds = new Dictionary<string, AudioClip>();

        private List<string> _states = new List<string>();

        public float JumpForce => _jumpForce;
        public int StartSpeed => _startSpeed;
        public int CurrentSpeed => _currentSpeed;
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
        public UnityEvent <int> StartTrickDeath = new UnityEvent <int> ();

        public static UnityEvent Death= new UnityEvent();
        public static UnityEvent ResPlayer;
        public static UnityEvent<int> ValueTrickChanged = new UnityEvent<int>();
        public static UnityEvent<int> ValueLivesChanged = new UnityEvent<int>();
        public static UnityEvent<int> ValueGoldChanged = new UnityEvent<int>();
        public static UnityEvent<int> ValueFragChanged = new UnityEvent<int>();
        public static UnityEvent<int> ValueCurrentSpeedChanged = new UnityEvent<int>();
        public static UnityEvent CalculateGold = new UnityEvent();
        public static UnityEvent<int, int> ValueRecordsChanged = new UnityEvent<int, int>();

        private void Awake() 
        {
            ResPlayer = new UnityEvent();
            ResPlayer.AddListener(ResetPlayer);
            _weapon = gameObject.transform.Find(ObjectNames.Body).transform.Find(ObjectNames.Sword).gameObject.GetComponent<SpriteRenderer>();
            ShopController.WeaponsPurchased.AddListener(ChangeWeapon);
            CalculateGold.AddListener(CalculateTranslationIntoGold);

                if (PlayerPrefs.HasKey(ObjectNames.Weapon))
            _weapon.sprite = Resources.Load<Sprite>($"{Path.WEAPON}{PlayerPrefs.GetString(ObjectNames.Weapon)}");

            AudioSourceInit();
        }

        private void Update() 
        {
            if (CheckForState(PlayerStates.Slide) && _timePreviousSlide + _slideTime <= ControllerManager.Timer)
            {
                ResetSlideState();
            }

            if (CheckForState(PlayerStates.IsRun) && _currentSpeed < _maxSpeed && ControllerManager.Timer >= _stepSpeedIncrease + _timePreviousSpeedIncrease)
            {
                _currentSpeed += _stepSpeed;
                TryEventInvoke(ValueCurrentSpeedChanged, _currentSpeed);
                _timePreviousSpeedIncrease = ControllerManager.Timer;
            }
        }

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

            if ((other.gameObject.name == ObjectNames.StopZone || other.gameObject.name == ObjectNames.AttackBox) && !_attackBox.activeSelf && !CheckForState(PlayerStates.JumpObstacle))
            {
                TryStartedClash();
            }

            if (!CheckForState(PlayerStates.Hit) && other.gameObject.name == ObjectNames.TrickZone && other.gameObject.transform.parent.TryGetComponent<Trunk>(out Trunk trunk))
            {
                trunk.AddListenerEvent(AddGold);
            }
            else if (!CheckForState(PlayerStates.Hit) && other.gameObject.name == ObjectNames.TrickZone && other.gameObject.transform.parent.TryGetComponent<HealingChest>(out HealingChest healingChest))
            {
                healingChest.AddListenerEvent(AddLives);
            }
        }

        private void AudioSourceInit()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private void AudioClipInit()
        {
            _sounds.Add(ObjectNames.SoundJump, Resources.Load<AudioClip>(Path.Sounds + ObjectNames.SoundJump));
            _sounds.Add(ObjectNames.SoundDeath, Resources.Load<AudioClip>(Path.Sounds + ObjectNames.SoundDeath));
            _sounds.Add(ObjectNames.SoundHit, Resources.Load<AudioClip>(Path.Sounds + ObjectNames.SoundHit));
            _sounds.Add(ObjectNames.SoundSlide, Resources.Load<AudioClip>(Path.Sounds + ObjectNames.SoundSlide));
            _sounds.Add(ObjectNames.SoundSwing, Resources.Load<AudioClip>(Path.Sounds + ObjectNames.SoundSwing));
            _sounds.Add(ObjectNames.SoundTrick, Resources.Load<AudioClip>(Path.Sounds + ObjectNames.SoundTrick));
        }

        private void PlaySoundHit()
        {
            PlaySound(ObjectNames.SoundHit);
        }

        private void PlaySound(string targetAudio)
        {
            _audioSource.Stop();
            _audioSource.clip = _sounds[targetAudio];
            _audioSource.Play();
        }

        private void CalculateTranslationIntoGold()
        {
            _gold += _fragValue * _fragPrice + _trickPrice * _trickValue;
            _fragValue = 0;
            _trickValue = 0;
            TryEventInvoke(ValueTrickChanged, _trickValue);
            TryEventInvoke(ValueGoldChanged, _gold);
            TryEventInvoke(ValueFragChanged, _fragValue);
            PlayerPrefs.SetInt(ObjectNames.Gold, _gold);
        }

        private void ChangeWeapon(Sprite sprite)
        {
            _weapon.sprite = sprite;
            PlayerPrefs.SetString(ObjectNames.Weapon, sprite.name);
        }

        private void TryDeath()
        {
            Death.Invoke();
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
                PlaySound(ObjectNames.SoundDeath);
                ValueRecordsChanged.Invoke(_trickValue, _fragValue);
                TryEventInvoke(DeathByObstacle);
                TrySetState(PlayerStates.DeathByObstacle);
                ResetRunningState();
                TryEventInvoke(ResetRunning);
                TakeLivesValue();
                SlowDown();
                _timePreviousHit = 0;
            }
        }

        private void TryStartedCrashed()
        {
            if (!CheckForState(PlayerStates.JumpObstacle) && !CheckForState(PlayerStates.IsTrickZone))
            {
                transform.position = new Vector3(transform.position.x - _crashedDistance, transform.position.y, transform.position.z);
                TryResetState(PlayerStates.IsStopZone);
                TrySetState(PlayerStates.CrashedJump);
                TakeLivesValue();
                TryEventInvoke(Crashed);
                SlowDown();
                _timePreviousHit = 0;
            }
        }

        private void FinishedCrashed()
        {
            TryResetState(PlayerStates.CrashedJump);
        }

        private void SlowDown()
        {
            _currentSpeed = _startSpeed;
            _timePreviousSpeedIncrease = ControllerManager.Timer;
            TryEventInvoke(ValueCurrentSpeedChanged, _currentSpeed);
        }

        private void TakeLivesValue()
        {
            if (_lives > 0)
            {
                _lives --;
                TryEventInvoke(ValueLivesChanged, _lives); 
            }
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
            ResetHitState();
            SetRunningState();
            AddTrickValue();
        }

        private void AddTrickValue()
        {
            TryEventInvoke(TrickDone);
            _trickValue ++;
            TryEventInvoke(ValueTrickChanged, _trickValue);
        }

        private void SetCurrentSpeed(int speed)
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

        private void TryEventInvoke(UnityEvent<int> targetEvent, int targetValue)
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
            ResetSlideState();
            ResetJumpState();
            _states.Clear();
        }

        private void AddLives ()
        {
            _lives ++;
            TryEventInvoke(ValueLivesChanged, _lives);
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

        public void AddFragValue()
        {
            _fragValue ++;
            TryEventInvoke(ValueFragChanged, _fragValue);
        }

        public void TrySetSlideState()
        {
            if (CheckForState(PlayerStates.IsRun) && ControllerManager.Timer >= _coolDownSlide + _timePreviousSlide)
            {
                PlaySound(ObjectNames.SoundSlide);
                TrySetState(PlayerStates.Slide);
                TryEventInvoke(StartedSlide);
                _timePreviousSlide = ControllerManager.Timer;
            }
            else if (ControllerManager.Timer < _coolDownSlide + _timePreviousSlide)
            {
                _fatigueEffect.Play();
            }
        }

        private void ResetSlideState()
        {
            TryResetState(PlayerStates.Slide);
            TryEventInvoke(ResetSlide);
        }

        private int GiveLivesValue()
        {
            return _lives;
        }

        public void SetJumpingState()
        {
            if (ControllerManager.Timer >= _coolDownJump + _timePreviousJump && _currentValueJump < _maxJump && !CheckForState(PlayerStates.IsTrickZone) && !CheckForState(PlayerStates.JumpObstacle) && !CheckForState(PlayerStates.CrashedJump) && !CheckForState(PlayerStates.DeathByObstacle) && _states.Count <= 1)
            {
                TrySetState(PlayerStates.IsJump);
                TryEventInvoke(StartedJumping);
                _currentValueJump ++;
                _timePreviousJump = ControllerManager.Timer;
                PlaySound(ObjectNames.SoundJump);
            }
            else if (ControllerManager.Timer >= _coolDownJump + _timePreviousJump && CheckForState(PlayerStates.IsTrickZone) && !CheckForState(PlayerStates.IsStopZone) && !CheckForState(PlayerStates.Slide))
            {
                PlaySound(ObjectNames.SoundTrick);
                TrySetState(PlayerStates.JumpObstacle);
            }
            else if (ControllerManager.Timer <= _coolDownJump + _timePreviousJump)
            {
                _fatigueEffect.Play();
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
            if (_states.Count <= 1 && CheckForState(PlayerStates.IsRun) && ControllerManager.Timer >= _coolDownHit + _timePreviousHit)
            {
                PlaySound(ObjectNames.SoundSwing);
                TrySetState(PlayerStates.Hit);
                TryEventInvoke(StartedHit);
                _timePreviousHit = ControllerManager.Timer;
            }
            else if (ControllerManager.Timer < _coolDownHit + _timePreviousHit)
            {
                _fatigueEffect.Play();
            }
        }

        public void Init()
        {
            if (PlayerPrefs.HasKey(ObjectNames.Gold))
                _gold = PlayerPrefs.GetInt(ObjectNames.Gold);

            _attackBox = gameObject.transform.Find(ObjectNames.AttackBox).gameObject;

            SetCurrentSpeed(_startSpeed);
            TryEventInvoke(ValueTrickChanged, _trickValue);
            TryEventInvoke(ValueLivesChanged, _lives);
            TryEventInvoke(ValueGoldChanged, _gold);
            TryEventInvoke(ValueFragChanged, _fragValue);
            TryEventInvoke(ValueCurrentSpeedChanged, _currentSpeed);

            _trickEffect = CreateObject($"{Path.PREFABS_EFFECTS}{ObjectNames.TrickZoneEffect}");
            _trickEffect.SetActive(false);
            _fatigueEffect = CreateObject($"{Path.PREFABS_EFFECTS}{ObjectNames.LightningEffect}").GetComponent<ParticleSystem>();
            _trickEffect.SetActive(false);

            _timePreviousJump = -_coolDownJump;
            _timePreviousHit = -_coolDownHit;
            _timePreviousSlide = -_coolDownSlide;

            UIController.LivesRequest += GiveLivesValue;

            AudioClipInit();

            Resources.UnloadUnusedAssets();
        }

        public bool CheckForState(string currentState)
        {
            return _states.Contains(currentState);
        }

        public void ResetPlayer()
        {
            SetIdleState();
            TryEventInvoke(ResetRunning);
            _lives += 1;
            TryEventInvoke(ValueLivesChanged, _lives);
        }

        public void StartedTrickDeath(int value)
        {
            ResetRunningState();
            TryEventInvoke(StartTrickDeath, value);
        }

        public void TryStartedClash()
        {
            ResetRunningState();
            ResetHitState();
            TrySetState(PlayerStates.IsStopZone);

            if (_lives <= 1)
            {
                TryStartedDeath();
            }
            else if (_lives > 1)
            {
                PlaySoundHit();
                TryStartedCrashed();
            }
        }

        public void TakeGold(int value)
        {
            _gold -= value;
            TryEventInvoke(ValueGoldChanged, _gold);
            PlayerPrefs.SetInt(ObjectNames.Gold, _gold);
        }

        public static void DeathInit()
        {
            Death = new UnityEvent();
        }
    }
}