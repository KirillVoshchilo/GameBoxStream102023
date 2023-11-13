using System;
using UnityEngine;

namespace App.Architecture.AppData
{
    [Serializable]
    public sealed class StartInventoryConfiguration
    {
        [SerializeField] private ItemCount[] _items;

        public ItemCount[] Items
            => _items;
    }
}