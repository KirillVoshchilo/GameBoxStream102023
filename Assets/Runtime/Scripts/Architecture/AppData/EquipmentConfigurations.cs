using System;
using UnityEngine;

namespace App.Architecture.AppData
{
    [Serializable]
    public class EquipmentConfigurations
    {
        [SerializeField] private ItemCategory[] _itemCategories;
        [SerializeField] private AxeProperty[] _axeProperties;
        [SerializeField] private Key[] _changableCategories;

        public AxeProperty[] AxeProperties => _axeProperties;
        public ItemCategory[] ItemCategories => _itemCategories;
        public Key[] ChangableCategories => _changableCategories;

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
        public Key[] GetChildInCategory(Key parent)
        {
            int count = _itemCategories.Length;
            for (int i = 0; i < count; i++)
            {
                if (_itemCategories[i].Category == parent)
                    return _itemCategories[i].Keys;
            }
            return null;
        }
        public Key GetUpperCategory(Key key)
        {
            int count = _itemCategories.Length;
            for (int i = 0; i < count; i++)
            {
                if (_itemCategories[i].HasKey(key))
                    return _itemCategories[i].Category;
            }
            return null;
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