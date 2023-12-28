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
        private EndLevelController _endLevelController;
        private LevelsController _levelsController;
        private BonfireFactory _bonfireFactory;

        [Inject]
        public void Construct(IAppInputSystem appInputSystem,
            PlayerEntity playerEntity,
            UIController uiController,
            EndLevelController endLevelController,
            BonfireFactory bonfireFactory,
            LevelsController levelsController)
        {
            _closeTipsButton.onClick.AddListener(OnCloseTipsClicked);
            _openTipsButton.onClick.AddListener(OnOpenTipsClicked);
            _levelsController = levelsController;
            _bonfireFactory = bonfireFactory;
            _endLevelController = endLevelController;
            _uiController = uiController;
            _playerInventory = playerEntity.Get<Inventory>();
            _playerEntity = playerEntity;
            _appInputSystem = appInputSystem;
            _closeAppButton.onClick.AddListener(OnEndGameClicked);
            _continueButton.onClick.AddListener(OnContinueClicked);
        }
        public void CloseTIpsPanel()
            => _tipsPanel.SetActive(false);

        private void OnOpenTipsClicked()
            => _tipsPanel.SetActive(true);
        private void OnCloseTipsClicked()
            => _tipsPanel.SetActive(false);

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
            _levelsController.EndCurrentLevel();
            _bonfireFactory.ClearAll();
            _uiController.CloseCurrentOpenedGamePanel();
            _playerInventory.Clear();
            _playerEntity.GetComponent<Rigidbody>().useGravity = false;
            _uiController.OpenMainMenu();
        }
    }
}