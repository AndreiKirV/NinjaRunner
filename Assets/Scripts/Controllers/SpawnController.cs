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
        }

        public void CreateIItem(Vector3 position)
        {
            GameObject tempObject = GameMain.InstantiateObject(_items[Random.Range(0, _items.Count)], position);
            tempObject.name = ObjectNames.JumpingObstacle;
            GameMain.DestroyObject(tempObject, 15);
        }
    }
}