using System;
using UnityEngine;

namespace App.Simples.CellsInventory
{
    [Serializable]
    public struct ItemConfiguration
    {
        [SerializeField] private SSOKey _key;
        [SerializeField] private int _maxCellsForItem;
        [SerializeField] private int _maxCountInCell;

        public readonly SSOKey Key => _key;
        public readonly int MaxCountInCell => _maxCountInCell;
        public readonly int MaxCellsForfItem => _maxCellsForItem;
    }
}