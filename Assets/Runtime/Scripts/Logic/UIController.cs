using App.Architecture.AppData;
using App.Architecture.AppInput;
using App.Content.UI;
using App.Content.UI.InventoryUI;
using App.Content.UI.Shop;
using UnityEngine;
using VContainer;

namespace App.Logic
{
    public sealed class UIController : MonoBehaviour
    {
        [SerializeField] private InventoryPresenter _inventoryPresenter;
        [SerializeField] private MainMenuPresenter _mainMenuPresenter;
        [SerializeField] private ShopPresenter _shopPresenter;
        [SerializeField] private PauseMenuPresenter _pauseMenuPresenter;
        [SerializeField] private ScarecrowMenuPresenter _scareCrowMenuPresenter;
        [SerializeField] private StorageMenuPresenter _storageMenuPresenter;
        [SerializeField] private GameObject _winCanvas;
        [SerializeField] private GameObject _defeatCanvas;
        [SerializeField] private FreezeScreenEffect _freezeScreenEffect;

        private IAppInputSystem _appInputSystem;

        [Inject]
        public void Construct(IAppInputSystem appInputSystem)
        {
            _mainMenuPresenter.UIController = this;
            _appInputSystem = appInputSystem;
            _appInputSystem.OnEscapePressed.AddListener(OnEscapePressed);
            _appInputSystem.OnInventoryPressed.AddListener(OnInventoryPressed);
        }
        public void OpenShop(ItemCost[] products)
        {
            _appInputSystem.InventoryIsEnable = false;
            _appInputSystem.EscapeIsEnable = true;
            _appInputSystem.PlayerMovingIsEnable = false;
            _shopPresenter.gameObject.SetActive(true);
            _shopPresenter.ShowProducts(products);
        }
        public void OpenScarecrowMenu()
        {
            _scareCrowMenuPresenter.gameObject.SetActive(true);
            _scareCrowMenuPresenter.Enable = true;
            _appInputSystem.InventoryIsEnable = false;
            _appInputSystem.EscapeIsEnable = true;
            _appInputSystem.PlayerMovingIsEnable = false;
            _appInputSystem.InventoryMoveIsEnable = true;
        }
        public void CloseScarecrowMenu()
        {
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
            _storageMenuPresenter.Enable = true;
            _appInputSystem.InventoryIsEnable = false;
            _appInputSystem.EscapeIsEnable = true;
            _appInputSystem.PlayerMovingIsEnable = false;
            _appInputSystem.InventoryMoveIsEnable = true;
        }
        public void CloseStorageMenu()
        {
            _storageMenuPresenter.Enable = false;
            _storageMenuPresenter.gameObject.SetActive(false);
            _appInputSystem.InventoryIsEnable = true;
            _appInputSystem.EscapeIsEnable = false;
            _appInputSystem.PlayerMovingIsEnable = true;
            _appInputSystem.InventoryMoveIsEnable = false;
        }
        public void CloseShop()
        {
            _appInputSystem.InventoryIsEnable = true;
            _appInputSystem.PlayerMovingIsEnable = true;
            _appInputSystem.EscapeIsEnable = true;
            _shopPresenter.ClearPanels();
            _shopPresenter.gameObject.SetActive(false);
        }
        public void OpenMainMenu()
        {
            _mainMenuPresenter.gameObject.SetActive(true);
            _pauseMenuPresenter.gameObject.SetActive(false);
            _freezeScreenEffect.gameObject.SetActive(false);
            _appInputSystem.EscapeIsEnable = false;
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

        private void OnEscapePressed()
        {
            if (_inventoryPresenter.gameObject.activeSelf)
            {
                CloseInventory();
                return;
            }
            if (_shopPresenter.gameObject.activeSelf)
            {
                CloseShop();
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
        private void OpenPausePanel()
        {
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
            {
                CloseInventory();
                _appInputSystem.PlayerMovingIsEnable = true;
            }
            else
            {
                OpenInventory();
                _appInputSystem.PlayerMovingIsEnable = false;
            }
        }
        private void CloseInventory()
        {
            _appInputSystem.PlayerMovingIsEnable = true;
            _inventoryPresenter.Clear();
            _inventoryPresenter.gameObject.SetActive(false);
        }
        private void OpenInventory()
        {
            _appInputSystem.EscapeIsEnable = true;
            _inventoryPresenter.gameObject.SetActive(true);
            _inventoryPresenter.FillWithItems();
        }
    }
}