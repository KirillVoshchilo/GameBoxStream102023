using App.Architecture;
using App.Architecture.AppData;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace App.Content.UI.Inventory
{
    public sealed class InventoryPresenter : MonoBehaviour
    {
        [SerializeField] private InventoryItemPresenter _prefab;
        [SerializeField] private Transform _content;

        private readonly List<InventoryItemPresenter> _itemsList = new();
        private PlayerInventorySystem _playerInventorySystem;
        private IconsConfiguration _iconsConfiguration;

        [Inject]
        public void Construct(PlayerInventorySystem playerInventorySystem,
            Configuration configurations)
        {
            _playerInventorySystem = playerInventorySystem;
            _iconsConfiguration = configurations.IconsConfiguration;
        }
        public void FillWithItems()
        {
            foreach ((Key name, int count) item in _playerInventorySystem.AllItems)
            {
                InventoryItemPresenter presenter = Instantiate(_prefab, _content);
                presenter.Count = item.count;
                presenter.Name = item.name.Value;
                presenter.Icon = _iconsConfiguration[item.name];
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