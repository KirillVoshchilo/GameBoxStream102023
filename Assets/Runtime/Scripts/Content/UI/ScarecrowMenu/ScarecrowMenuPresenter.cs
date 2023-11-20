﻿using App.Architecture.AppData;
using App.Architecture.AppInput;
using App.Content.Player;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using VContainer;

public class ScarecrowMenuPresenter : MonoBehaviour
{
    [SerializeField] private CellPresenter[] _cellPresenters;
    [SerializeField] private TextMeshProUGUI _currentWoodCount;
    [SerializeField] private TextMeshProUGUI _currentWoodCountShort;
    [SerializeField] private TextMeshProUGUI _woodRequirements;
    [SerializeField] private TextMeshProUGUI _trustText;
    [SerializeField] private DialogueSystem _dialogueSystem;
    [SerializeField] private Key _requirementResource;

    private Inventory _playerInventory;
    private IconsConfiguration _iconsConfiguration;
    private VillageTrustSystem _villageTrustSystem;
    private TrustLevels _trustLevels;
    private IAppInputSystem _appInputSystem;
    private readonly CellPresenter[,] _cellsMatrix = new CellPresenter[3, 3];
    private (int x, int y) _selectionPosition;
    private bool _enable;

    public bool Enable
    {
        get => _enable;
        set
        {
            _enable = value;
            if (value)
            {
                UpdateCells(_playerInventory.Cells);
                PrepareInventoryMatrix();
                SetSelection(0, 0);
                ShowGoalCount(_villageTrustSystem.CurrentTrustLevel);
                ShowWoodQuantity(_villageTrustSystem.Trust);
                DefferedSubscribes()
                    .Forget();
            }
            else
            {
                _villageTrustSystem.OnTrustChanged.RemoveListener(ShowWoodQuantity);
                _villageTrustSystem.OnTrustLevelChanged.RemoveListener(OnTrustLevelChanged);
                _appInputSystem.OnMovedInInventory.RemoveListener(MoveSelectionSelection);
                _appInputSystem.OnInventorySelected.RemoveListener(PushMaterials);
                _playerInventory.OnInventoryUpdated.RemoveListener(UpdateCells);
            }
        }
    }

    private async UniTask DefferedSubscribes()
    {
        await UniTask.NextFrame();
        _villageTrustSystem.OnTrustChanged.AddListener(ShowWoodQuantity);
        _villageTrustSystem.OnTrustLevelChanged.AddListener(OnTrustLevelChanged);
        _appInputSystem.OnMovedInInventory.AddListener(MoveSelectionSelection);
        _appInputSystem.OnInventorySelected.AddListener(PushMaterials);
        _playerInventory.OnInventoryUpdated.AddListener(UpdateCells);
    }
    private void OnTrustLevelChanged(int trustLevel)
    {
        ShowGoalCount(trustLevel);
        ShowWoodQuantity(_villageTrustSystem.Trust);
    }
    private void ShowWoodQuantity(float trust)
    {
        _trustText.text = $"Доверие: {trust}";
        if (_villageTrustSystem.CurrentTrustLevel >= _trustLevels.Length)
        {
            _currentWoodCountShort.text = "0";
            _currentWoodCount.text = $"В наличии {0}";
            return;
        }
        float difference = trust;
        if (_villageTrustSystem.CurrentTrustLevel > 0)
        {
            int previousLevel = Mathf.Clamp(_villageTrustSystem.CurrentTrustLevel - 1, 0, _villageTrustSystem.CurrentTrustLevel);
            difference -= _trustLevels[previousLevel];
        }
        _currentWoodCountShort.text = difference.ToString();
        _currentWoodCount.text = $"В наличии {difference}";
    }

    private void ShowGoalCount(int trustLevel)
    {
        if (trustLevel >= _trustLevels.Length)
            return;
        float goal = _trustLevels[trustLevel];
        if (trustLevel > 0)
        {
            int previousLevel = Mathf.Clamp(trustLevel - 1, 0, trustLevel);
            goal -= _trustLevels[previousLevel];
        }
        _woodRequirements.text = $"{goal} дров.";
    }

    [Inject]
    public void Construct(PlayerEntity playerEntity,
        Configuration configurations,
        VillageTrustSystem villageTrustSystem,
        IAppInputSystem appInputSystem)
    {
        _appInputSystem = appInputSystem;
        _villageTrustSystem = villageTrustSystem;
        _playerInventory = playerEntity.Get<Inventory>();
        _trustLevels = configurations.TrustLevels;
        _iconsConfiguration = configurations.IconsConfiguration;
    }

    private void PushMaterials()
    {
        Cell cell = _cellsMatrix[_selectionPosition.y, _selectionPosition.x].Cell;
        if (cell == null)
            return;
        if (_requirementResource != cell.Key)
            return;
        int cellIndex = _selectionPosition.y * 3 + _selectionPosition.x;
        int toRemove = 0;
        if (_villageTrustSystem.CurrentTrustLevel >= _trustLevels.Length)
        {
            toRemove = cell.Count;
        }
        else
        {
            toRemove = GetRequiredWood();
            toRemove = Mathf.Clamp(toRemove, 0, cell.Count);
        }
        _villageTrustSystem.AddTrust(toRemove);
        _playerInventory.RemoveItemFromCell(cell.Key, toRemove, cellIndex);
    }

    private int GetRequiredWood()
    {
        int currentLevel = _villageTrustSystem.CurrentTrustLevel;
        return (int)(_trustLevels[currentLevel] - _villageTrustSystem.Trust);
    }
    public void Clear()
    {
        int count = _cellPresenters.Length;
        for (int i = 0; i < count; i++)
            _cellPresenters[i].Clear();
    }

    private void PrepareInventoryMatrix()
    {
        int count = _cellPresenters.Length;
        int j = 0;
        int k = 0;
        for (int i = 0; i < count; i++)
        {
            _cellsMatrix[j, k] = _cellPresenters[i];
            k++;
            if (k >= 3)
            {
                j++;
                k = 0;
            }
        }
    }
    private void MoveSelectionSelection(Vector2 vector)
    {
        if (vector.x > 0 && _selectionPosition.x < 2)
            SetSelection(_selectionPosition.x + 1, _selectionPosition.y);
        if (vector.x < 0 && _selectionPosition.x > 0)
            SetSelection(_selectionPosition.x - 1, _selectionPosition.y);
        if (vector.y > 0 && _selectionPosition.y > 0)
            SetSelection(_selectionPosition.x, _selectionPosition.y - 1);
        if (vector.y < 0 && _selectionPosition.y < 2)
            SetSelection(_selectionPosition.x, _selectionPosition.y + 1);
    }
    private void SetSelection(int x, int y)
    {
        _cellsMatrix[_selectionPosition.y, _selectionPosition.x].Highlighter.TurnOffHighlight();
        _selectionPosition = (x, y);
        _cellsMatrix[y, x].Highlighter.Highlight();
    }
    private void UpdateCells(Cell[] obj)
    {
        int count = obj.Length;
        for (int i = 0; i < count; i++)
        {
            if (obj[i] != null)
            {
                _cellPresenters[i].Cell = obj[i];
                _cellPresenters[i].SetCount(obj[i].Count);
                _cellPresenters[i].SetSprite(_iconsConfiguration[obj[i].Key]);
            }
            else
            {
                _cellPresenters[i].Clear();
            }
        }
    }
}