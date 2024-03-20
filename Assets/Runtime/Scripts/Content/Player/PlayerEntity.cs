using App.Architecture.AppData;
using App.Architecture.AppInput;
using App.Content.Bonfire;
using App.Simples.CellsInventory;
using UnityEngine;
using VContainer;

namespace App.Content.Player
{
    public sealed class PlayerEntity : MonoBehaviour, IEntity
    {
        [SerializeField] private PlayerData _playerData;

        private HeatHandler _heatHandler;
        private BonfireBuildHandler _bonfireBuildHandler;
        private PlayerMoveHandler _playerMoveHandler;
        private InteractBonfireHandler _interactBonfireHandler;
        private InteractCharacterHandler _interactCharacterHandler;
        private ChopHandler _chopHandler;
        private EnvironmentObjectsSwitcher _environmentObjectsSwitcher;
        private bool _isEnable;

        public bool IsEnable
        {
            get => _isEnable;
            set
            {
                if (value == _isEnable)
                    return;
                _isEnable = value;
                if (value)
                {
                    _heatHandler.IsEnable = true;
                    _playerMoveHandler.IsEnable = true;
                    _bonfireBuildHandler.IsEnable = true;
                    _chopHandler.IsEnable = true;
                    _interactCharacterHandler.IsEnable = true;
                    _interactBonfireHandler.IsEnable = true;
                    _environmentObjectsSwitcher.IsEnable = true;
                }
                else
                {
                    _heatHandler.IsEnable = false;
                    _playerMoveHandler.IsEnable = false;
                    _bonfireBuildHandler.IsEnable = false;
                    _chopHandler.IsEnable = false;
                    _interactCharacterHandler.IsEnable = false;
                    _interactBonfireHandler.IsEnable = false;
                    _environmentObjectsSwitcher.IsEnable = false;
                }
            }
        }

        private void OnDrawGizmos()
        {
            // Draw a yellow sphere at the transform's position
            if (_playerData.Walker == null)
                return;
            Gizmos.color = new Color(1, 1, 1, 0.3f);
            Gizmos.DrawSphere(_playerData.BonfireTargetPosition.position, _playerData.BuildCheckcolliderSize);
        }

        [Inject]
        public void Construct(IAppInputSystem appInputSystem,
            CamerasStorage camerasStorage,
            Configuration configuration,
            BonfireFactory bonfireFactory)
        {
            bonfireFactory.PlayerEntity = this;
            bonfireFactory.Parent = _playerData.BonfireTargetPosition;
            _playerData.Configuration = configuration;
            _playerData.BonfireFactory = bonfireFactory;
            _playerData.Walker = new(_playerData.Rigidbody);
            _playerData.AppInputSystem = appInputSystem;
            _playerData.MainCameraTransform = camerasStorage.MainCamera.transform;
            _playerData.PlayerInventory = new Inventory(configuration.PlayerInventoryConfigurations, 9);
            _heatHandler = new HeatHandler(_playerData);
            _bonfireBuildHandler = new BonfireBuildHandler(_playerData);
            _playerMoveHandler = new PlayerMoveHandler(_playerData);
            _chopHandler = new ChopHandler(_playerData);
            _interactCharacterHandler = new InteractCharacterHandler(_playerData);
            _interactBonfireHandler = new InteractBonfireHandler(_playerData);
            _environmentObjectsSwitcher = new EnvironmentObjectsSwitcher(_playerData);
        }

        public T Get<T>() where T : class
        {
            if (typeof(T) == typeof(WalkerData))
                return _playerData.Walker as T;
            if (typeof(T) == typeof(Inventory))
                return _playerData.PlayerInventory as T;
            if (typeof(T) == typeof(HeatData))
                return _playerData.HeatData as T;
            return null;
        }
    }
}