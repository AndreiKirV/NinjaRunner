namespace game.item
{

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using dictionaries;
    using game.controllers.player;

    public class Item : MonoBehaviour
    {
        Animator _animator;
        private void Awake() 
        {
            if (TryGetComponent<Animator>(out Animator animator))
            _animator = animator;
        }

        private void OnTriggerEnter2D(Collider2D other) 
        {
            if (other.gameObject.TryGetComponent<Player>(out Player player))
            {
                player.TrickDone.AddListener(StartAnimatingSuccessfulTrick);
            }
        }

        private void StartAnimatingSuccessfulTrick()
        {
            _animator.SetTrigger("IsTrickSucceeds");
        }
    }
}