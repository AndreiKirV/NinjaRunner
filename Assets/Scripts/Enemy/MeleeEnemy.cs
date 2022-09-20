namespace game.enemy
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using game.controllers.enemy;
    using dictionaries;
    using game.controllers.player;
    using game.controllers;

    public class MeleeEnemy : Enemy
    {
        [SerializeField] private List<SpriteRenderer> _bones = new List<SpriteRenderer>();
        [SerializeField] private GameObject _bloodEffect;
        private float _timePreviousHit;
        private float _coolDownHit = 1;
        private float _demolitionDistance = 32;
        private float _timeDestroy = 2.5f;
        private bool _isRipped = false;
        private Vector2 _forceBodyPart = new Vector2(2500,3700);
        private EnemyViewController _view = new EnemyViewController();
        private EnemyAnimationController _ainmator;
        private Player _player;

        private void Awake() 
        {
            _view.SetBones(_bones);
            _view.InitMeleeEnemy();
            _view.SetSprites();
            _ainmator = new EnemyAnimationController(gameObject);
            _timePreviousHit = -_coolDownHit;
        }

        private void OnTriggerEnter2D(Collider2D other) 
        {            
            if (other.gameObject.name == ObjectNames.AttackBox)
                {
                    TryDeath(other);
                }
            else if (other.gameObject.name == ObjectNames.Character && !_isRipped)
                {
                    _player = other.gameObject.GetComponent<Player>();
                    TryHit();  
                }
        }

        private void TryHit()
        {
            if (_player.Lives >= 1 && ControllerManager.Timer >= _coolDownHit + _timePreviousHit)
                {
                    _ainmator.StartHit();
                    _timePreviousHit = ControllerManager.Timer;
                }
        }

        private void TryDeath(Collider2D other)
        {
            if (!_isRipped && transform.position.x - other.gameObject.transform.position.x <= _demolitionDistance && transform.position.x - other.gameObject.transform.position.x >= 0)
            {
                _ainmator.StartDeath();

                GameObject tempObject = _view.GiveBodyPart();
                _bloodEffect.SetActive(true);
                _bloodEffect.transform.position = tempObject.transform.position;
                _bloodEffect.transform.SetParent(tempObject.transform);
                //tempObject.transform.position = new Vector3(gameObject.transform.position.x ,gameObject.transform.position.y + 12, 0);
                tempObject.transform.parent = null;
                tempObject.GetComponent<PolygonCollider2D>().enabled = true;
                Rigidbody2D tempRigidbody = tempObject.GetComponent<Rigidbody2D>();
                tempRigidbody.simulated = true;
                tempRigidbody.AddForce(_forceBodyPart);
                Destroy(tempObject, _timeDestroy);
                Destroy(gameObject, _timeDestroy);
                _isRipped = true;
                
            }
        }
    }
}