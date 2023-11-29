using System;
using UnityEngine;

namespace App.Architecture.AppData
{
    [Serializable]
    public class LevelsConfigurations
    {
        [SerializeField] private LevelConfiguration[] _levelConfiguration;

        public LevelConfiguration this[int index]
            => _levelConfiguration[index];
        public int Count => _levelConfiguration.Length;

    }
}