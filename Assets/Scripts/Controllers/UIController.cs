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

    public class UIController
    {
        private Camera _camera;
        private Canvas _canvas;
        private bool _isGameActive = true;
        private Dictionary<string, GameObject> _elements = new Dictionary<string, GameObject>();
        private Dictionary<string, TextMeshProUGUI> _counters = new Dictionary<string, TextMeshProUGUI>();

        public UIController(Camera camera)
        {
            _camera = camera;
        }

        public void Init()
        {
            CreateCanvas();
            CreatePrefabUI(ObjectNames.ButtonStartRunning);
            CreatePrefabUI(ObjectNames.ButtonSlide).SetActive(false);
            CreatePrefabUI(ObjectNames.ButtonJump).SetActive(false);
            CreatePrefabUI(ObjectNames.ButtonAttack).SetActive(false);
            CreatePrefabUI(ObjectNames.ButtonMenu).SetActive(true);
            CreateMenu();
            MenuInit();

            CreateCounters();

            _elements[ObjectNames.ButtonStartRunning].GetComponent<Button>().onClick.AddListener(DisableButtonRun);

            Player.ValueTrickChanged.AddListener(ChangeTrickValue);
            Player.ValueLivesChanged.AddListener(ChangeLivesValue);
            Player.ValueGoldChanged.AddListener(ChangeGoldValue);
            Player.ValueCurrentSpeedChanged.AddListener(ChangeSpeedValue);
            Player.ValueFragChanged.AddListener(ChangeFragValue);

            GiveButton(ObjectNames.ButtonMenu).onClick.AddListener(delegate {
                ChangeActivityUI(ObjectNames.Panel);
                Time.timeScale = 0;
            });

            Resources.UnloadUnusedAssets();
        }

        public void EnableButtonRun()
        {
            ChangeActivityUI(ObjectNames.ButtonStartRunning);
            ChangeActivityUI(ObjectNames.ButtonSlide);
            ChangeActivityUI(ObjectNames.ButtonJump);
            ChangeActivityUI(ObjectNames.ButtonAttack);
        }

        private void MenuInit()
        {
            GiveButton($"{ObjectNames.Button}Continue").onClick.AddListener(delegate {
                ChangeActivityUI(ObjectNames.Panel);
                Time.timeScale = 1;});

            GiveButton($"{ObjectNames.Button}Restart").onClick.AddListener(delegate {
                ChangeActivityUI(ObjectNames.Panel);
                Time.timeScale = 1;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);});
                
            GiveButton($"{ObjectNames.Button}Shop").onClick.AddListener(delegate {});
            GiveButton($"{ObjectNames.Button}Exit").onClick.AddListener(delegate {Application.Quit();});

            GiveButton(ObjectNames.ButtonResetPlayer).onClick.AddListener(delegate {
                Player.ResPlayer.Invoke();
            });

            Player.Death.AddListener(delegate {
                Time.timeScale = 0;
                ChangeActivityUI(ObjectNames.Panel);});
        }

        private void CreateMenu()
        {
            CreatePrefabUI(ObjectNames.Panel).SetActive(false);
            CreateButtonMenu("Continue");
            CreateButtonMenu("Restart");
            CreateButtonMenu("Shop");
            CreateButtonMenu("Exit");
            CreateButton(ObjectNames.ButtonResetPlayer, "ButtonContinue");

            float with = _canvas.gameObject.GetComponent<CanvasScaler>().referenceResolution.x;
            float height = _canvas.gameObject.GetComponent<CanvasScaler>().referenceResolution.y / 5;
            _elements[ObjectNames.Panel].GetComponent<GridLayoutGroup>().cellSize = new Vector2(with, height);
        }

        private void CreateButtonMenu(string name)
        {
            GameObject tempButton = CreatePrefabUI(ObjectNames.Button, $"{ObjectNames.Button}{name}");
            tempButton.SetActive(true);
            tempButton.name = name;
            tempButton.transform.SetParent(_elements[ObjectNames.Panel].transform);
            tempButton.GetComponentInChildren<TextMeshProUGUI>().text = name;
        }

        private void CreateButton(string name, string parent)
        {
            GameObject tempButton = CreatePrefabUI(name, name, _elements[parent]);
            tempButton.SetActive(true);
            tempButton.name = name;
        }

        private void ChangeFragValue(int value)
        {
            ChangeCounterValue(ObjectNames.FragCounter, value.ToString());
        }

        private void ChangeSpeedValue(float value)
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

        public Button GiveButton(string targetButton)
        {
            Button tempButton = null;
            GameObject tempObject = GiveUi(targetButton);

            if (tempObject != null && tempObject.TryGetComponent<Button>(out Button button))
                tempButton = button;

            return tempButton;
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
            
            tempObject = GameMain.Instantiate(targetObject, parent.transform);

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

        private void DisableButtonRun()
        {
            if (int.Parse(_counters[ObjectNames.LiveCounter].text) > 0)
            {
                ChangeActivityUI(ObjectNames.ButtonStartRunning);
                ChangeActivityUI(ObjectNames.ButtonSlide);
                ChangeActivityUI(ObjectNames.ButtonJump);
                ChangeActivityUI(ObjectNames.ButtonAttack);
            }
        }
    }
}