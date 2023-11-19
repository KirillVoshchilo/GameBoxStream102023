using App.Architecture;
using App.Architecture.AppData;
using App.Content.Player;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace App.Content.UI.InventoryUI
{
    public sealed class InventoryPresenter : MonoBehaviour
    {
        [SerializeField] private InventoryItemPresenter _prefab;
        [SerializeField] private Transform _content;

        private readonly List<InventoryItemPresenter> _itemsList = new();
        private Inventory _playerInventory;
        private IconsConfiguration _iconsConfiguration;

        [Inject]
        public void Construct(PlayerEntity playerEntity,
            Configuration configurations)
        {
            _playerInventory = playerEntity.Get<Inventory>();
            _iconsConfiguration = configurations.IconsConfiguration;
        }
        public void FillWithItems()
        {
            foreach (Cell cell in _playerInventory.Cells)
            {
                if (cell == null)
                    continue;
                InventoryItemPresenter presenter = Instantiate(_prefab, _content);
                presenter.Count = cell.Count;
                presenter.Name = cell.Key.Value;
                presenter.Icon = _iconsConfiguration[cell.Key];
                _itemsList.Add(presenter);
            }
        }
        public void Clear()
        {
            InventoryItemPresenter[] array = _itemsList.ToArray();
            int count = array.Length;
            for (int i = 0; i < count; i++)
                Destroy(array[i].gameObject);
            _itemsList.Clear();
        }
    }
}