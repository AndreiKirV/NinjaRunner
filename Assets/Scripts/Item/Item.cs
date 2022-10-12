namespace game.item
{

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using dictionaries;
    using game.controllers.player;

    public class Item : MonoBehaviour
    {
        protected Animator _animator;
        protected AudioSource _audioSource;
        protected float _demolitionDistance = 23;
        private bool isSigned = false;
        private void Awake() 
        {
            if (TryGetComponent<Animator>(out Animator animator))
            _animator = animator;

            if (TryGetComponent<AudioSource>(out AudioSource audioSource))
            _audioSource = audioSource;
            _audioSource.clip = Resources.Load<AudioClip>(Path.Sounds + ObjectNames.SoundCrash);

            GameMain.DestroyObject(gameObject, 10);
        }

        private void OnTriggerEnter2D(Collider2D other) 
        {
            TryAddListener(other);
            TryCrush(other);
        }

        protected virtual void TryAddListener(Collider2D other)
        {
            if (!isSigned && other.gameObject.TryGetComponent<Player>(out Player player))
            {
                player.TrickDone.AddListener(StartAnimatingSuccessfulTrick);
                player.StartedIdle.AddListener(StartAnimatingCrashed);
                player.DeathByObstacle.AddListener(StartAnimatingCrashed);
                isSigned = true;
            }
        }

        protected virtual void TryCrush(Collider2D other)
        {
            if (other.gameObject.name == ObjectNames.AttackBox && transform.position.x - other.gameObject.transform.position.x <= _demolitionDistance && transform.position.x - other.gameObject.transform.position.x >=0)
            {
                _audioSource.Play();
                StartAnimatingCrashed();
            }
        }

        protected virtual void StartAnimatingSuccessfulTrick()
        {
            if (_animator != null)
            _animator.SetTrigger("IsTrickSucceeds");
        }

        protected virtual void StartAnimatingCrashed()
        {
            _audioSource.Play();
            
            if (_animator != null)
            _animator.SetTrigger("IsCrashed");
        }
    }
}