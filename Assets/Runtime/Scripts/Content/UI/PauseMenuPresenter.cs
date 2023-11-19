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
        private PlayerInventory _playerInventory;
        private PlayerEntity _playerEntity;
        private LevelLoaderSystem _levelLoader;

        [Inject]
        public void Construct(IAppInputSystem appInputSystem,
            LevelLoaderSystem levelLoader,
            PlayerEntity playerEntity,
            UIController uiController)
        {
            _uiController = uiController;
            _playerInventory = playerEntity.Get<PlayerInventory>();
            _playerEntity = playerEntity;
            _levelLoader = levelLoader;
            _appInputSystem = appInputSystem;
            _closeAppButton.onClick.AddListener(OnCloseAppClicked);
            _continueButton.onClick.AddListener(OnContinueClicked);
        }

        private void OnContinueClicked()
        {
            _appInputSystem.EscapeIsEnable = true;
            _appInputSystem.InventoryIsEnable = true;
            _appInputSystem.PlayerMovingIsEnable = true;
            gameObject.SetActive(false);
        }
        private void OnCloseAppClicked()
        {
            _playerInventory.Clear();
            _playerEntity.GetComponent<Rigidbody>().useGravity = false;
            _levelLoader.UnloadScene(LevelLoaderSystem.FIRST_LEVEL);
            _uiController.OpenMainMenu();
            _uiController.CloseWinCanvas();
        }
    }
}