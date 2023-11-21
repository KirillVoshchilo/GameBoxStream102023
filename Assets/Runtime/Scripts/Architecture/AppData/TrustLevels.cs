using System;
using UnityEngine;

namespace App.Architecture.AppData
{
    [Serializable]
    public sealed class TrustLevels
    {
        [SerializeField] private float[] _trustLevels;

        public float this[int value]
            => _trustLevels[value];

        public int Length
            => _trustLevels.Length;
    }
}