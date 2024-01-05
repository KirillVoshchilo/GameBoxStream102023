using App.Architecture;
using App.Architecture.AppInput;
using App.Architecture.Factories.UI;
using App.Content;
using App.Content.Audio;
using App.Content.Player;
using App.Content.UI;
using App.Simples.CellsInventory;
using UnityEngine;
using VContainer;

namespace App.Logic
{
    public sealed class UIController
    {
        private readonly LevelTimer _levelTimer;
        private readonly HeatData _playerHeat;
        private readonly IAppInputSystem _appInputSystem;
        private readonly AudioStorage _audioController;
        private readonly GameWatchFactory _gameWatchFactory;
        private readonly FevroniaMenuFactory _fevroniaMenuFactory;
        private readonly GrigoryMenuFactory _grigoryMenuFactory;
        private readonly MainMenuFactory _mainmenuFactory;
        private readonly FreezeEffectFactory _freezeEffectFactory;
        private readonly PauseMenuFactory _pauseMenuFactory;
        private readonly InventoryMenuFactory _inventoryMenuFactory;
        private bool _isInteractionEnable;
        private bool _isInventoryEnable;
        private FevroniaMenuPresenter _fevroniaMenuPresenter;
        private GrigoryMenuPresenter _grigoryMenuPresenter;
        private MainMenuPresenter _mainmenuPresenter;
        private FreezeScreenEffect _freezeScreenEffect;
        private PauseMenuPresenter _pauseMenuPresenter;
        private InventoryPresenter _inventoryPresenter;
        private GameWatchPresenter _gameWatchPresenter;

        [Inject]
        public UIController(IAppInputSystem appInputSystem,
            PlayerEntity playerEntity,
            AudioStorage audioController,
            LevelsController levelsController,
            LevelTimer levelTimer,
            FevroniaMenuFactory fevroniaMenuFactory,
            GrigoryMenuFactory grigoryMenuFactory,
            MainMenuFactory mainMenuFactory,
            FreezeEffectFactory freezeEffectFactory,
            PauseMenuFactory pauseMenuFactory,
            InventoryMenuFactory inventoryMenuFactory,
            GameWatchFactory gameWatchFactory)
        {
            _gameWatchFactory = gameWatchFactory;
            _fevroniaMenuFactory = fevroniaMenuFactory;
            _grigoryMenuFactory = grigoryMenuFactory;
            _mainmenuFactory = mainMenuFactory;
            _freezeEffectFactory = freezeEffectFactory;
            _pauseMenuFactory = pauseMenuFactory;
            _inventoryMenuFactory = inventoryMenuFactory;
            _levelTimer = levelTimer;
            levelsController.UiController = this;
            _audioController = audioController;
            _playerHeat = playerEntity.Get<HeatData>();
            _appInputSystem = appInputSystem;
            _appInputSystem.OnEscapePressed.AddListener(OnEscClicked);
            _appInputSystem.OnInventoryPressed.AddListener(OnInventoryPressed);
        }
        public void OpenFevroniaMenu()
        {
            if (_fevroniaMenuPresenter != null)
                return;
            _levelTimer.PauseTimer();
            _audioController.AudioData.SoundTracks.OpenFevoniaInterface.Play();
            _playerHeat.CurrentHeat = _playerHeat.DefaultHeatValue;
            _playerHeat.IsFreezing = false;
            _fevroniaMenuPresenter = _fevroniaMenuFactory.Create();
            _appInputSystem.IsGoNextEnable = true;
            _appInputSystem.OnGoNext.AddListener(_fevroniaMenuPresenter.Dialoge.ShowNext);
            _fevroniaMenuPresenter.Dialoge.OnSlidShowEnded.AddListener(()
                => _appInputSystem.OnGoNext.RemoveListener(_fevroniaMenuPresenter.Dialoge.ShowNext));
            _fevroniaMenuPresenter.Dialoge.ShowFirst();
            _appInputSystem.InventoryIsEnable = false;
            _appInputSystem.PlayerMovingIsEnable = false;
            _appInputSystem.InteractionIsEnable = false;
            _appInputSystem.InventoryMoveIsEnable = true;
        }
        public void CloseFevroniaMenu()
        {
            if (_fevroniaMenuPresenter == null)
                return;
            _levelTimer.ContinueTimer();
            _audioController.AudioData.SoundTracks.CloseInventory.Play();
            _playerHeat.IsFreezing = true;
            _appInputSystem.OnGoNext.RemoveListener(_fevroniaMenuPresenter.Dialoge.ShowNext);
            _appInputSystem.IsGoNextEnable = false;
            _fevroniaMenuFactory.Remove(_fevroniaMenuPresenter);
            _appInputSystem.InventoryIsEnable = true;
            _appInputSystem.InteractionIsEnable = true;
            _appInputSystem.PlayerMovingIsEnable = true;
            _appInputSystem.InventoryMoveIsEnable = false;
        }
        public void OpenGrigoryMenu(Inventory inventory)
        {
            if (_grigoryMenuPresenter != null)
                return;
            _levelTimer.PauseTimer();
            _audioController.AudioData.SoundTracks.OpenGregoryInterface.Play();
            _playerHeat.CurrentHeat = _playerHeat.DefaultHeatValue;
            _playerHeat.IsFreezing = false;
            _grigoryMenuPresenter = _grigoryMenuFactory.Create();
            _grigoryMenuPresenter.SetInventory(inventory);
            _grigoryMenuPresenter.IsEnable = true;
            _appInputSystem.IsGoNextEnable = true;
            _appInputSystem.OnGoNext.AddListener(_grigoryMenuPresenter.Dialoge.ShowNext);
            _grigoryMenuPresenter.Dialoge.OnSlidShowEnded.AddListener(()
                => _appInputSystem.OnGoNext.RemoveListener(_grigoryMenuPresenter.Dialoge.ShowNext));
            _grigoryMenuPresenter.Dialoge.ShowFirst();
            _appInputSystem.InteractionIsEnable = false;
            _appInputSystem.InventoryIsEnable = false;
            _appInputSystem.PlayerMovingIsEnable = false;
            _appInputSystem.InventoryMoveIsEnable = true;
        }
        public void CloseGrigoryMenu()
        {
            if (_grigoryMenuPresenter == null)
                return;
            _levelTimer.ContinueTimer();
            _audioController.AudioData.SoundTracks.CloseInventory.Play();
            _playerHeat.IsFreezing = true;
            _appInputSystem.OnGoNext.RemoveListener(_grigoryMenuPresenter.Dialoge.ShowNext);
            _appInputSystem.IsGoNextEnable = false;
            _grigoryMenuFactory.Remove(_grigoryMenuPresenter);
            _appInputSystem.InventoryIsEnable = true;
            _appInputSystem.InteractionIsEnable = true;
            _appInputSystem.PlayerMovingIsEnable = true;
            _appInputSystem.InventoryMoveIsEnable = false;
        }
        public void OpenMainMenu()
        {
            if (_mainmenuPresenter != null)
                return;
            Cursor.visible = true;
            _audioController.PlayAudioSource(_audioController.AudioData.CycleTracks.MainMenuMusic);
            _mainmenuPresenter = _mainmenuFactory.Create();
            _mainmenuPresenter.UIController = this;
        }
        public void CloseMainMenu()
        {
            if (_mainmenuPresenter == null)
                return;
            Cursor.visible = false;
            _mainmenuFactory.Remove(_mainmenuPresenter);
        }
        public void ShowFreezeEffect()
        {
            if (_freezeScreenEffect != null)
                return;
            _freezeScreenEffect = _freezeEffectFactory.Create();
        }
        public void CloseFreezeEffect()
        {
            if (_freezeScreenEffect == null)
                return;
            _freezeEffectFactory.Remove(_freezeScreenEffect);
        }
        public void OnEscClicked()
        {
            if (_inventoryPresenter != null)
            {
                CloseInventory();
                return;
            }
            if (_grigoryMenuPresenter != null)
            {
                ShowFreezeEffect();
                OpenGameWatch();
                CloseGrigoryMenu();
                return;
            }
            if (_fevroniaMenuPresenter != null)
            {
                ShowFreezeEffect();
                OpenGameWatch();
                CloseFevroniaMenu();   
                return;
            }
            if (_pauseMenuPresenter != null)
                ClosePausePanel();
            else OpenPausePanel();
        }
        public void OpenGameWatch()
        {
            if (_gameWatchPresenter != null)
                return;
            _gameWatchPresenter = _gameWatchFactory.Create();
        }
        public void CloseGameWatch()
        {
            if (_gameWatchPresenter == null)
                return;
            _gameWatchFactory.Remove(_gameWatchPresenter);
        }
        public void ClosePausePanel()
        {
            if (_pauseMenuPresenter == null)
                return;
            _levelTimer.ContinueTimer();
            _appInputSystem.InteractionIsEnable = _isInteractionEnable;
            _appInputSystem.InventoryIsEnable = _isInventoryEnable;
            _playerHeat.IsFreezing = true;
            Cursor.visible = false;
            _pauseMenuFactory.Remove(_pauseMenuPresenter);
            _appInputSystem.PlayerMovingIsEnable = true;
        }

        private void OpenPausePanel()
        {
            if (_pauseMenuPresenter != null)
                return;
            _isInteractionEnable = _appInputSystem.InteractionIsEnable;
            _isInventoryEnable = _appInputSystem.InventoryIsEnable;
            _levelTimer.PauseTimer();
            _playerHeat.IsFreezing = false;
            Cursor.visible = true;
            _appInputSystem.InteractionIsEnable = false;
            _appInputSystem.InventoryIsEnable = false;
            _appInputSystem.PlayerMovingIsEnable = false;
            _pauseMenuPresenter = _pauseMenuFactory.Create();
            _pauseMenuPresenter.UiController = this;
        }
        private void OnInventoryPressed()
        {
            if (_inventoryPresenter != null)
            {
                CloseInventory();
                OpenGameWatch();
            }
            else
            {
                OpenInventory();
                CloseGameWatch();
            }
        }
        private void CloseInventory()
        {
            if (_inventoryPresenter == null)
                return;
            _levelTimer.ContinueTimer();
            _inventoryMenuFactory.Remove(_inventoryPresenter);
            _audioController.AudioData.SoundTracks.CloseInventory.Play();
            _appInputSystem.IsGoNextEnable = false;
            _appInputSystem.InteractionIsEnable = true;
            _appInputSystem.PlayerMovingIsEnable = true;
            _appInputSystem.InventoryMoveIsEnable = false;
        }
        private void OpenInventory()
        {
            if (_inventoryPresenter != null)
                return;
            _levelTimer.PauseTimer();
            _audioController.AudioData.SoundTracks.CloseInventory.Play();
            _inventoryPresenter = _inventoryMenuFactory.Create();
            _appInputSystem.IsGoNextEnable = true;
            _appInputSystem.InteractionIsEnable = false;
            _appInputSystem.PlayerMovingIsEnable = false;
            _appInputSystem.InventoryMoveIsEnable = true;
        }
    }
}