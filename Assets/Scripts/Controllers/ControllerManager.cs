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
        private Button _jumpPlayer;

        private BackgroundController _backgroundController = new BackgroundController();
        private GroundController _groundController = new GroundController();
        private PlayerController _playerController = new PlayerController();
        private SpawnController _spawnController = new SpawnController();
        private UIController _uIController = new UIController();

        public ControllerManager(Camera camera, GameObject player, Button startRunningPlayer, Button jumpPlayer)
        {
            _camera = camera;
            _player = player;
            _startRunningPlayer = startRunningPlayer;
            _jumpPlayer = jumpPlayer;
        }

        public void Awake() 
        {
            _playerController.SetPlayer(_player);
            _playerController.SetButtonRun(_startRunningPlayer);
            _playerController.SetButtonJump(_jumpPlayer);
            _playerController.Init();

            _backgroundController.SetOffset(4, 1);
            _backgroundController.SetStartPositionZ(40);
            _backgroundController.SetCamera(_camera);
            _backgroundController.Init();

            _groundController.SetPlayer(_player);
            _groundController.PositionChanged.AddListener(_spawnController.CreateIItem);
            _groundController.Init();

            _spawnController.Init();
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