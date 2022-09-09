namespace game
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using game.controllers;
    using game.controllers.player;
    using UnityEngine.UI;
    using dictionaries;
    using System.Reflection;

    public class GameMain : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private GameObject _player;
        [SerializeField] private Button _startRunningPlayer;
         [SerializeField] private Button _jumpPlayer;
        private ControllerManager _controllerManager;

        private void Awake() 
        {
            _controllerManager = new ControllerManager(_camera, _player, _startRunningPlayer, _jumpPlayer);
            _controllerManager.Awake();
        }

        private void Start()
        {
            _controllerManager.Start();
        }
        
        private void Update()
        {
            _controllerManager.Update();
        }

        public static GameObject InstantiateObject(GameObject obj)
        {
            return Instantiate(obj);
        }

        public static GameObject InstantiateObject(GameObject obj, Vector3 position)
        {
            return Instantiate(obj, position, Quaternion.identity);
        }

        public static void DestroyObject(GameObject obj, float delay)
        {
            Destroy(obj, delay);
        }
    }
}