using App.Architecture.AppData;
using App.Content.Player;
using Cysharp.Threading.Tasks;
using System;
using TMPro;
using UnityEngine;
using VContainer;

namespace App.Content.UI.InventoryUI
{
    public sealed class InventoryPresenter : MonoBehaviour
    {
        private const string TIME_FORMAT = "00";
        [SerializeField] private CellPresenter[] _inventoryCells;
        [SerializeField] private TextMeshProUGUI _trustText;
        [SerializeField] private TextMeshProUGUI _timer;

        private Inventory _playerInventory;
        private IconsConfiguration _iconsConfiguration;
        private bool _enable;
        private VillageTrustSystem _villageTrustSystem;
        private LevelTimer _levelTimer;

        public bool Enable
        {
            get => _enable;
            set
            {
                _enable = value;
                if (value)
                {
                    UpdatePlayerInventoryCells(_playerInventory.Cells);
                    DefferedSubscribes()
                        .Forget();
                    _trustText.text = $"Доверие: {_villageTrustSystem.Trust}";
                }
                else
                {
                    _playerInventory.OnInventoryUpdated.RemoveListener(UpdatePlayerInventoryCells);
                }
            }
        }

        [Inject]
        public void Construct(PlayerEntity playerEntity,
            Configuration configurations,
            LevelTimer levelTimer,
            VillageTrustSystem villageTrustSystem)
        {
            _villageTrustSystem = villageTrustSystem;
            _levelTimer = levelTimer;
            levelTimer.OnTimeHasChanged.AddListener(ShowTime);
            _playerInventory = playerEntity.Get<Inventory>();
            _iconsConfiguration = configurations.IconsConfiguration;
        }

        private void ShowTime(float obj)
        {
            int value = (int)obj;
            int seconds = value % 60;
            int minutes = (value - seconds) / 60;
            _timer.text = $"{minutes.ToString(TIME_FORMAT)}:{seconds.ToString(TIME_FORMAT)}";
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