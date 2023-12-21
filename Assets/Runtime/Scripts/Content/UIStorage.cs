using App.Content.UI;
using UnityEngine;

namespace App.Content
{
    public sealed class UIStorage
    {
        [SerializeField] private InventoryPresenter _inventoryPresenter;
        [SerializeField] private MainMenuPresenter _mainMenuPresenter;
        [SerializeField] private PauseMenuPresenter _pauseMenuPresenter;
        [SerializeField] private FevroniaMenuPresenter _scareCrowMenuPresenter;
        [SerializeField] private GrigoryMenuPresenter _storageMenuPresenter;
        [SerializeField] private FreezeScreenEffect _freezeScreenEffect;
        [SerializeField] private GameWatchPresenter _gameWatchPresenter;

        public InventoryPresenter InventoryPresenter => _inventoryPresenter;
        public MainMenuPresenter MainMenuPresenter => _mainMenuPresenter;
        public PauseMenuPresenter PauseMenuPresenter => _pauseMenuPresenter;
        public FevroniaMenuPresenter ScareCrowMenuPresenter => _scareCrowMenuPresenter;
        public GrigoryMenuPresenter StorageMenuPresenter => _storageMenuPresenter;
        public FreezeScreenEffect FreezeScreenEffect => _freezeScreenEffect;
        public GameWatchPresenter GameWatchPresenter => _gameWatchPresenter;
    }
}