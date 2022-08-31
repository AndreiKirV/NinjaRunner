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
        private Camera _camera;
        private Dictionary <int, GameObject> _backgrounds = new Dictionary<int, GameObject>();
        private Dictionary <int, GameObject> _backgroundsClone = new Dictionary<int, GameObject>();

        public void Update() 
        {
            for (int i = 0; i < _backgrounds.Count; i++)
            {
                float tempWidth = _backgrounds[i].GetComponent<SpriteRenderer>().sprite.rect.width / 100 * _backgrounds[i].transform.localScale.x;

                if (_camera.transform.position.x > _backgrounds[i].transform.position.x + tempWidth)
                    _backgrounds[i].transform.position = new Vector3(tempWidth * 2 + _backgrounds[i].transform.position.x, _backgrounds[i].transform.position.y, _backgrounds[i].transform.position.z);
                else if (_camera.transform.position.x > _backgroundsClone[i].transform.position.x + tempWidth)
                    _backgroundsClone[i].transform.position = new Vector3(tempWidth * 2 + _backgroundsClone[i].transform.position.x, _backgroundsClone[i].transform.position.y, _backgroundsClone[i].transform.position.z);
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
            SetStartBGPosition();
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
                _backgrounds.Add(i, tempObj);
                GameObject tempClone = GameMain.InstantiateObject(_backgrounds[i]);
                tempClone.transform.position = new Vector3(tempObj.GetComponent<SpriteRenderer>().sprite.rect.width / 100 * tempObj.transform.localScale.x, tempObj.transform.position.y, tempObj.transform.position.z);
                _backgroundsClone.Add(i, tempClone);
            }
        }

        private void SetStartBGPosition()
        {
            for (int i = 0; i < _backgrounds.Count; i++)
            {
                _backgrounds[i].transform.position = new Vector3(0, _camera.transform.position.y + i * _offsetY, _startPositionZ - i * _offsetZ);
                _backgroundsClone[i].transform.position = new Vector3(_backgroundsClone[i].transform.position.x, _backgrounds[i].transform.position.y, _backgrounds[i].transform.position.z);
            }
        }

        private void SetCameraBackgroundColor()
        {
            SpriteRenderer tempSpriteRenderer = _backgrounds[0].GetComponent<SpriteRenderer>();
            _camera.backgroundColor = tempSpriteRenderer.sprite.texture.GetPixel(0,(int)tempSpriteRenderer.sprite.textureRect.height);
        }
    }
}