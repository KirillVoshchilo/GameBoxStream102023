using App.Architecture.AppData;
using System;
using UnityEngine;

[Serializable]
public sealed class InventoryConfigurations
{
    [SerializeField] ItemConfiguration[] _itemsConfigurations;

    public int GetCountInCell(Key key)
    {
        int count = _itemsConfigurations.Length;
        for (int i = 0; i < count; i++)
        {
            if (_itemsConfigurations[i].Key == key)
                return _itemsConfigurations[i].MaxCountInCell;
        }
        return 0;
    }
    public int GetCellsCountForItem(Key key)
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