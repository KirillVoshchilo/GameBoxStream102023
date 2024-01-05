using App.Architecture;
using App.Architecture.AppData;
using App.Content.Player;
using App.Logic;
using App.Simples.CellsInventory;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace App.Content.UI
{
    public sealed class InventoryPresenter : MonoBehaviour
    {
        private const string TIME_FORMAT = "00";

        [SerializeField] private CellPresenter[] _inventoryCells;
        [SerializeField] private TextMeshProUGUI _trustText;
        [SerializeField] private TextMeshProUGUI _timer;

        private Inventory _playerInventory;
        private IconsConfiguration _iconsConfiguration;
        private bool _isEnable;
        private LevelTimer _levelTimer;
        private LevelsController _levelsController;
        private VillageTrustSystem _villageTrustSystem;

        public bool IsEnable
        {
            get => _isEnable;
            set
            {
                _isEnable = value;
                if (value)
                {
                    _levelTimer.OnTimeHasChanged.AddListener(ShowTime);
                    ShowTime(_levelTimer.CurrenTime);
                    UpdatePlayerInventoryCells(_playerInventory.Cells);
                    DefferedSubscribes()
                        .Forget();
                    _trustText.text = $"Доверие: {_villageTrustSystem.Trust}";
                }
                else
                {
                    _levelTimer.OnTimeHasChanged.RemoveListener(ShowTime);
                    _playerInventory.OnInventoryUpdated.RemoveListener(UpdatePlayerInventoryCells);
                }
            }
        }

        public void Construct(PlayerEntity playerEntity,
            Configuration configurations,
            LevelTimer levelTimer,
            VillageTrustSystem villageTrustSystem,
            LevelsController levelsController)
        {
            _levelTimer = levelTimer;
            _levelsController = levelsController;
            _villageTrustSystem = villageTrustSystem;
            _playerInventory = playerEntity.Get<Inventory>();
            _iconsConfiguration = configurations.IconsConfiguration;
        }

        private void ShowTime(float obj)
        {
            int value = (int)obj;
            int seconds = value % 60;
            int minutes = (value - seconds) / 60;
            _timer.text = $"{_levelsController.CurrentLevel} - {minutes.ToString(TIME_FORMAT)}:{seconds.ToString(TIME_FORMAT)}";
        }
        private async UniTask DefferedSubscribes()
        {
            await UniTask.NextFrame();
            _playerInventory.OnInventoryUpdated.AddListener(UpdatePlayerInventoryCells);
        }
        private void UpdatePlayerInventoryCells(Cell[] obj)
        {
            int count = obj.Length;
            for (int i = 0; i < count; i++)
            {
                if (obj[i] != null)
                {
                    _inventoryCells[i].Cell = obj[i];
                    _inventoryCells[i].SetCount(obj[i].Count);
                    _inventoryCells[i].SetSprite(_iconsConfiguration[obj[i].Key]);
                }
                else _inventoryCells[i].Clear();
            }
        }
    }
}