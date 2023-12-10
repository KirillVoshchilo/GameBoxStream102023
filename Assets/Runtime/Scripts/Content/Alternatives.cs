using App.Architecture.AppData;
using System;
using UnityEngine;

namespace App.Content
{
    [Serializable]
    public sealed class Alternatives
    {
        [SerializeField] private ItemCount[] _requirements;

        public ItemCount[] Requirements
            => _requirements;
    }
}