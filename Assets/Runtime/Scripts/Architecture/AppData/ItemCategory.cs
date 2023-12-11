using App.Simples;
using System;
using System.Linq;
using UnityEngine;

namespace App.Architecture.AppData
{
    [Serializable]
    public sealed class ItemCategory
    {
        [SerializeField] private SSOKey _category;
        [SerializeField] private SSOKey[] _keys;

        public SSOKey Category => _category;

        public bool HasKey(SSOKey key)
            => _keys.Contains(key);
    }
}