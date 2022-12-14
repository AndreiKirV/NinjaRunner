namespace game.controllers.shop
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using dictionaries;
    using TMPro;
    using UnityEngine.Events;
    using UnityEngine.UI;

    public class ShopController
    {
        private Canvas _canvas;
        private GameObject _shopView;
        private GridLayoutGroup _groupView;
        public static UnityEvent <Sprite> WeaponsPurchased = new UnityEvent <Sprite>();
        public static UnityEvent OpenShop;
        public delegate int IntInput();
        public IntInput MoneyRequest;
        public delegate void IntOutput(int value);
        public IntOutput GoldSpent;

        private int CheckedValueWeapon()
        {
            bool isChecked = false;
            int  i = 0;

            while (isChecked == false)
            {
                Sprite temp = Resources.Load<Sprite>($"{Path.WEAPON}{i+1}");

                if (temp == null)
                {
                    isChecked = true;
                    break;
                }

                i++;
            }

            Resources.UnloadUnusedAssets();
            return i;
        }

        private void CreateCells()
        {
            int maxWeapon = CheckedValueWeapon();

            for (int i = 0; i < maxWeapon; i++)
            {
                Product tempProduct = new Product(_groupView.gameObject.transform);
                Sprite tempSprite = Resources.Load<Sprite>($"{Path.WEAPON}{i+1}");
                tempSprite.name = (i+1).ToString();
                tempProduct.SetSprite(tempSprite);
                tempProduct.WeaponsSelected.AddListener(GiveWeapon);

                if (!PlayerPrefs.HasKey(tempSprite.name))
                {
                    if (i+1 == 41 || i+1 == 42 || i+1 == 43)
                    {
                        tempProduct.SetPrice(0);
                    }
                    else
                    {    
                        switch (i % 5) 
                        {
                            case 0: 
                                tempProduct.SetPrice(50);
                                break;
                            case 1:
                                tempProduct.SetPrice(100);
                                break;
                            case 2:
                                tempProduct.SetPrice(150);
                                break;
                            case 3:
                                tempProduct.SetPrice(200);
                                break;
                            case 4:
                                tempProduct.SetPrice(250);
                                break;
                        }
                    }
                }
                else
                tempProduct.SetPrice(0);
            }

            Resources.UnloadUnusedAssets();
        }

        public void SetCanvas(Canvas canvas)
        {
            _canvas = canvas;
        }

        public void Init()
        {
            OpenShop = new UnityEvent();

            _shopView = GameMain.InstantiateObject(Resources.Load<GameObject>($"{Path.PREFABS_UI}{ObjectNames.ShopView}"), _canvas.transform);
            _shopView.name = ObjectNames.ShopView;
            _shopView.gameObject.SetActive(false);
            _groupView = _shopView.GetComponentInChildren<GridLayoutGroup>();
            CreateCells();

            OpenShop.AddListener(SetUpStore);
            _shopView.transform.Find(ObjectNames.ButtonExit).GetComponentInChildren<Button>().onClick.AddListener(SetUpStore);
        }

        private void GiveWeapon(Product targetProduct)
        {
            if (targetProduct.Price <= MoneyRequest.Invoke())
            {
                WeaponsPurchased.Invoke(targetProduct.Weapon);
                GoldSpent(targetProduct.Price);
                targetProduct.SetPrice(0);
            }
        }

        private void SetUpStore()
        {
            _shopView.SetActive(!_shopView.activeSelf);
        }
    }
}