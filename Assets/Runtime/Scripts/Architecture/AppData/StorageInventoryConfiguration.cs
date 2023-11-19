using System;
using UnityEngine;

namespace App.Architecture.AppData
{
    [Serializable]
    public class StorageInventoryConfiguration
    {
        [SerializeField] private StorageItemConfiguration[] _items;

        public StorageItemConfiguration[] Items => _items;
        public StorageItemConfiguration this[Key key]
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