using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace App.Architecture.AppData
{
    [Serializable]
    public class EquipmentConfigurations
    {
        [SerializeField] private ItemCategory[] _itemCategories;
        [SerializeField] private AxeProperty[] _axeProperties;

        public AxeProperty[] AxeProperties => _axeProperties;
        public ItemCategory[] ItemCategories => _itemCategories;

        public bool CheckBelongingToTheCategory(Key part, Key main)
        {
            int count = _itemCategories.Length;
            for (int i = 0; i < count; i++)
            {
                if (_itemCategories[i].Category == main && _itemCategories[i].HasKey(part))
                    return true;
            }
            return false;
        }
        public bool CheckAvailabilityOfTheCategory(Key category)
        {
            int count = _itemCategories.Length;
            for (int i = 0; i < count; i++)
            {
                if (_itemCategories[i].Category == category)
                    return true;
            }
            return false;
        }
    }
}