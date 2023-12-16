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

        private Dictionary<SSOKey, Icon> _iconsDictionary;

        public Sprite this[SSOKey value]
        {
            get
            {
                if (_iconsDictionary == null)
                {
                    _iconsDictionary = new Dictionary<SSOKey, Icon>();
                    foreach (Icon icon in _icons)
                        _iconsDictionary.Add(icon.Name, icon);
                }
                return _iconsDictionary[value].Value;
            }
        }
    }
}