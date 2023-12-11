using App.Simples;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace App.Architecture.AppData
{
    [Serializable]
    public sealed class IconsConfiguration
    {
        [SerializeField] private Icon[] _icons;

        private readonly Dictionary<SSOKey, Icon> _iconsDictionary = new();

        public Sprite this[SSOKey value]
            => _iconsDictionary[value].Value;

        public void Construct()
        {
            foreach (Icon icon in _icons)
                _iconsDictionary.Add(icon.Name, icon);
        }
    }
}