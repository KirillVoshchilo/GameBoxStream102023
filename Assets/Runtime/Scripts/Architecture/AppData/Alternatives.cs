using System;
using UnityEngine;

namespace App.Architecture.AppData
{
    [Serializable]
    public sealed class Alternatives
    {
        [SerializeField] private ItemCount[] _requirements;

        public ItemCount[] Requirements => _requirements;
    }
}