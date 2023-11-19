using UnityEngine;

namespace App.Architecture.AppData
{
    [CreateAssetMenu]
    public class Configuration : ScriptableObject
    {
        [SerializeField] private StartInventoryConfiguration _startInventoryConfiguration;
        [SerializeField] private ItemsOptions _itemsOptions;
        [SerializeField] private IconsConfiguration _iconsConfiguration;
        [SerializeField] private TrustLevels _trustLevels;
        [SerializeField] private InventoryConfigurations _inventoryConfigurations;

        public StartInventoryConfiguration StartInventoryConfiguration => _startInventoryConfiguration;
        public ItemsOptions ItemsOptions => _itemsOptions;
        public IconsConfiguration IconsConfiguration => _iconsConfiguration;
        public TrustLevels TrustLevels => _trustLevels;
        public InventoryConfigurations InventoryConfigurations  => _inventoryConfigurations;

        public void Construct()
            => _iconsConfiguration.Construct();
    }
}