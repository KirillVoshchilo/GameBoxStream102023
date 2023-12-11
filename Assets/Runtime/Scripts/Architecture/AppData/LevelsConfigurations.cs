using System;
using UnityEngine;

namespace App.Architecture.AppData
{
    [Serializable]
    public sealed class LevelsConfigurations
    {
        [SerializeField] private LevelConfiguration[] _levelConfiguration;

        public int Count => _levelConfiguration.Length;
        
        public LevelConfiguration this[int index]
            => _levelConfiguration[index];
    }
}