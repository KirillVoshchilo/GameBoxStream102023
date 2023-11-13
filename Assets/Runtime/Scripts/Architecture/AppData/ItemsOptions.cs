using System;
using UnityEngine;

namespace App.Architecture.AppData
{
    [Serializable]
    public sealed class ItemsOptions
    {
        [SerializeField] private Key[] _singletones;

        public Key[] Singletone
            => _singletones;
    }
}