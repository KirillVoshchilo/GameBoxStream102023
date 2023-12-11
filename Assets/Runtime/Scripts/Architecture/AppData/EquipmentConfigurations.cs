using App.Simples;
using System;
using UnityEngine;

namespace App.Architecture.AppData
{
    [Serializable]
    public sealed class EquipmentConfigurations
    {
        [SerializeField] private ItemCategory[] _itemCategories;
        [SerializeField] private SSOKey[] _changableCategories;

        public SSOKey[] ChangableCategories => _changableCategories;

        public SSOKey GetUpperCategory(SSOKey key)
        {
            int count = _itemCategories.Length;
            for (int i = 0; i < count; i++)
            {
                if (_itemCategories[i].HasKey(key))
                    return _itemCategories[i].Category;
            }
            return null;
        }
    }
}