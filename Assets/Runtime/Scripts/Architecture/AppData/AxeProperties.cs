using System;
using UnityEngine;

namespace App.Architecture.AppData
{
    [Serializable]
    public class AxeProperty
    {
        [SerializeField] private Key _axe;
        [SerializeField] private int _extraCut;

        public Key Axe => _axe;
        public int ExtraCut  => _extraCut; 
    }
}