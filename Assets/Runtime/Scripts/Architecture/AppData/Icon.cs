using System;
using UnityEngine;

namespace App.Architecture.AppData
{
    [Serializable]
    public sealed class Icon
    {
        [SerializeField] private Key _name;
        [SerializeField] private Sprite _icon;

        public Key Name
            => _name;
        public Sprite Value
            => _icon;
    }
}