namespace game.controllers
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using game.controllers.player;
    using UnityEngine.UI;
    using dictionaries;
    using game.controllers.shop;

    public class ControllerManager
    {
        private Camera _camera;
        private GameObject _player;

        private BackgroundController _backgroundController = new BackgroundController();
        private GroundController _groundController = new GroundController();
        private PlayerController _playerController = new PlayerController();
        private SpawnController _spawnController = new SpawnController();
        private ShopController _shop = new ShopController();
        private UIController _uIController;
        public static float Timer = 0;

        public ControllerManager(Camera camera, GameObject player)
        {
            _camera = camera;
            _player = player;
        }

        public void Awake() 
        {
            _uIController = new UIController(_camera);
            _uIController.Init();
            
            _playerController.SetPlayer(_player);
            _playerController.SetButtonRun(_uIController.GiveButton(ObjectNames.ButtonStartRunning));
            _playerController.SetButtonJump(_uIController.GiveButton(ObjectNames.ButtonJump));
            _playerController.SetButtonHit(_uIController.GiveButton(ObjectNames.ButtonAttack));
            _playerController.SetButtonSlide(_uIController.GiveButton(ObjectNames.ButtonSlide));
            _playerController.SetEventResetRunning(_uIController.EnableButtonRun);
            _playerController.Init();

            _backgroundController.SetOffset(4, 1);
            _backgroundController.SetStartPositionZ(49);
            _backgroundController.SetCamera(_camera);
            _backgroundController.Init();

            _groundController.SetPlayer(_player);
            _groundController.PositionChanged.AddListener(_spawnController.CreateIItem);
            _groundController.Init();

            _spawnController.SetPlayer(_player.GetComponent<Player>());
            _spawnController.Init();
            
            _shop.SetCanvas(_uIController.GiveCanvas());
            _shop.MoneyRequest += _playerController.GiveWallet;
            _shop.Init();
        }

        public void Start()
        {
            
        }

        public void Update()
        {
            _backgroundController.Update();
            _groundController.Update();
            _playerController.Update();

            Timer += Time.deltaTime;
        }

        public void FixedUpdate()
        {
            _playerController.FixedUpdate();
        }
    }
}