using System;
using System.Linq;
using UnityEngine;

namespace App.Architecture.AppData
{
    [Serializable]
    public class ItemCategory
    {
        [SerializeField] private Key _category;
        [SerializeField] private Key[] _keys;

        public Key Category => _category;
        public Key[] Keys => _keys;

        public bool HasKey(Key key)
            => _keys.Contains(key);
    }
}