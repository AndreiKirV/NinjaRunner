namespace game.item
{

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using dictionaries;
    using game.controllers.player;
    using UnityEngine.Events;

    public class HealingChest : Item
    {
        public UnityEvent SuccessfullyOvercome = new UnityEvent();
        private UnityAction [] _events = new UnityAction[1];
        private bool isAwardReceived = false;

        protected override void StartAnimatingSuccessfulTrick()
        {
            if (!isAwardReceived)
            {
                _animator.SetTrigger("IsTrickSucceeds");

                if (SuccessfullyOvercome != null)
                {
                    SuccessfullyOvercome.Invoke();
                    isAwardReceived = true;
                }
            }
        }

        public void AddListenerEvent(UnityAction ev)
        {
            if (_events[0] == null)
            {
                SuccessfullyOvercome.AddListener(ev);
                _events[0] = ev;
            }
        }
    }
}