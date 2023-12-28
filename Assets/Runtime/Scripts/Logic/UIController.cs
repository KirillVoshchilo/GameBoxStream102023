using App.Architecture;
using App.Architecture.AppInput;
using App.Content;
using App.Content.Audio;
using App.Content.Player;
using App.Simples.CellsInventory;
using UnityEngine;
using VContainer;

namespace App.Logic
{
    public sealed class UIController
    {
        private readonly LevelTimer _levelTimer;
        private readonly UIStorage _uIStorage;
        private readonly HeatData _playerHeat;
        private readonly IAppInputSystem _appInputSystem;
        private readonly AudioStorage _audioController;

        private bool _isInteractionEnable;
        private bool _isInventoryEnable;

        [Inject]
        public UIController(IAppInputSystem appInputSystem,
            PlayerEntity playerEntity,
            AudioStorage audioController,
            LevelsController levelsController,
            UIStorage uIStorage,
            LevelTimer levelTimer)
        {
            _levelTimer = levelTimer;
            _uIStorage = uIStorage;
            levelsController.UiController = this;
            _audioController = audioController;
            _playerHeat = playerEntity.Get<HeatData>();
            _uIStorage.MainMenuPresenter.UIController = this;
            _appInputSystem = appInputSystem;
            _appInputSystem.OnEscapePressed.AddListener(OnEscClicked);
            _appInputSystem.OnInventoryPressed.AddListener(OnInventoryPressed);
        }
        public void OpenScarecrowMenu()
        {
            _levelTimer.PauseTimer();
            _audioController.AudioData.SoundTracks.OpenFevoniaInterface.Play();
            _playerHeat.CurrentHeat = _playerHeat.DefaultHeatValue;
            _playerHeat.IsFreezing = false;
            _uIStorage.ScareCrowMenuPresenter.gameObject.SetActive(true);
            _uIStorage.ScareCrowMenuPresenter.Enable = true;
            _appInputSystem.IsGoNextEnable = true;
            _appInputSystem.OnGoNext.AddListener(_uIStorage.ScareCrowMenuPresenter.Dialoge.ShowNext);
            _uIStorage.ScareCrowMenuPresenter.Dialoge.OnSlidShowEnded.AddListener(()
                => _appInputSystem.OnGoNext.RemoveListener(_uIStorage.ScareCrowMenuPresenter.Dialoge.ShowNext));
            _uIStorage.ScareCrowMenuPresenter.Dialoge.ShowFirst();
            _appInputSystem.InventoryIsEnable = false;
            _appInputSystem.PlayerMovingIsEnable = false;
            _appInputSystem.InteractionIsEnable = false;
            _appInputSystem.InventoryMoveIsEnable = true;
        }
        public void CloseScarecrowMenu()
        {
            _levelTimer.ContinueTimer();
            _audioController.AudioData.SoundTracks.CloseInventory.Play();
            _playerHeat.IsFreezing = true;
            _appInputSystem.OnGoNext.RemoveListener(_uIStorage.ScareCrowMenuPresenter.Dialoge.ShowNext);
            _appInputSystem.IsGoNextEnable = false;
            _uIStorage.ScareCrowMenuPresenter.Enable = false;
            _uIStorage.ScareCrowMenuPresenter.gameObject.SetActive(false);
            _appInputSystem.InventoryIsEnable = true;
            _appInputSystem.InteractionIsEnable = true;
            _appInputSystem.PlayerMovingIsEnable = true;
            _appInputSystem.InventoryMoveIsEnable = false;
        }
        public void OpenStorageMenu(Inventory inventory)
        {
            _levelTimer.PauseTimer();
            _audioController.AudioData.SoundTracks.OpenGregoryInterface.Play();
            _playerHeat.CurrentHeat = _playerHeat.DefaultHeatValue;
            _playerHeat.IsFreezing = false;
            _uIStorage.StorageMenuPresenter.gameObject.SetActive(true);
            _uIStorage.StorageMenuPresenter.SetInventory(inventory);
            _appInputSystem.IsGoNextEnable = true;
            _appInputSystem.OnGoNext.AddListener(_uIStorage.StorageMenuPresenter.Dialoge.ShowNext);
            _uIStorage.StorageMenuPresenter.Dialoge.OnSlidShowEnded.AddListener(()
                => _appInputSystem.OnGoNext.RemoveListener(_uIStorage.StorageMenuPresenter.Dialoge.ShowNext));
            _uIStorage.StorageMenuPresenter.Dialoge.ShowFirst();
            _uIStorage.StorageMenuPresenter.Enable = true;
            _appInputSystem.InteractionIsEnable = false;
            _appInputSystem.InventoryIsEnable = false;
            _appInputSystem.PlayerMovingIsEnable = false;
            _appInputSystem.InventoryMoveIsEnable = true;
        }
        public void CloseStorageMenu()
        {
            _levelTimer.ContinueTimer();
            _audioController.AudioData.SoundTracks.CloseInventory.Play();
            _playerHeat.IsFreezing = true;
            _appInputSystem.OnGoNext.RemoveListener(_uIStorage.StorageMenuPresenter.Dialoge.ShowNext);
            _appInputSystem.IsGoNextEnable = false;
            _uIStorage.StorageMenuPresenter.Enable = false;
            _uIStorage.StorageMenuPresenter.gameObject.SetActive(false);
            _appInputSystem.InventoryIsEnable = true;
            _appInputSystem.InteractionIsEnable = true;
            _appInputSystem.PlayerMovingIsEnable = true;
            _appInputSystem.InventoryMoveIsEnable = false;
        }
        public void OpenMainMenu()
        {
            Cursor.visible = true;
            _audioController.PlayAudioSource(_audioController.AudioData.CycleTracks.MainMenuMusic);
            _uIStorage.MainMenuPresenter.gameObject.SetActive(true);
            _uIStorage.PauseMenuPresenter.gameObject.SetActive(false);
            _uIStorage.FreezeScreenEffect.gameObject.SetActive(false);
        }
        public void CloseMainMenu()
        {
            Cursor.visible = false;
            _uIStorage.MainMenuPresenter.gameObject.SetActive(false);
        }
        public void ShowFreezeEffect()
            => _uIStorage.FreezeScreenEffect.gameObject.SetActive(true);

        public void OnEscClicked()
        {
            if (_uIStorage.InventoryPresenter.gameObject.activeSelf)
            {
                CloseInventory();
                return;
            }
            if (_uIStorage.ScareCrowMenuPresenter.gameObject.activeSelf)
            {
                CloseScarecrowMenu();
                return;
            }
            if (_uIStorage.StorageMenuPresenter.gameObject.activeSelf)
            {
                CloseStorageMenu();
                return;
            }
            if (_uIStorage.PauseMenuPresenter.gameObject.activeSelf)
                ClosePausePanel();
            else OpenPausePanel();
        }
        public void CloseCurrentOpenedGamePanel()
        {
            if (_uIStorage.InventoryPresenter.gameObject.activeSelf)
                CloseInventory();
            if (_uIStorage.ScareCrowMenuPresenter.gameObject.activeSelf)
                CloseScarecrowMenu();
            if (_uIStorage.StorageMenuPresenter.gameObject.activeSelf)
                CloseStorageMenu();
            if (_uIStorage.PauseMenuPresenter.gameObject.activeSelf)
                ClosePausePanel();
        }
        private void OpenPausePanel()
        {
            _isInteractionEnable = _appInputSystem.InteractionIsEnable;
            _isInventoryEnable = _appInputSystem.InventoryIsEnable;
            _levelTimer.PauseTimer();
            _playerHeat.IsFreezing = false;
            Cursor.visible = true;
            _appInputSystem.InteractionIsEnable = false;
            _appInputSystem.InventoryIsEnable = false;
            _appInputSystem.PlayerMovingIsEnable = false;
            _uIStorage.PauseMenuPresenter.gameObject.SetActive(true);
        }
        private void ClosePausePanel()
        {
            _levelTimer.ContinueTimer();
            _appInputSystem.InteractionIsEnable = _isInteractionEnable;
            _appInputSystem.InventoryIsEnable = _isInventoryEnable;
            _playerHeat.IsFreezing = true;
            Cursor.visible = false;
            _uIStorage.PauseMenuPresenter.CloseTIpsPanel();
            _appInputSystem.PlayerMovingIsEnable = true;
            _uIStorage.PauseMenuPresenter.gameObject.SetActive(false);
        }
        private void OnInventoryPressed()
        {
            if (_uIStorage.InventoryPresenter.gameObject.activeSelf)
                CloseInventory();
            else OpenInventory();
        }
        private void CloseInventory()
        {
            _levelTimer.ContinueTimer();
            _uIStorage.GameWatchPresenter.gameObject.SetActive(true);
            _audioController.AudioData.SoundTracks.CloseInventory.Play();
            _appInputSystem.IsGoNextEnable = false;
            _uIStorage.InventoryPresenter.Enable = false;
            _appInputSystem.InteractionIsEnable = true;
            _uIStorage.InventoryPresenter.gameObject.SetActive(false);
            _appInputSystem.PlayerMovingIsEnable = true;
            _appInputSystem.InventoryMoveIsEnable = false;
        }
        private void OpenInventory()
        {
            _levelTimer.PauseTimer();
            _uIStorage.GameWatchPresenter.gameObject.SetActive(false);
            _audioController.AudioData.SoundTracks.CloseInventory.Play();
            _uIStorage.InventoryPresenter.gameObject.SetActive(true);
            _appInputSystem.IsGoNextEnable = true;
            _uIStorage.InventoryPresenter.Enable = true;
            _appInputSystem.InteractionIsEnable = false;
            _appInputSystem.PlayerMovingIsEnable = false;
            _appInputSystem.InventoryMoveIsEnable = true;
        }
    }
}