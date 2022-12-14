namespace game.controllers
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using dictionaries;
    using UnityEngine.UI;
    using UnityEngine.Events;
    using TMPro;
    using game.controllers.player;
    using UnityEngine.SceneManagement;
    using game.controllers.shop;

    public class UIController
    {
        private Camera _camera;
        private Canvas _canvas;
        private Dictionary<string, GameObject> _elements = new Dictionary<string, GameObject>();
        private Dictionary<string, TextMeshProUGUI> _counters = new Dictionary<string, TextMeshProUGUI>();
        private SaveRecordController _records;
        public delegate int IntInput();
        public static IntInput LivesRequest;
        public delegate bool BoolOutput(string name);
        public static BoolOutput PanelRequest;

        public UIController(Camera camera)
        {
            _camera = camera;
        }

        private void MenuInit()
        {
            _records = _elements[ObjectNames.RecordCounter].GetComponent<SaveRecordController>();
            
            GiveButton($"{ObjectNames.Button}{ObjectNames.Continue}").onClick.AddListener(delegate {
                if (LivesRequest.Invoke() > 0)
                {
                    ChangeActivityUI(ObjectNames.Panel);
                    Time.timeScale = 1;
                }
                });

            GiveButton($"{ObjectNames.Button}{ObjectNames.Restart}").onClick.AddListener(delegate {
                _records.SetValue(int.Parse(_counters[ObjectNames.TrickCounter].text), int.Parse(_counters[ObjectNames.FragCounter].text));
                Player.CalculateGold.Invoke();
                ChangeActivityUI(ObjectNames.Panel);
                Time.timeScale = 1;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                });
                
            GiveButton($"{ObjectNames.Button}{ObjectNames.Shop}").onClick.AddListener(delegate {
                Player.CalculateGold.Invoke();
                ShopController.OpenShop.Invoke();
                });

            GiveButton($"{ObjectNames.Button}{ObjectNames.Exit}").onClick.AddListener(delegate {
                _records.SetValue(int.Parse(_counters[ObjectNames.TrickCounter].text), int.Parse(_counters[ObjectNames.FragCounter].text));
                Player.CalculateGold.Invoke();
                Application.Quit();
                });

            GiveButton(ObjectNames.ButtonResetPlayer).onClick.AddListener(delegate {
                Player.ResPlayer.Invoke();
            });

            Player.DeathInit();
            Player.Death.AddListener(delegate {
                Time.timeScale = 0;
                ChangeActivityUI(ObjectNames.Panel);
                });

            GiveButton(ObjectNames.ButtonResetPlayer).gameObject.SetActive(false);

            PanelRequest = CheckActivityElement;
        }

        private void CreateMenu()
        {
            CreatePrefabUI(ObjectNames.Panel).SetActive(false);
            CreateButtonMenu(ObjectNames.Continue);
            CreateButtonMenu(ObjectNames.Restart);
            CreateButtonMenu(ObjectNames.Shop);
            CreateButtonMenu(ObjectNames.Exit);
            CreatePrefabUI(ObjectNames.RecordCounter, ObjectNames.RecordCounter, _elements[ObjectNames.Panel]);

            CreateObject(ObjectNames.ButtonResetPlayer, ObjectNames.ButtonContinue);

            float with = _canvas.gameObject.GetComponent<CanvasScaler>().referenceResolution.x;
            float height = _canvas.gameObject.GetComponent<CanvasScaler>().referenceResolution.y / 6.5f;
            _elements[ObjectNames.Panel].GetComponent<GridLayoutGroup>().cellSize = new Vector2(with, height);
        }

        private void CreateButtonMenu(string name)
        {
            GameObject tempObject = CreatePrefabUI(ObjectNames.Button, $"{ObjectNames.Button}{name}");
            tempObject.SetActive(true);
            tempObject.name = name;
            tempObject.transform.SetParent(_elements[ObjectNames.Panel].transform);
            tempObject.GetComponentInChildren<TextMeshProUGUI>().text = name;
        }

        private void CreateObject(string name, string parent)
        {
            GameObject tempButton = CreatePrefabUI(name, name, _elements[parent]);
            tempButton.SetActive(true);
            tempButton.name = name;
        }

        private void ChangeFragValue(int value)
        {
            ChangeCounterValue(ObjectNames.FragCounter, value.ToString());
        }

        private void ChangeSpeedValue(int value)
        {
            ChangeCounterValue(ObjectNames.SpeedCounter, value.ToString());
        }

        private void ChangeTrickValue(int value)
        {
            ChangeCounterValue(ObjectNames.TrickCounter, value.ToString());
        }

        private void ChangeGoldValue(int value)
        {
            ChangeCounterValue(ObjectNames.GoldCounter, value.ToString());
        }

        private void ChangeLivesValue(int value)
        {
            ChangeCounterValue(ObjectNames.LiveCounter, value.ToString());
        }

        private void ChangeCounterValue(string name, string text)
        {
            _counters[name].text = text;
        }

        private void CreateCounters()
        {
            CreateCounter(ObjectNames.GoldCounter);
            CreateCounter(ObjectNames.SpeedCounter);
            CreateCounter(ObjectNames.TrickCounter);
            CreateCounter(ObjectNames.FragCounter);
            CreateCounter(ObjectNames.LiveCounter);
        }

        private void CreateCounter(string objectName)
        {
            GameObject tempObject = CreatePrefabUI(objectName);
            TextMeshProUGUI tempTMP = tempObject.GetComponentInChildren<TextMeshProUGUI>();
            _counters.Add(objectName, tempTMP);
        }

        private GameObject CreatePrefabUI(string objectName)
        {
            GameObject tempObject;
            GameObject targetObject = Resources.Load<GameObject>($"{Path.PREFABS_UI}{objectName}");

            if (targetObject.name != ObjectNames.Canvas)
                tempObject = GameMain.InstantiateObject(targetObject, _elements[ObjectNames.Canvas].transform);
            else
                tempObject = GameMain.InstantiateObject(targetObject);

            tempObject.name = targetObject.name;
            _elements.Add(tempObject.name, tempObject);
            return tempObject;
        }

        private GameObject CreatePrefabUI(string objectName, string targetName)
        {
            GameObject tempObject;
            GameObject targetObject = Resources.Load<GameObject>($"{Path.PREFABS_UI}{objectName}");

            if (targetObject.name != ObjectNames.Canvas)
                tempObject = GameMain.InstantiateObject(targetObject, _elements[ObjectNames.Canvas].transform);
            else
                tempObject = GameMain.InstantiateObject(targetObject);

            tempObject.name = targetName;
            _elements.Add(tempObject.name, tempObject);
            return tempObject;
        }

        private GameObject CreatePrefabUI(string objectName, string targetName, GameObject parent)
        {
            GameObject tempObject;
            GameObject targetObject = Resources.Load<GameObject>($"{Path.PREFABS_UI}{objectName}");
            
            tempObject = GameMain.InstantiateObject(targetObject, parent.transform);

            tempObject.name = targetName;
            _elements.Add(tempObject.name, tempObject);
            return tempObject;
        }

        private void CreateCanvas()
        {
            GameObject canvas = CreatePrefabUI(ObjectNames.Canvas);
            _canvas = canvas.GetComponent<Canvas>();
            _canvas.worldCamera = _camera;
        }

        private GameObject GiveUi(string targetUI)
        {
            GameObject tempObject = null;

            if (_elements.ContainsKey(targetUI))
                tempObject = _elements[targetUI];

            return tempObject;
        }

        private void ChangeActivityUI(string targetObject)
        {
            _elements[targetObject].SetActive(!_elements[targetObject].activeSelf);
        }

        public void DisableButtonRun()
        {
            if (int.Parse(_counters[ObjectNames.LiveCounter].text) > 0)
            {
                ChangeActivityUI(ObjectNames.ButtonStartRunning);
                ChangeActivityUI(ObjectNames.ButtonSlide);
                ChangeActivityUI(ObjectNames.ButtonJump);
                ChangeActivityUI(ObjectNames.ButtonAttack);
            }
        }

        public void Init()
        {
           
            CreateCanvas(); 
            CreatePrefabUI(ObjectNames.ButtonStartRunning);
            CreatePrefabUI(ObjectNames.ButtonSlide).SetActive(false);
            CreatePrefabUI(ObjectNames.ButtonJump).SetActive(false);
            CreatePrefabUI(ObjectNames.ButtonAttack).SetActive(false);
            CreatePrefabUI(ObjectNames.ButtonMenu).SetActive(true);
            
            _elements[ObjectNames.ButtonStartRunning].GetComponent<Button>().onClick.AddListener(DisableButtonRun);

            Player.ValueTrickChanged.AddListener(ChangeTrickValue);
            Player.ValueLivesChanged.AddListener(ChangeLivesValue);
            Player.ValueGoldChanged.AddListener(ChangeGoldValue);
            Player.ValueFragChanged.AddListener(ChangeFragValue);
            Player.ValueCurrentSpeedChanged.AddListener(ChangeSpeedValue);
            
            CreateMenu();
            MenuInit();

            CreateCounters();
            
            GiveButton(ObjectNames.ButtonMenu).onClick.AddListener(delegate {
                _records.SetValue(int.Parse(_counters[ObjectNames.TrickCounter].text), int.Parse(_counters[ObjectNames.FragCounter].text));
                ChangeActivityUI(ObjectNames.Panel);
                Time.timeScale = 0;
            });

            Resources.UnloadUnusedAssets();
        }

        public void EnableButtonRun()
        {
            if (_elements[ObjectNames.ButtonStartRunning].activeSelf == false)
            {
                ChangeActivityUI(ObjectNames.ButtonStartRunning);
                ChangeActivityUI(ObjectNames.ButtonSlide);
                ChangeActivityUI(ObjectNames.ButtonJump);
                ChangeActivityUI(ObjectNames.ButtonAttack);
            }
        }

        public Canvas GiveCanvas()
        {
            return _canvas;
        }

        public Button GiveButton(string targetButton)
        {
            Button tempButton = null;
            GameObject tempObject = GiveUi(targetButton);

            if (tempObject != null && tempObject.TryGetComponent<Button>(out Button button))
                tempButton = button;

            return tempButton;
        }

        private bool CheckActivityElement(string name)
        {
            return _elements[name].activeSelf;
        }
    }
}