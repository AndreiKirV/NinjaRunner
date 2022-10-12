namespace game.enemy
{
    using System.Collections.Generic;
    using UnityEngine;
    using game.controllers.enemy;
    using dictionaries;
    using game.controllers.player;
    using game.controllers;
    using UnityEngine.Events;

    public class EnemyRanged : MonoBehaviour
    {
        [SerializeField] private List<SpriteRenderer> _bones = new List<SpriteRenderer>();
        [SerializeField] private GameObject _bullet; 
        [SerializeField] private GameObject [] _bulletPosition = new GameObject [2];
        private int _maxBullets = 2;
        private float _timeDestroy = 3f;
        private float _coolDownHit = 1.7f;
        private float _timePreviousHit;
        private bool _isRipped = false;
        private List <GameObject> _bullets = new List<GameObject>();
        private EnemyAnimationController _ainmator;
        private EnemyViewController _view = new EnemyViewController();
        public UnityEvent Dead = new UnityEvent();

        private void OnTriggerEnter2D(Collider2D other) 
        {   
            if (other.gameObject.name == ObjectNames.Character && !_isRipped)
            {  
                Escape();
                Destroy(gameObject, _timeDestroy);
                _isRipped = true;
            }
        }

        private void Awake() 
        {
            _view.SetBones(_bones);
            _view.InitRangedEnemy();
            _view.SetSprites();
            _bullet.GetComponent<SpriteRenderer>().sprite = _view.GiveBullet().sprite;
            _ainmator = new EnemyAnimationController(gameObject);
            _timePreviousHit = -_coolDownHit;
        }

        private void Update() 
        {
            if (ControllerManager.Timer >= _coolDownHit + _timePreviousHit && _bullets.Count < _maxBullets)
            {
                _timePreviousHit = ControllerManager.Timer;
                _ainmator.StartHit();
            }
        }

        private void Hit()
        {
            GameObject tempObject = Instantiate(_bullet, _bulletPosition[Random.Range(0, _bulletPosition.Length)].transform.position, Quaternion.identity);
            _bullets.Add(tempObject);
            tempObject.name = ObjectNames.Bullet;
            tempObject.GetComponent<Bullet>().Destroyed.AddListener(DeleteBullet);
        }

        private void Escape()
        {
            _ainmator.StartEscape();
        }

        private void DeleteBullet(GameObject bullet)
        {
            _bullets.Remove(bullet);
        }

        private void OnDestroy() 
        {
            if (_bullets.Count > 0)
            {
                foreach (var item in _bullets)
                {
                    Destroy(item);
                }
            }
        }

        public void Death()
        {
            GetComponent<AudioSource>().Play();
            Dead.Invoke();
            _ainmator.StartDeath();
            Destroy(gameObject, _timeDestroy);
        }
    }
}