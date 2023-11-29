using System;
using UnityEngine;

namespace App.Architecture.AppData
{
    [Serializable]
    public sealed class TrustLevels
    {
        [SerializeField] private TrustLevel[] _values;

        public TrustLevel this[int value]
            => _values[value];
        public int Length
            => _values.Length;
        public TrustLevel[] Values => _values;
    }
}