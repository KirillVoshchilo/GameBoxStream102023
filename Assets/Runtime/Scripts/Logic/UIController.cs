using App.Architecture.AppInput;
using App.Content.Player;
using App.Content.UI;
using App.Content.UI.InventoryUI;
using UnityEngine;
using VContainer;

namespace App.Logic
{
    public sealed class UIController : MonoBehaviour
    {
        [SerializeField] private InventoryPresenter _inventoryPresenter;
        [SerializeField] private MainMenuPresenter _mainMenuPresenter;
        [SerializeField] private PauseMenuPresenter _pauseMenuPresenter;
        [SerializeField] private ScarecrowMenuPresenter _scareCrowMenuPresenter;
        [SerializeField] private StorageMenuPresenter _storageMenuPresenter;
        [SerializeField] private FreezeScreenEffect _freezeScreenEffect;
        [SerializeField] private HeatData _playerHeat;
        [SerializeField] private GameWatchPresenter _gameWatchPresenter;

        private IAppInputSystem _appInputSystem;
        private LevelsController _levelsController;
        private AudioController _audioController;

        public ScarecrowMenuPresenter ScareCrowMenuPresenter => _scareCrowMenuPresenter;
        public StorageMenuPresenter StorageMenuPresenter => _storageMenuPresenter;

        [Inject]
        public void Construct(IAppInputSystem appInputSystem,
            PlayerEntity playerEntity,
            AudioController audioController,
            LevelsController levelsController)
        {
            _levelsController = levelsController;
            levelsController.UiController = this;
            _audioController = audioController;
            _playerHeat = playerEntity.Get<HeatData>();
            _mainMenuPresenter.UIController = this;
            _appInputSystem = appInputSystem;
            _appInputSystem.OnEscapePressed.AddListener(OnEscClicked);
            _appInputSystem.OnInventoryPressed.AddListener(OnInventoryPressed);
        }
        public void OpenScarecrowMenu()
        {
            _audioController.AudioData.SoundTracks.OpenFevoniaInterface.Play();
            _playerHeat.CurrentHeat = _playerHeat.DefaultHeatValue;
            _playerHeat.IsFreezing = false;
            _scareCrowMenuPresenter.gameObject.SetActive(true);
            _scareCrowMenuPresenter.Enable = true;
            _appInputSystem.IsGoNextEnable = true;
            _appInputSystem.OnGoNext.AddListener(_scareCrowMenuPresenter.Dialoge.ShowNext);
            _scareCrowMenuPresenter.Dialoge.ShowFirst();
            _appInputSystem.InventoryIsEnable = false;
            _appInputSystem.PlayerMovingIsEnable = false;
            _appInputSystem.InteractionIsEnable = false;
            _appInputSystem.InventoryMoveIsEnable = true;
        }
        public void CloseScarecrowMenu()
        {
            _audioController.AudioData.SoundTracks.CloseInventory.Play();
            _playerHeat.IsFreezing = true;
            _appInputSystem.OnGoNext.RemoveListener(_scareCrowMenuPresenter.Dialoge.ShowNext);
            _appInputSystem.IsGoNextEnable = false;
            _scareCrowMenuPresenter.Enable = false;
            _scareCrowMenuPresenter.gameObject.SetActive(false);
            _appInputSystem.InventoryIsEnable = true;
            _appInputSystem.InteractionIsEnable = true;
            _appInputSystem.PlayerMovingIsEnable = true;
            _appInputSystem.InventoryMoveIsEnable = false;
        }
        public void OpenStorageMenu(Inventory inventory)
        {
            _audioController.AudioData.SoundTracks.OpenGregoryInterface.Play();
            _playerHeat.CurrentHeat = _playerHeat.DefaultHeatValue;
            _playerHeat.IsFreezing = false;
            _storageMenuPresenter.gameObject.SetActive(true);
            _storageMenuPresenter.SetInventory(inventory);
            _appInputSystem.IsGoNextEnable = true;
            _appInputSystem.OnGoNext.AddListener(_storageMenuPresenter.Dialoge.ShowNext);
            _storageMenuPresenter.Dialoge.ShowFirst();
            _storageMenuPresenter.Enable = true;
            _appInputSystem.InteractionIsEnable = false;
            _appInputSystem.InventoryIsEnable = false;
            _appInputSystem.PlayerMovingIsEnable = false;
            _appInputSystem.InventoryMoveIsEnable = true;
        }
        public void CloseStorageMenu()
        {
            _audioController.AudioData.SoundTracks.CloseInventory.Play();
            _playerHeat.IsFreezing = true;
            _appInputSystem.OnGoNext.RemoveListener(_storageMenuPresenter.Dialoge.ShowNext);
            _appInputSystem.IsGoNextEnable = false;
            _storageMenuPresenter.Enable = false;
            _storageMenuPresenter.gameObject.SetActive(false);
            _appInputSystem.InventoryIsEnable = true;
            _appInputSystem.InteractionIsEnable = true;
            _appInputSystem.PlayerMovingIsEnable = true;
            _appInputSystem.InventoryMoveIsEnable = false;
        }
        public void OpenMainMenu()
        {
            _audioController.PlayAudioSource(_audioController.AudioData.CycleTracks.MainMenuMusic);
            _mainMenuPresenter.gameObject.SetActive(true);
            _pauseMenuPresenter.gameObject.SetActive(false);
            _freezeScreenEffect.gameObject.SetActive(false);
        }
        public void CloseMainMenu()
        {
            _mainMenuPresenter.gameObject.SetActive(false);
        }
        public void ShowFreezeEffect()
            => _freezeScreenEffect.gameObject.SetActive(true);

        public void OnEscClicked()
        {
            if (_inventoryPresenter.gameObject.activeSelf)
            {
                CloseInventory();
                return;
            }
            if (_scareCrowMenuPresenter.gameObject.activeSelf)
            {
                CloseScarecrowMenu();
                return;
            }
            if (_storageMenuPresenter.gameObject.activeSelf)
            {
                CloseStorageMenu();
                return;
            }
            if (_pauseMenuPresenter.gameObject.activeSelf)
                ClosePausePanel();
            else OpenPausePanel();
        }
        public void CloseCurrentOpenedGamePanel()
        {
            if (_inventoryPresenter.gameObject.activeSelf)
                CloseInventory();
            if (_scareCrowMenuPresenter.gameObject.activeSelf)
                CloseScarecrowMenu();
            if (_storageMenuPresenter.gameObject.activeSelf)
                CloseStorageMenu();
            if (_pauseMenuPresenter.gameObject.activeSelf)
                ClosePausePanel();
        }
        private void OpenPausePanel()
        {
            Debug.Log("открылась пауза");
            _appInputSystem.InteractionIsEnable = false;
            _appInputSystem.InventoryIsEnable = false;
            _appInputSystem.PlayerMovingIsEnable = false;
            _pauseMenuPresenter.gameObject.SetActive(true);
        }
        private void ClosePausePanel()
        {
            if (_levelsController.CurrentLevel == 0)
            {
                _appInputSystem.InteractionIsEnable = false;
                _appInputSystem.InventoryIsEnable = false;
            }
            else
            {
                _appInputSystem.InteractionIsEnable = true;
                _appInputSystem.InventoryIsEnable = true;
            }
            _appInputSystem.PlayerMovingIsEnable = true;
            _pauseMenuPresenter.gameObject.SetActive(false);
        }
        private void OnInventoryPressed()
        {
            if (_inventoryPresenter.gameObject.activeSelf)
                CloseInventory();
            else OpenInventory();
        }
        private void CloseInventory()
        {
            _gameWatchPresenter.gameObject.SetActive(true);
            _audioController.AudioData.SoundTracks.CloseInventory.Play();
            _appInputSystem.IsGoNextEnable = false;
            _inventoryPresenter.Enable = false;
            _appInputSystem.InteractionIsEnable = true;
            _inventoryPresenter.gameObject.SetActive(false);
            _appInputSystem.PlayerMovingIsEnable = true;
            _appInputSystem.InventoryMoveIsEnable = false;
        }
        private void OpenInventory()
        {
            _gameWatchPresenter.gameObject.SetActive(false);
            _audioController.AudioData.SoundTracks.CloseInventory.Play();
            _inventoryPresenter.gameObject.SetActive(true);
            _appInputSystem.IsGoNextEnable = true;
            _inventoryPresenter.Enable = true;
            _appInputSystem.InteractionIsEnable = false;
            _appInputSystem.PlayerMovingIsEnable = false;
            _appInputSystem.InventoryMoveIsEnable = true;
        }
    }
}