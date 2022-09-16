namespace game.controllers
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using dictionaries;

    public class SpawnController
    {
        private List<GameObject> _items = new List<GameObject>();

        private void Add(string path, string type)
        {
            GameObject tempObject = Resources.Load(path) as GameObject;
            tempObject.name = type;
            _items.Add(tempObject);
        }

        public void Init()
        {
            Add($"{Path.DECOR}Trunk", ObjectNames.JumpingObstacle);
            Add($"{Path.DECOR}Box", ObjectNames.JumpingObstacle);
            Add($"{Path.DECOR}Crate", ObjectNames.JumpingObstacle);
            Add($"{Path.DECOR}{ObjectNames.FullTable}", ObjectNames.SlideObstacle);

        }

        public void CreateIItem(Vector3 position)
        {
            int tempIndex = Random.Range(0, _items.Count);
            GameObject tempObject = GameMain.InstantiateObject(_items[tempIndex], position);
            tempObject.name = _items[tempIndex].name;
        }
    }
}