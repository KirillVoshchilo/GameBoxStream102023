using System;
using UnityEngine;

namespace App.Architecture.AppData
{
    [Serializable]
    public class TrustLevel
    {
        [SerializeField] private float _trust;

        public float Trust => _trust;
    }
}