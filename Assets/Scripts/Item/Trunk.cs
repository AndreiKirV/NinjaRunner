namespace game.item
{

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using dictionaries;
    using game.controllers.player;
    using UnityEngine.Events;

    public class Trunk : Item
    {
        public UnityEvent <int> SuccessfullyOvercome = new UnityEvent<int>();
        private UnityAction<int> [] _events = new UnityAction<int>[1];
        private bool isAwardReceived = false;
        
        private int _minPrice = 2;
        private int _maxPrice = 10;

        protected override void StartAnimatingSuccessfulTrick()
        {
            if (!isAwardReceived)
            {
                _animator.SetTrigger("IsTrickSucceeds");

                if (SuccessfullyOvercome != null)
                {
                    int tempRandom = Random.Range(_minPrice,_maxPrice+1);
                    SuccessfullyOvercome.Invoke(tempRandom);
                    isAwardReceived = true;
                }
            }
        }

        public void AddListenerEvent(UnityAction<int> ev)
        {
            if (_events[0] == null)
            {
                SuccessfullyOvercome.AddListener(ev);
                _events[0] = ev;
            }
        }
    }
}