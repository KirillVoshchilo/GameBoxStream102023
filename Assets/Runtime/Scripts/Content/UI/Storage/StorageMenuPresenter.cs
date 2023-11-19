using App.Architecture.AppData;
using App.Architecture.AppInput;
using App.Content.Player;
using Cysharp.Threading.Tasks;
using System;
using TMPro;
using UnityEngine;
using VContainer;

public class StorageMenuPresenter : MonoBehaviour
{
    [SerializeField] private CellPresenter[] _storageCells;
    [SerializeField] private CellPresenter[] _inventoryCells;
    [SerializeField] private TextMeshProUGUI _trustText;
    [SerializeField] private DialogueSystem _dialogueSystem;

    private Inventory _playerInventory;
    private IconsConfiguration _iconsConfiguration;
    private VillageTrustSystem _villageTrustSystem;
    private IAppInputSystem _appInputSystem;
    private readonly CellPresenter[,] _inventoryMatrix = new CellPresenter[3, 3];
    private readonly CellPresenter[,] _storageMatrix = new CellPresenter[3, 3];
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
                UpdatePlayerInventoryCells(_playerInventory.Cells);
                PrepareInventoryMatrix();
                SetSelection(0, 0);
                ShowTrustQuantity(_villageTrustSystem.Trust);
                DefferedSubscribes()
                    .Forget();
            }
            else
            {
                _villageTrustSystem.OnTrustChanged.RemoveListener(ShowTrustQuantity);
                _villageTrustSystem.OnTrustLevelChanged.RemoveListener(OnTrustLevelChanged);
                _appInputSystem.OnMovedInInventory.RemoveListener(MoveSelectionSelection);
                _appInputSystem.OnInventorySelected.RemoveListener(OnInventorySelect);
                _playerInventory.OnInventoryUpdated.RemoveListener(UpdatePlayerInventoryCells);
            }
        }
    }

    private void OnInventorySelect() => throw new NotImplementedException();

    [Inject]
    public void Construct(PlayerEntity playerEntity,
        Configuration configurations,
        VillageTrustSystem villageTrustSystem,
        IAppInputSystem appInputSystem)
    {
        _appInputSystem = appInputSystem;
        _villageTrustSystem = villageTrustSystem;
        _playerInventory = playerEntity.Get<Inventory>();
        _iconsConfiguration = configurations.IconsConfiguration;
    }
    public void Clear()
    {
        int count = _storageCells.Length;
        for (int i = 0; i < count; i++)
            _storageCells[i].Clear();
    }

    private async UniTask DefferedSubscribes()
    {
        await UniTask.NextFrame();
        _villageTrustSystem.OnTrustChanged.AddListener(ShowTrustQuantity);
        _villageTrustSystem.OnTrustLevelChanged.AddListener(OnTrustLevelChanged);
        _appInputSystem.OnMovedInInventory.AddListener(MoveSelectionSelection);
        _appInputSystem.OnInventorySelected.AddListener(OnInventorySelect);
        _playerInventory.OnInventoryUpdated.AddListener(UpdatePlayerInventoryCells);
    }
    private void OnTrustLevelChanged(int trustLevel)
    {
        ShowTrustQuantity(_villageTrustSystem.Trust);
    }
    private void ShowTrustQuantity(float trust)
    {
        _trustText.text = $"Доверие: {trust}";
    }
    private void PrepareInventoryMatrix()
    {
        int count = _inventoryCells.Length;
        int j = 0;
        int k = 0;
        for (int i = 0; i < count; i++)
        {
            _inventoryMatrix[j, k] = _inventoryCells[i];
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
        _inventoryMatrix[_selectionPosition.y, _selectionPosition.x].Highlighter.TurnOffHighlight();
        _selectionPosition = (x, y);
        _inventoryMatrix[y, x].Highlighter.Highlight();
    }
    private void UpdatePlayerInventoryCells(Cell[] obj)
    {
        int count = obj.Length;
        for (int i = 0; i < count; i++)
        {
            if (obj[i] != null)
            {
                _inventoryCells[i].Cell = obj[i];
                _inventoryCells[i].SetCount(obj[i].Count);
                _inventoryCells[i].SetSprite(_iconsConfiguration[obj[i].Key]);
            }
            else  _inventoryCells[i].Clear();
        }
    }
}