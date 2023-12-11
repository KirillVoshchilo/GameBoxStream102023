using System;

namespace App.Simples.CellsInventory
{
    public sealed class Cell
    {
        public Action<int> OnCountChanged;

        private readonly SSOKey _itemKey;
        private int _count;

        public SSOKey Key
            => _itemKey;
        public int Count
            => _count;

        public Cell(SSOKey itemKey)
            => _itemKey = itemKey;

        public void AddItem(int count)
        {
            _count += count;
            OnCountChanged?.Invoke(_count);
        }
        public void RemoveItem(int count)
        {
            _count -= count;
            OnCountChanged?.Invoke(_count);
        }
    }
}