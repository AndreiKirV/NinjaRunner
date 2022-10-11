namespace game.controllers.shop
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using dictionaries;
    using TMPro;
    using UnityEngine.UI;
    using UnityEngine.Events;

    public class Product
    {
        private GameObject _view;
        private TextMeshProUGUI _priceText;
        private int _price = 0;
        private Sprite _weapon;
        private Button _button;

        public int Price => _price;
        public Sprite Weapon => _weapon;

        public UnityEvent <Product> WeaponsSelected = new UnityEvent <Product> ();

        public Product(Transform parent)
        {
            _view = GameMain.InstantiateObject(Resources.Load<GameObject>($"{Path.PREFABS_UI}{ObjectNames.Product}"), parent);
        }

        public void SetPrice(int value)
        {
            if (value != 0)
            {
                _priceText = _view.GetComponentInChildren<TextMeshProUGUI>();
                _priceText.text = value.ToString(); 
                _price = value;
            }
            else
            {
                _priceText = _view.GetComponentInChildren<TextMeshProUGUI>();
                _priceText.text = "V";
                _price = 0;
            }

            _button = _view.GetComponentInChildren<Button>();
            _button.onClick.AddListener(delegate {
                WeaponsSelected.Invoke(this);
                PlayerPrefs.SetInt(_weapon.name, 1);
                });
        }

        public void SetSprite(Sprite sprite)
        {
            _view.transform.Find("View").gameObject.GetComponent<Image>().overrideSprite = sprite;
            _weapon = sprite;
        }
    }
}