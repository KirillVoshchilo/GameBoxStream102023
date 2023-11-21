using System;
using UnityEngine;

namespace App.Architecture.AppData
{
    [Serializable]
    public class StorageItemConfiguration
    {
        [SerializeField] private Key _name;
        [SerializeField] private int _count;
        [SerializeField] private int _trustRequirement;

        public Key Key
            => _name;
        public int Count
            => _count;
        public int TrustRequirement 
            => _trustRequirement; 
    }
}