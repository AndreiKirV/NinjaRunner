namespace game.controllers
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using game;
    using dictionaries;

    public class BackgroundController
    {
        private float _offsetZ;
        private float _offsetY;
        private float _startPositionZ;
        private int _bGValue = 3;
        private Vector3 _localScale;
        private SpriteRenderer _spriteRenderer;
        private Camera _camera;
        private List <GameObject> _backgrounds = new List<GameObject>();

        public void Update() 
        {
            float tempWidth = _spriteRenderer.sprite.rect.width / 100 * _localScale.x;

            for (int i = 0; i < _backgrounds.Count; i++)
            {
                if (_camera.transform.position.x > _backgrounds[i].transform.position.x + tempWidth * 2)
                    _backgrounds[i].transform.position = new Vector3(tempWidth * _backgrounds.Count + _backgrounds[i].transform.position.x, _backgrounds[i].transform.position.y, _backgrounds[i].transform.position.z);
            }
        }

        public void SetCamera(Camera camera)
        {
            _camera = camera;
        }

        public void SetOffset(float offsetZ, float offsetY)
        {
            _offsetZ = offsetZ;
            _offsetY = offsetY;
        }

        public void SetStartPositionZ(float z)
        {
            _startPositionZ = z;
        }

        public void Init()
        {
            CreateBG();
            SetCameraBackgroundColor();
        }

        private int CheckedSpriteValuePath(string path)
        {
            bool isChecked = false;
            int  i = 0;

            while (isChecked == false)
            {
                Texture2D temp = Resources.Load<Texture2D>($"{path}{i+1}");

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

        private int CheckedValueTypesBG()
        {
            bool isChecked = false;
            int  i = 0;

            while (isChecked == false)
            {
                Texture2D temp = Resources.Load<Texture2D>($"{Path.BACKGROUND}{i+1}/1");

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

        private void CreateBG()
        {
            int BGIndex = Random.Range(1, CheckedValueTypesBG() + 1);

            for (int i = 0; i < CheckedSpriteValuePath($"{Path.BACKGROUND}{BGIndex}/"); i++)
            {
                Material tempMaterial = Resources.Load<Material>("Material/BG");
                tempMaterial.mainTexture = Resources.Load<Texture2D>($"{Path.BACKGROUND}{BGIndex}/{i+1}");
                GameObject tempObj = GameMain.InstantiateObject(Resources.Load<GameObject>($"{Path.PREFABS}BG/BG"));
                tempObj.GetComponent<SpriteRenderer>().material.SetTexture(i.ToString(), tempMaterial.mainTexture);
                tempObj.GetComponent<SpriteRenderer>().sprite = Sprite.Create(Resources.Load<Texture2D>($"{Path.BACKGROUND}{BGIndex}/{i+1}"), new Rect(0, 0, tempMaterial.mainTexture.width, tempMaterial.mainTexture.height), new Vector2(0.5f,0.5f));
                _backgrounds.Add(tempObj);
            }

            _localScale = new Vector3(_backgrounds[0].transform.localScale.x, _backgrounds[0].transform.localScale.y, _backgrounds[0].transform.localScale.z);
            SetStartBGPosition();
            GameObject bg = new GameObject("BG");
            _spriteRenderer = _backgrounds[0].GetComponent<SpriteRenderer>();
            bg.transform.position = new Vector3(_backgrounds[0].transform.position.x - _spriteRenderer.sprite.rect.width / 100 * _localScale.x / 2, _backgrounds[0].transform.position.y - _spriteRenderer.sprite.rect.height / 100 * _localScale.y / 2, _backgrounds[0].transform.position.z);
            
            foreach (var item in _backgrounds)
            {
                item.transform.parent = bg.transform;
            }

            _backgrounds.Clear();
            _backgrounds.Add(bg);

            for (int i = 1; i < _bGValue; i++)
            {
                GameObject tempObject = GameMain.InstantiateObject(_backgrounds[0]);
                tempObject.transform.position = new Vector3(tempObject.transform.position.x + _spriteRenderer.sprite.rect.width / 100 * _localScale.x * i, tempObject.transform.position.y, tempObject.transform.position.z);
                _backgrounds.Add(tempObject);
            }

            Resources.UnloadUnusedAssets();
        }

        private void SetStartBGPosition()
        {
            for (int i = 0; i < _backgrounds.Count; i++)
            {
                _backgrounds[i].transform.position = new Vector3(0, _camera.transform.position.y + i * _offsetY, _startPositionZ - i * _offsetZ);
            }
        }

        private void SetCameraBackgroundColor()
        {
            _camera.backgroundColor = _spriteRenderer.sprite.texture.GetPixel(0,(int)_spriteRenderer.sprite.textureRect.height);
        }
    }
}