namespace game.controllers.enemy
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using dictionaries;

    public class EnemyViewController
    {
        private List<SpriteRenderer> _bones = new List<SpriteRenderer>();
        private List<string> _path = new List<string>();
        private List<GameObject> _bodyParts = new List<GameObject>();

        private void InitBodyParts(List<SpriteRenderer> bones)
        {
            for (int i = 0; i < bones.Count; i++)
            {
                if (bones[i].gameObject.name ==  ObjectNames.Head || bones[i].gameObject.name ==  ObjectNames.RightArm || bones[i].gameObject.name ==  ObjectNames.LeftArm || bones[i].gameObject.name == ObjectNames.RightHand || bones[i].gameObject.name ==  ObjectNames.LeftHand)
                {
                    _bodyParts.Add(bones[i].gameObject);  
                }
            }
        }

        public void SetBones(List<SpriteRenderer> bones)
        {
            _bones = bones;
            InitBodyParts(bones);
        }

        public GameObject GiveBodyPart()
        {
            return _bodyParts[Random.Range(0, _bodyParts.Count)];
        }

        public void InitMeleeEnemy()
        {
            _path.Add($"{Path.ENEMY_VIEW}{ObjectNames.Golem}1");
            _path.Add($"{Path.ENEMY_VIEW}{ObjectNames.Golem}2");
            _path.Add($"{Path.ENEMY_VIEW}{ObjectNames.Golem}3");
            _path.Add($"{Path.ENEMY_VIEW}{ObjectNames.Minotaur}1");
            _path.Add($"{Path.ENEMY_VIEW}{ObjectNames.Minotaur}2");
            _path.Add($"{Path.ENEMY_VIEW}{ObjectNames.Minotaur}3");
        }

        public void SetSprites()
        {
            string tempPath = _path[Random.Range(0, _path.Count)];

            foreach (var item in _bones)
            {
                string tempName = item.name;

                if (tempName != ObjectNames.Weapon)
                    item.sprite = Resources.Load<Sprite>($"{tempPath}/{tempName}");
                else
                    item.sprite = Resources.Load<Sprite>($"{_path[Random.Range(0, _path.Count)]}/{tempName}");
            }

            Resources.UnloadUnusedAssets();
        }
    }
}