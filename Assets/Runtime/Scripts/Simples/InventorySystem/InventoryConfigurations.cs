using System;
using UnityEngine;

namespace App.Simples.CellsInventory
{
    [Serializable]
    public sealed class InventoryConfigurations
    {
        [SerializeField] ItemConfiguration[] _itemsConfigurations;

        public int GetCountInCell(SSOKey key)
        {
            int count = _itemsConfigurations.Length;
            for (int i = 0; i < count; i++)
            {
                if (_itemsConfigurations[i].Key == key)
                    return _itemsConfigurations[i].MaxCountInCell;
            }
            return 0;
        }
        public int GetCellsCountForItem(SSOKey key)
        {
            int count = _itemsConfigurations.Length;
            for (int i = 0; i < count; i++)
            {
                if (_itemsConfigurations[i].Key == key)
                    return _itemsConfigurations[i].MaxCellsForfItem;
            }
            return 0;
        }
    }
}