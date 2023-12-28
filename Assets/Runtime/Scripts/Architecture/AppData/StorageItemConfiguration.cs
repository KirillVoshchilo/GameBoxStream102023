using App.Simples;
using System;
using UnityEngine;

namespace App.Architecture.AppData
{
    [Serializable]
    public sealed class StorageItemConfiguration
    {
        [SerializeField] private SSOKey _name;
        [SerializeField] private int _count;
        [SerializeField] private int _trustRequirement;
        [SerializeField] private int _levelRequirement;

        public SSOKey Key => _name;
        public int Count => _count;
        public int TrustRequirement => _trustRequirement;
        public int LevelRequirement => _levelRequirement;
    }
}