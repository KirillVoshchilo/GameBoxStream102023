using App.Architecture.AppData;
using App.Simples;
using System;
using System.Collections.Generic;
using System.Reflection;


public sealed class Inventory
{
    private readonly SEvent<Cell[]> _onInventoryUpdated = new();

    private Cell[] _cells;
    private readonly int _size;
    private readonly InventoryConfigurations _inventoryConfigurations;

    public Cell[] Cells => _cells;
    public SEvent<Cell[]> OnInventoryUpdated => _onInventoryUpdated;

    public Inventory(InventoryConfigurations data, int cellsCount)
    {
        _inventoryConfigurations = data;
        _cells = new Cell[cellsCount];
        _size = cellsCount;
    }

    public int GetCount(Key key)
    {
        int resultCount = 0;
        for (int i = 0; i < _size; i++)
        {
            if (_cells[i] == null)
                continue;
            if (_cells[i].Key == key)
                resultCount += _cells[i].Count;
        }
        return resultCount;
    }
    public void AddItem(Key key, int count)
    {
        DefineEmptyCells(key, out List<int> emptyCells, out List<int> suitableCells);
        int remainsToFold = count;
        int maxCountInCell = _inventoryConfigurations.GetCountInCell(key);
        foreach (int index in suitableCells)
        {
            int space = maxCountInCell - _cells[index].Count;
            if (space < remainsToFold)
            {
                _cells[index].AddItem(space);
                remainsToFold -= space;
            }
            else
            {
                _cells[index].AddItem(remainsToFold);
                remainsToFold -= remainsToFold;
            }
            if (remainsToFold == 0)
            {
                _onInventoryUpdated?.Invoke(_cells);
                return;
            }
        }
        foreach (int index in emptyCells)
        {
            _cells[index] = new Cell(key);
            if (maxCountInCell < remainsToFold)
            {
                _cells[index].AddItem(maxCountInCell);
                remainsToFold -= maxCountInCell;
            }
            else
            {
                _cells[index].AddItem(remainsToFold);
                remainsToFold -= remainsToFold;
            }
            if (remainsToFold == 0)
            {
                _onInventoryUpdated?.Invoke(_cells);
                return;
            }
        }
        _onInventoryUpdated?.Invoke(_cells);
    }
    public void RemoveItem(Key key, int count)
    {
        int remainsToRemove = count;
        List<int> cellsToClear = new();
        int cellsCount = _cells.Length;
        for (int i = 0; i < cellsCount; i++)
        {
            if (_cells[i].Key == key)
            {
                if (remainsToRemove > _cells[i].Count)
                {
                    remainsToRemove -= _cells[i].Count;
                    _cells[i].RemoveItem(_cells[i].Count);
                    cellsToClear.Add(i);
                }
                else if (remainsToRemove == _cells[i].Count)
                {
                    remainsToRemove -= _cells[i].Count;
                    _cells[i].RemoveItem(_cells[i].Count);
                    cellsToClear.Add(i);
                    break;
                }
                else if (remainsToRemove < _cells[i].Count)
                {
                    _cells[i].RemoveItem(remainsToRemove);
                    break;
                }
            }
        }
        foreach (int index in cellsToClear)
            _cells[index] = null;
        _onInventoryUpdated?.Invoke(_cells);
    }
    public void RemoveItemFromCell(Key key, int count, int cellIndex)
    {
        _cells[cellIndex].RemoveItem(count);
        if (_cells[cellIndex].Count == 0)
            _cells[cellIndex] = null;
        _onInventoryUpdated?.Invoke(_cells);
    }
    public int CheckSpaceInInventory(Key key)
    {
        int maxCountInCell = _inventoryConfigurations.GetCountInCell(key);
        int spaceCount = 0;
        for (int i = 0; i < _size; i++)
        {
            if (_cells[i] == null)
            {
                spaceCount += maxCountInCell;
                continue;
            }
            if (_cells[i].Key == key)
                spaceCount += maxCountInCell - _cells[i].Count;
        }
        return spaceCount;
    }
    public void Clear()
    {
        int count = _cells.Length;
        _cells = new Cell[count];
    }

    private void DefineEmptyCells(Key key, out List<int> emptyCells, out List<int> suitableCells)
    {
        emptyCells = new();
        suitableCells = new();
        int usedCellsCount = 0;
        int maxCellsCountForItem = _inventoryConfigurations.GetCellsCountForItem(key);
        for (int i = 0; i < _size; i++)
        {
            if (_cells[i] != null && _cells[i].Key == key)
            {
                suitableCells.Add(i);
                usedCellsCount++;
            }
            if (usedCellsCount == maxCellsCountForItem)
                return;
        }
        for (int i = 0; i < _size; i++)
        {
            if (_cells[i] == null)
            {
                emptyCells.Add(i);
                usedCellsCount++;
            }
            if (usedCellsCount == maxCellsCountForItem)
                return;
        }
    }
}