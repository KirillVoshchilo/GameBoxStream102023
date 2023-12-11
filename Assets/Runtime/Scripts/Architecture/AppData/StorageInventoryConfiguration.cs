using App.Simples;
using System;
using UnityEngine;

namespace App.Architecture.AppData
{
    [Serializable]
    public sealed class StorageInventoryConfiguration
    {
        [SerializeField] private StorageItemConfiguration[] _items;

        public StorageItemConfiguration[] Items => _items;
        public StorageItemConfiguration this[SSOKey key]
        {
            get
            {
                foreach (StorageItemConfiguration item in _items)
                {
                    if (item.Key == key)
                        return item;
                }
                return null;
            }
        }
    }
}