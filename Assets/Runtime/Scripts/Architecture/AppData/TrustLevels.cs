using System;
using UnityEngine;

namespace App.Architecture.AppData
{
    [Serializable]
    public sealed class TrustLevels
    {
        [SerializeField] private TrustLevel[] _values;

        public int Length => _values.Length;

        public TrustLevel this[int value]
            => _values[value];
    }
}