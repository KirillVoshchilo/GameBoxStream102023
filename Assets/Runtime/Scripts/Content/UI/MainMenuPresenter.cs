﻿using App.Architecture;
using App.Architecture.AppData;
using App.Architecture.AppInput;
using App.Content.Player;
using App.Logic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace App.Content.UI
{
    public sealed class MainMenuPresenter : MonoBehaviour
    {
        [SerializeField] private Button _startGameButton;
        [SerializeField] private Button _closeAppButton;
        [SerializeField] private Button _descriptionButton;
        [SerializeField] private Button _closeDescriptionButton;
        [SerializeField] private GameObject _descriptionPanel;

        private IAppInputSystem _appInputSystem;
        private LevelLoaderSystem _levelLoader;
        private Configuration _configuration;
        private Inventory _playerInventory;
        private PlayerEntity _playerEntity;
        private UIController _uiController;
        private DefeatController _defeatController;

        public UIController UIController { set => _uiController = value; }

        [Inject]
        public void Construct(IAppInputSystem appInputSystem,
            LevelLoaderSystem levelLoader,
            PlayerEntity playerEntity,
            Configuration configuration,
            DefeatController defeatController)
        {
            _defeatController = defeatController;
            _configuration = configuration;
            _playerInventory = playerEntity.Get<Inventory>();
            _playerEntity = playerEntity;
            _levelLoader = levelLoader;
            appInputSystem.EscapeIsEnable = false;
            appInputSystem.PlayerMovingIsEnable = false;
            _appInputSystem = appInputSystem;
            _closeAppButton.onClick.AddListener(OnCloseAppClicked);
            _startGameButton.onClick.AddListener(OnStartNewGameButton);
            _closeDescriptionButton.onClick.AddListener(OnCloseDescriptionClicked);
            _descriptionButton.onClick.AddListener(OnDescriptionClicked);
        }

        private void OnDescriptionClicked()
            => _descriptionPanel.SetActive(true);
        private void OnCloseDescriptionClicked()
            => _descriptionPanel.SetActive(false);
        private void OnStartNewGameButton()
        {
            _defeatController.IsEnable = true;
            _levelLoader.LoadScene(LevelLoaderSystem.FIRST_LEVEL, OnCompleteLoading)
                .Forget();
            int count = _configuration.StartInventoryConfiguration.Items.Length;
            for (int i = 0; i < count; i++)
            {
                Key key = _configuration.StartInventoryConfiguration.Items[i].Key;
                int quantity = _configuration.StartInventoryConfiguration.Items[i].Count;
                _playerInventory.AddItem(key, quantity);
            }
            _playerEntity.GetComponent<Rigidbody>().useGravity = true;
            HeatData heatData = _playerEntity.Get<HeatData>();
            heatData.CurrentHeat = heatData.DefaultHeatValue;
            heatData.IsFreezing = true;
            _appInputSystem.EscapeIsEnable = true;
            _appInputSystem.InventoryIsEnable = true;
            _appInputSystem.PlayerMovingIsEnable = true;
            gameObject.SetActive(false);
            _uiController.ShowFreezeEffect();
        }
        private void OnCompleteLoading(LevelStorage storage)
            => _playerEntity.transform.position = storage.PlayerTransform.position;
        private void OnCloseAppClicked()
            => Application.Quit();
    }
}