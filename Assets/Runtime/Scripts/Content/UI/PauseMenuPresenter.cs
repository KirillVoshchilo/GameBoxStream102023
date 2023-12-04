using App.Architecture;
using App.Architecture.AppInput;
using App.Content.Player;
using App.Logic;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace App.Content.UI
{
    public sealed class PauseMenuPresenter : MonoBehaviour
    {
        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _closeAppButton;

        private IAppInputSystem _appInputSystem;
        private UIController _uiController;
        private Inventory _playerInventory;
        private PlayerEntity _playerEntity;
        private LevelLoaderSystem _levelLoader;
        private DefeatController _defeatController;
        private BonfireFactory _bonusFactory;

        [Inject]
        public void Construct(IAppInputSystem appInputSystem,
            LevelLoaderSystem levelLoader,
            PlayerEntity playerEntity,
            UIController uiController,
            DefeatController defeatController,
            BonfireFactory bonfireFactory)
        {
            _bonusFactory = bonfireFactory;
            _defeatController = defeatController;
            _uiController = uiController;
            _playerInventory = playerEntity.Get<Inventory>();
            _playerEntity = playerEntity;
            _levelLoader = levelLoader;
            _appInputSystem = appInputSystem;
            _closeAppButton.onClick.AddListener(OnEndGameClicked);
            _continueButton.onClick.AddListener(OnContinueClicked);
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
            _playerInventory.Clear();
            _playerEntity.GetComponent<Rigidbody>().useGravity = false;
            _levelLoader.UnloadScene(LevelLoaderSystem.FIRST_LEVEL);
            _uiController.OpenMainMenu();
            _uiController.CloseWinCanvas();
        }
    }
}