using App.Architecture.AppData;
using App.Content.Player;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using VContainer;

namespace App.Content.UI.InventoryUI
{
    public sealed class InventoryPresenter : MonoBehaviour
    {
        [SerializeField] private CellPresenter[] _inventoryCells;
        [SerializeField] private TextMeshProUGUI _trustText;
        [SerializeField] private TextMeshProUGUI _timer;

        private Inventory _playerInventory;
        private IconsConfiguration _iconsConfiguration;
        private bool _enable;

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
                }
                else
                {
                    _playerInventory.OnInventoryUpdated.RemoveListener(UpdatePlayerInventoryCells);
                }
            }
        }

        [Inject]
        public void Construct(PlayerEntity playerEntity,
            Configuration configurations)
        {
            _playerInventory = playerEntity.Get<Inventory>();
            _iconsConfiguration = configurations.IconsConfiguration;
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