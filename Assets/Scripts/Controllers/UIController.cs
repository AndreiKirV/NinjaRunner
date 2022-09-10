namespace game.controllers
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using dictionaries;
    using UnityEngine.UI;
    using TMPro;

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
            CreatePrefabUI(ObjectNames.ButtonSlip).SetActive(false);
            CreatePrefabUI(ObjectNames.ButtonJump).SetActive(false);
            CreatePrefabUI(ObjectNames.ButtonAttack).SetActive(false);

            CreateCounters();

            _elements[ObjectNames.ButtonStartRunning].GetComponent<Button>().onClick.AddListener(DisableButtonRun);

            Resources.UnloadUnusedAssets();
        }

        public void EnableButtonRun()
        {
            ChangeActivityUI(ObjectNames.ButtonStartRunning);
            ChangeActivityUI(ObjectNames.ButtonSlip);
            ChangeActivityUI(ObjectNames.ButtonJump);
            ChangeActivityUI(ObjectNames.ButtonAttack);
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

        public Button GiveButton(string targetButton)
        {
            Button tempButton = null;
            GameObject tempObject = GiveUi(targetButton);

            if (tempObject != null && tempObject.TryGetComponent<Button>(out Button button))
                tempButton = button;

            return tempButton;
        }

        private void ChangeActivityUI(string targetObject)
        {
            _elements[targetObject].SetActive(!_elements[targetObject].activeSelf);
        }

        private void DisableButtonRun()
        {
            ChangeActivityUI(ObjectNames.ButtonStartRunning);
            ChangeActivityUI(ObjectNames.ButtonSlip);
            ChangeActivityUI(ObjectNames.ButtonJump);
            ChangeActivityUI(ObjectNames.ButtonAttack);
        }
    }
}