using System;
using UnityEngine;

namespace App.Architecture.AppData
{
    [Serializable]
    public sealed class ItemCount
    {
        [SerializeField] private Key _name;
        [SerializeField] private int _count;

        public Key Key
            => _name;
        public int Count
            => _count;
    }
}