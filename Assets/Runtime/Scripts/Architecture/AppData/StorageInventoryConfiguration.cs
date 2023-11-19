using System;
using UnityEngine;

namespace App.Architecture.AppData
{
    [Serializable]
    public class StorageInventoryConfiguration
    {
        [SerializeField] private StorageItemConfiguration[] _items;

        public StorageItemConfiguration[] Items
            => _items;
    }
}