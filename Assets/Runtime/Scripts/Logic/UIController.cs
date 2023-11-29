using App.Architecture.AppInput;
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
        [SerializeField] private GameObject _winCanvas;
        [SerializeField] private GameObject _defeatCanvas;
        [SerializeField] private FreezeScreenEffect _freezeScreenEffect;

        private IAppInputSystem _appInputSystem;

        public ScarecrowMenuPresenter ScareCrowMenuPresenter => _scareCrowMenuPresenter;
        public StorageMenuPresenter StorageMenuPresenter => _storageMenuPresenter;

        [Inject]
        public void Construct(IAppInputSystem appInputSystem)
        {
            _mainMenuPresenter.UIController = this;
            _appInputSystem = appInputSystem;
            _appInputSystem.OnEscapePressed.AddListener(OnEscClicked);
            _appInputSystem.OnInventoryPressed.AddListener(OnInventoryPressed);
        }
        public void OpenScarecrowMenu()
        {
            _scareCrowMenuPresenter.gameObject.SetActive(true);
            _scareCrowMenuPresenter.Enable = true;
            _appInputSystem.IsGoNextEnable = true;
            _appInputSystem.OnGoNext.AddListener(_scareCrowMenuPresenter.Dialoge.ShowNext);
            _scareCrowMenuPresenter.Dialoge.ShowFirst();
            _appInputSystem.InventoryIsEnable = false;
            _appInputSystem.EscapeIsEnable = true;
            _appInputSystem.PlayerMovingIsEnable = false;
            _appInputSystem.InventoryMoveIsEnable = true;
        }
        public void CloseScarecrowMenu()
        {
            _appInputSystem.OnGoNext.RemoveListener(_scareCrowMenuPresenter.Dialoge.ShowNext);
            _appInputSystem.IsGoNextEnable = false;
            _scareCrowMenuPresenter.Enable = false;
            _scareCrowMenuPresenter.gameObject.SetActive(false);
            _appInputSystem.InventoryIsEnable = true;
            _appInputSystem.EscapeIsEnable = false;
            _appInputSystem.PlayerMovingIsEnable = true;
            _appInputSystem.InventoryMoveIsEnable = false;
        }
        public void OpenStorageMenu(Inventory inventory)
        {
            _storageMenuPresenter.gameObject.SetActive(true);
            _storageMenuPresenter.SetInventory(inventory);
            _appInputSystem.IsGoNextEnable = true;
            _appInputSystem.OnGoNext.AddListener(_storageMenuPresenter.Dialoge.ShowNext);
            _storageMenuPresenter.Dialoge.ShowFirst();
            _storageMenuPresenter.Enable = true;
            _appInputSystem.InventoryIsEnable = false;
            _appInputSystem.EscapeIsEnable = true;
            _appInputSystem.PlayerMovingIsEnable = false;
            _appInputSystem.InventoryMoveIsEnable = true;
        }
        public void CloseStorageMenu()
        {
            _appInputSystem.OnGoNext.RemoveListener(_storageMenuPresenter.Dialoge.ShowNext);
            _appInputSystem.IsGoNextEnable = false;
            _storageMenuPresenter.Enable = false;
            _storageMenuPresenter.gameObject.SetActive(false);
            _appInputSystem.InventoryIsEnable = true;
            _appInputSystem.EscapeIsEnable = false;
            _appInputSystem.PlayerMovingIsEnable = true;
            _appInputSystem.InventoryMoveIsEnable = false;
        }
        public void OpenMainMenu()
        {
            _mainMenuPresenter.gameObject.SetActive(true);
            _pauseMenuPresenter.gameObject.SetActive(false);
            _freezeScreenEffect.gameObject.SetActive(false);
        }
        public void CloseMainMenu()
        {
            _mainMenuPresenter.gameObject.SetActive(false);
        }
        public void CloseWinCanvas()
            => _winCanvas.SetActive(false);
        public void ShowWinCanvas()
            => _winCanvas.SetActive(true);
        public void OpenDefeatCanvas()
            => _defeatCanvas.SetActive(true);
        public void CloseDefeatCanvas()
            => _defeatCanvas.SetActive(false);
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
            _appInputSystem.InventoryIsEnable = false;
            _appInputSystem.PlayerMovingIsEnable = false;
            _pauseMenuPresenter.gameObject.SetActive(true);
        }
        private void ClosePausePanel()
        {
            _appInputSystem.InventoryIsEnable = true;
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
            _appInputSystem.PlayerMovingIsEnable = true;
            _inventoryPresenter.Clear();
            _inventoryPresenter.gameObject.SetActive(false);
            _appInputSystem.PlayerMovingIsEnable = true;
        }
        private void OpenInventory()
        {
            _appInputSystem.EscapeIsEnable = true;
            _inventoryPresenter.gameObject.SetActive(true);
            _inventoryPresenter.FillWithItems();
            _appInputSystem.PlayerMovingIsEnable = false;
        }
    }
}