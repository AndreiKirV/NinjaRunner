namespace game.controllers
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using game;
    using dictionaries;
    using UnityEngine.Events;

    public class GroundController
    {
        private float _sizeX;
        private GameObject _player;
        private List <GameObject> _grounds = new List<GameObject>();
        public UnityEvent<Vector3> PositionChanged = new UnityEvent<Vector3>();

        public void Update() 
        {
            foreach (var item in _grounds)
            {
                if (item.transform.position.x + _sizeX < _player.transform.position.x)
                {
                    item.transform.position = new Vector3(item.transform.position.x + (_sizeX  * 2), item.transform.position.y, item.transform.position.z);
                    PositionChanged.Invoke(item.transform.position);
                }
            }
        }

        public void Init()
        {
            Create();
        }

        public void SetPlayer(GameObject player)
        {
            _player = player;
        }

        private void Create()
        {
            for (int i = 0; i < 2; i++)
            {
                GameObject tempObj = GameMain.InstantiateObject(Resources.Load<GameObject>($"{Path.PREFABS}BG/Ground"));
                _grounds.Add(tempObj);
                tempObj.name = ObjectNames.Ground;
            }

            SetSizeX();
            _grounds[1].transform.position = new Vector3(_grounds[1].transform.position.x + _sizeX, _grounds[1].transform.position.y, _grounds[1].transform.position.z);
            Resources.UnloadUnusedAssets();
        }

        private void SetSizeX()
        {
            _sizeX = _grounds[0].GetComponent<BoxCollider2D>().size.x;
        }
    }
}