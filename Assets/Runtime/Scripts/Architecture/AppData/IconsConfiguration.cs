using System;
using System.Collections.Generic;
using UnityEngine;

namespace App.Architecture.AppData
{
    [Serializable]
    public sealed class IconsConfiguration
    {
        [SerializeField] private Icon[] _icons;

        private readonly Dictionary<Key, Icon> _iconsDictionary = new();

        public Sprite this[Key value]
            => _iconsDictionary[value].Value;

        public void Construct()
        {
            foreach (Icon icon in _icons)
                _iconsDictionary.Add(icon.Name, icon);
        }
    }
}