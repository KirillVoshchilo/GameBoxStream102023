using App.Simples;
using System;
using UnityEngine;

namespace App.Architecture.AppData
{
    [Serializable]
    public sealed class Icon
    {
        [SerializeField] private SSOKey _name;
        [SerializeField] private Sprite _icon;

        public SSOKey Name => _name;
        public Sprite Value => _icon;
    }
}