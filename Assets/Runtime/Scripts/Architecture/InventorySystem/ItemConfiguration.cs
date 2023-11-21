using App.Architecture.AppData;
using System;
using UnityEngine;

[Serializable]
public struct ItemConfiguration
{
    [SerializeField] private Key _key;
    [SerializeField] private int _maxCellsForItem;
    [SerializeField] private int _maxCountInCell;

    public readonly Key Key
        => _key;
    public readonly int MaxCountInCell
        => _maxCountInCell;
    public readonly int MaxCellsForfItem
    => _maxCellsForItem;
}