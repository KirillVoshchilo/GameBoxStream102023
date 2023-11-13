using App.Architecture.AppData;
using App.Simples;
using System.Collections.Generic;

namespace App.Architecture
{
    public sealed class PlayerInventorySystem
    {
        private readonly Dictionary<Key, int> _inventory = new();
        private readonly SEvent<Key, int> _onInventoryValueChanged = new();

        public SEvent<Key, int> OnInventoryValueChanged
            => _onInventoryValueChanged;
        public List<(Key, int)> AllItems
        {
            get
            {
                List<(Key name, int count)> items = new();
                foreach (KeyValuePair<Key, int> item in _inventory)
                    items.Add((name: item.Key, count: item.Value));
                return items;
            }
        }

        public void Clear()
            => _inventory.Clear();
        public int GetCount(Key key)
        {
            if (_inventory.TryGetValue(key, out int value))
                return value;
            return 0;
        }
        public void AddItem(Key key, int count)
        {
            if (_inventory.TryGetValue(key, out int value))
                _inventory[key] = count + value;
            else _inventory.Add(key, count);
            _onInventoryValueChanged.Invoke(key, _inventory[key]);
        }
        public void RemoveItem(Key key, int count)
        {
            if (_inventory.TryGetValue(key, out int value))
                _inventory[key] = value - count;
            _onInventoryValueChanged.Invoke(key, _inventory[key]);
            if (_inventory[key] == 0)
                _inventory.Remove(key);
        }
    }
}