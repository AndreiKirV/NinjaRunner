namespace game.controllers.player
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Events;

    public class Player : MonoBehaviour
    {
        private float _speed = 75;
        public float Speed => _speed;
        public UnityAction GroundReached;

        private void OnCollisionEnter2D (Collision2D other) 
        {
            if (other.gameObject.name == "Ground(Clone)")
            {
                GroundReached();
            }
        }
    }
}