using App.Architecture.AppData;
using System;


public sealed class Cell
{
    public Action<int> OnCountChanged;

    private readonly Key _itemKey;
    private int _count;

    public Key Key
        => _itemKey;
    public int Count
        => _count;

    public Cell(Key itemKey)
        => _itemKey = itemKey;

    public void AddItem(int count)
    {
        _count += count;
        OnCountChanged?.Invoke(_count);
    }
    public void RemoveItem(int count)
    {
        _count -= count;
        OnCountChanged?.Invoke(_count);
    }
}