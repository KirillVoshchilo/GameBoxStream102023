using App.Simples;
using System;
using UnityEngine;

namespace App.Architecture.AppData
{
    [Serializable]
    public sealed class ItemCount
    {
        [SerializeField] private SSOKey _name;
        [SerializeField] private int _count;

        public SSOKey Key => _name;
        public int Count => _count;
    }
}