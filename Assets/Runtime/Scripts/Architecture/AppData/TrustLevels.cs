using System;
using UnityEngine;

namespace App.Architecture.AppData
{
    [Serializable]
    public sealed class TrustLevels
    {
        [SerializeField] private float[] _values;

        public int Length => _values.Length;

        public float this[int value]
            => _values[value];
    }
}