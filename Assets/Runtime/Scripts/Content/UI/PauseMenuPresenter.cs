using App.Architecture;
using App.Architecture.AppInput;
using App.Content.Bonfire;
using App.Content.Player;
using App.Logic;
using App.Simples.CellsInventory;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace App.Content.UI
{
    public sealed class PauseMenuPresenter : MonoBehaviour
    {
        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _closeAppButton;
        [SerializeField] private Button _closeTipsButton;
        [SerializeField] private Button _openTipsButton;
        [SerializeField] private GameObject _tipsPanel;

        private IAppInputSystem _appInputSystem;
        private UIController _uiController;
        private Inventory _playerInventory;
        private PlayerEntity _playerEntity;
        private LevelTimer _levelTimer;
        private DefeatController _defeatController;
        private LevelsController _levelsController;
        private BonfireFactory _bonusFactory;

        [Inject]
        public void Construct(IAppInputSystem appInputSystem,
            PlayerEntity playerEntity,
            UIController uiController,
            DefeatController defeatController,
            BonfireFactory bonfireFactory,
            LevelsController levelsController,
            LevelTimer levelTimer)
        {
            _closeTipsButton.onClick.AddListener(OnCloseTipsClicked);
            _openTipsButton.onClick.AddListener(OnOpenTipsClicked);
            _levelTimer = levelTimer;
            _levelsController = levelsController;
            _bonusFactory = bonfireFactory;
            _defeatController = defeatController;
            _uiController = uiController;
            _playerInventory = playerEntity.Get<Inventory>();
            _playerEntity = playerEntity;
            _appInputSystem = appInputSystem;
            _closeAppButton.onClick.AddListener(OnEndGameClicked);
            _continueButton.onClick.AddListener(OnContinueClicked);
        }
        public void CloseTIpsPanel()
        {
            _tipsPanel.SetActive(false);
        }

        private void OnOpenTipsClicked()
        {
            _tipsPanel.SetActive(true);
        }
        private void OnCloseTipsClicked()
        {
            _tipsPanel.SetActive(false);
        }

        private void OnContinueClicked()
        {
            _appInputSystem.EscapeIsEnable = true;
            _appInputSystem.InventoryIsEnable = true;
            _appInputSystem.PlayerMovingIsEnable = true;
            _appInputSystem.InteractionIsEnable = true;
            gameObject.SetActive(false);
        }
        private void OnEndGameClicked()
        {
            _bonusFactory.ClearAll();
            _defeatController.IsEnable = false;
            _levelsController.ResetLevelController();
            _playerInventory.Clear();
            _levelTimer.StopTimer();
            _playerEntity.GetComponent<Rigidbody>().useGravity = false;
            _uiController.OpenMainMenu();
        }
    }
}