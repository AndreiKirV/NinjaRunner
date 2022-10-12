namespace game.controllers
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using TMPro;
    using dictionaries;
    using game.controllers.player;

    public class SaveRecordController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _trickValue;
        [SerializeField] private TextMeshProUGUI _fragValue;
        private void Awake() 
        {
            Player.ValueRecordsChanged.AddListener(SetValue);
        }
        private void Start()
        {
            if (PlayerPrefs.HasKey(ObjectNames.TrickCounter))
            {
                TakeValue();
            }
        }

        public void SetValue(int trickValue, int fragValue)
        {
            if (PlayerPrefs.HasKey(ObjectNames.TrickCounter))
            {
                if (fragValue > PlayerPrefs.GetInt(ObjectNames.FragCounter))
                {
                    PlayerPrefs.SetInt(ObjectNames.FragCounter, fragValue);
                }

                if (trickValue > PlayerPrefs.GetInt(ObjectNames.TrickCounter))
                {
                    PlayerPrefs.SetInt(ObjectNames.TrickCounter, trickValue);
                }
            }
            else
            {
                PlayerPrefs.SetInt(ObjectNames.TrickCounter, trickValue);
                PlayerPrefs.SetInt(ObjectNames.FragCounter, fragValue);
            }
            
            TakeValue();
        }

        private void TakeValue()
        {
            _trickValue.text = PlayerPrefs.GetInt(ObjectNames.TrickCounter).ToString();
            _fragValue.text = PlayerPrefs.GetInt(ObjectNames.FragCounter).ToString();
        }
    }
}