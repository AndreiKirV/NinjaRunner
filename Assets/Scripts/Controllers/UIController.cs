namespace game.controllers
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using dictionaries;
    using UnityEngine.UI;

    public class UIController
    {
        private Camera _camera;
        private Canvas _canvas;
        private Dictionary<string, GameObject> _elements = new Dictionary<string, GameObject>();

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

        public UIController(Camera camera)
        {
            _camera = camera;
        }

        public void Init()
        {
            CreateCanvas();
            CreatePrefabUI(ObjectNames.ButtonStartRunning);
            CreatePrefabUI(ObjectNames.ButtonSlip);
            CreatePrefabUI(ObjectNames.ButtonJump);
            CreatePrefabUI(ObjectNames.ButtonAttack);

            Resources.UnloadUnusedAssets();
        }

        public GameObject GiveUi(string targetUI)
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
    }
}