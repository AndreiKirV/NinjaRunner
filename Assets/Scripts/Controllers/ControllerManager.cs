namespace game.controllers
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using game.controllers.player;
    using UnityEngine.UI;

    public class ControllerManager
    {
        private Camera _camera;
        private GameObject _player;
        private Button _startRunningPlayer;

        BackgroundController _backgroundController = new BackgroundController();
        GroundController _groundController = new GroundController();
        PlayerController _playerController = new PlayerController();

        public ControllerManager(Camera camera, GameObject player, Button startRunningPlayer)
        {
            _camera = camera;
            _player = player;
            _startRunningPlayer = startRunningPlayer;
        }

        public void Awake() 
        {
            _playerController.SetPlayer(_player);
            _playerController.SetButtonRun(_startRunningPlayer);
            _playerController.Init();

            _backgroundController.SetOffset(4, 1);
            _backgroundController.SetStartPositionZ(40);
            _backgroundController.SetCamera(_camera);
            _backgroundController.Init();

            _groundController.SetPlayer(_player);
            _groundController.Init();
        }

        public void Start()
        {
            
        }

        public void Update()
        {
            _backgroundController.Update();
            _groundController.Update();
            _playerController.Update();
        }
    }
}