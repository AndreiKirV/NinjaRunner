namespace game.controllers
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using dictionaries;
    using game.controllers.player;
    using game.enemy;

    public class SpawnController
    {
        private List<GameObject> _items = new List<GameObject>();
        private int _valueAnimationsTrickDeath;
        private Player _player;

        private void Add(string path, string type)
        {
            GameObject tempObject = Resources.Load(path) as GameObject;
            tempObject.name = type;
            _items.Add(tempObject);
        }

        public void Init()
        {/*
            Add($"{Path.DECOR}{ObjectNames.Trunk}", ObjectNames.Trunk);
            Add($"{Path.DECOR}{ObjectNames.HealingChest}", ObjectNames.HealingChest);
            Add($"{Path.DECOR}{ObjectNames.Box}", ObjectNames.JumpingObstacle);
            Add($"{Path.DECOR}{ObjectNames.Crate}", ObjectNames.JumpingObstacle);
            Add($"{Path.DECOR}{ObjectNames.Furnace}", ObjectNames.JumpingObstacle);
            Add($"{Path.DECOR}{ObjectNames.FishBox}", ObjectNames.JumpingObstacle);
            Add($"{Path.DECOR}{ObjectNames.FullTable}", ObjectNames.SlideObstacle);
            Add($"{Path.DECOR}{ObjectNames.Table}", ObjectNames.SlideObstacle);
            Add($"{Path.DECOR}{ObjectNames.TableWithArmor}", ObjectNames.SlideObstacle);*/
            
            Add($"{Path.PREFABS_ENEMIES}{ObjectNames.Enemy}", ObjectNames.Enemy);
            Add($"{Path.PREFABS_ENEMIES}{ObjectNames.Enemy}", ObjectNames.Enemy);
            
            //Add($"{Path.PREFABS_ENEMIES}{ObjectNames.EnemyRanged}", ObjectNames.EnemyRanged);
        }

        public void CreateIItem(Vector3 position)
        {
            int tempIndex = Random.Range(0, _items.Count);
            GameObject tempObject = GameMain.InstantiateObject(_items[tempIndex], position);
            tempObject.name = _items[tempIndex].name;

            if (tempObject.name == ObjectNames.EnemyRanged)
            {
                tempObject.GetComponent<EnemyRanged>().Dead.AddListener(_player.AddFragValue);
                _player.DeathByObstacle.AddListener(delegate {
                    GameMain.DestroyObject(tempObject, 1f);
                });
            }
            else if (tempObject.name == ObjectNames.Enemy)
            {
                tempObject.GetComponent<MeleeEnemy>().Dead.AddListener(_player.AddFragValue);
                _player.DeathByObstacle.AddListener(delegate {
                    GameMain.DestroyObject(tempObject, 1f);
                });
            }
        }

        public void SetPlayer(Player player)
        {
            _player = player;
        }
    }
}