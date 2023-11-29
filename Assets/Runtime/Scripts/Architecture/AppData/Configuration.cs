using UnityEngine;

namespace App.Architecture.AppData
{
    [CreateAssetMenu]
    public class Configuration : ScriptableObject
    {
        [SerializeField] private StartInventoryConfiguration _startInventoryConfiguration;
        [SerializeField] private StorageInventoryConfiguration _defaultStorageItems;
        [SerializeField] private ItemsOptions _itemsOptions;
        [SerializeField] private IconsConfiguration _iconsConfiguration;
        [SerializeField] private TrustLevels _trustLevels;
        [SerializeField] private InventoryConfigurations _playerInventoryConfigurations;
        [SerializeField] private InventoryConfigurations _storageInventoryConfigurations;
        [SerializeField] private EquipmentConfigurations _equipmentConfigurations;
        [SerializeField] private LevelsConfigurations _levelsConfigurations;
        [SerializeField] private FinalCutScenes _finalCutScenes;

        public StartInventoryConfiguration StartInventoryConfiguration => _startInventoryConfiguration;
        public ItemsOptions ItemsOptions => _itemsOptions;
        public IconsConfiguration IconsConfiguration => _iconsConfiguration;
        public TrustLevels TrustLevels => _trustLevels;
        public InventoryConfigurations PlayerInventoryConfigurations  => _playerInventoryConfigurations;
        public EquipmentConfigurations EquipmentConfigurations => _equipmentConfigurations; 
        public StorageInventoryConfiguration DefauleStorageItems  => _defaultStorageItems;
        public InventoryConfigurations StorageInventoryConfigurations => _storageInventoryConfigurations;
        public LevelsConfigurations LevelsConfigurations => _levelsConfigurations;
        public FinalCutScenes FinalCutScenes => _finalCutScenes; 

        public void Construct()
            => _iconsConfiguration.Construct();
    }
}