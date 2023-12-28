using App.Architecture;
using App.Architecture.AppData;
using App.Architecture.AppInput;
using App.Content.Audio;
using App.Content.Player;
using App.Logic;
using App.Simples;
using App.Simples.CellsInventory;
using Cysharp.Threading.Tasks;
using SimpleComponents.UI;
using System;
using System.Linq;
using TMPro;
using UnityEngine;
using VContainer;

namespace App.Content.UI
{
    public sealed class GrigoryMenuPresenter : MonoBehaviour
    {
        [SerializeField] private CellPresenter[] _storageCells;
        [SerializeField] private CellPresenter[] _inventoryCells;
        [SerializeField] private TextMeshProUGUI _trustText;
        [SerializeField] private Transform _dialogeTransform;

        private SCSlideShow _dialoge;
        private Inventory _playerInventory;
        private Inventory _storageInventory;
        private IconsConfiguration _iconsConfiguration;
        private VillageTrustSystem _villageTrustSystem;
        private AudioStorage _audioController;
        private EquipmentConfigurations _equipmentConfigurations;
        private StorageInventoryConfiguration _storageInventoryConfiguration;
        private InventoryConfigurations _playerInventoryConfigurations;
        private LevelsController _levelsController;
        private IAppInputSystem _appInputSystem;
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
                    UpdateStorageInventoryCells(_storageInventory.Cells);
                    PrepareInventoryMatrix();
                    SetSelection(0, 0);
                    OnTrustUpdated(_villageTrustSystem.Trust);
                    DefferedSubscribes()
                        .Forget();
                }
                else
                {
                    _villageTrustSystem.OnTrustChanged.RemoveListener(OnTrustUpdated);
                    _appInputSystem.OnMovedInInventory.RemoveListener(MoveSelectionSelection);
                    _appInputSystem.OnInventorySelected.RemoveListener(OnInventorySelect);
                    _playerInventory.OnInventoryUpdated.RemoveListener(UpdatePlayerInventoryCells);
                    _storageInventory.OnInventoryUpdated.RemoveListener(UpdateStorageInventoryCells);
                }
            }
        }
        public SCSlideShow Dialoge
        {
            get => _dialoge;
            set
            {
                if (value == null)
                    return;
                if (_dialoge != null)
                    Destroy(_dialoge.gameObject);
                _dialoge = Instantiate(value, _dialogeTransform);
                _dialoge.IsLoop = false;
            }
        }

        [Inject]
        public void Construct(PlayerEntity playerEntity,
            Configuration configurations,
            VillageTrustSystem villageTrustSystem,
            IAppInputSystem appInputSystem,
            AudioStorage audioController,
            LevelsController levelsController)
        {
            _levelsController = levelsController;
            _audioController = audioController;
            _equipmentConfigurations = configurations.EquipmentConfigurations;
            _playerInventoryConfigurations = configurations.PlayerInventoryConfigurations;
            _appInputSystem = appInputSystem;
            _villageTrustSystem = villageTrustSystem;
            _playerInventory = playerEntity.Get<Inventory>();
            _iconsConfiguration = configurations.IconsConfiguration;
            _storageInventoryConfiguration = configurations.DefauleStorageItems;
        }
        public void SetInventory(Inventory inventory)
            => _storageInventory = inventory;

        private void OnInventorySelect()
        {
            CellPresenter cellPresenter = _storageMatrix[_selectionPosition.y, _selectionPosition.x];
            Cell cell = cellPresenter.Cell;
            if (cell == null)
                return;
            if (!cellPresenter.IsInteractable)
                return;
            int maxQuantityInPlayerInventory = _playerInventoryConfigurations.GetCountInCell(cell.Key);
            int emptySpace = _playerInventory.CheckSpaceInInventory(cell.Key);
            int toAdd;
            if (emptySpace > maxQuantityInPlayerInventory)
                toAdd = maxQuantityInPlayerInventory;
            else toAdd = emptySpace;
            SSOKey upperCategoryInStorage = _equipmentConfigurations.GetUpperCategory(cell.Key);
            bool isChangable = _equipmentConfigurations.ChangableCategories.Contains(upperCategoryInStorage);
            if (isChangable)
            {
                int count = _playerInventory.Cells.Length;
                for (int i = 0; i < count; i++)
                {
                    SSOKey upper = _equipmentConfigurations.GetUpperCategory(_playerInventory.Cells[i].Key);
                    if (upper == upperCategoryInStorage)
                    {
                        _playerInventory.RemoveItem(_playerInventory.Cells[i].Key, toAdd);
                        break;
                    }
                }
            }
            _audioController.AudioData.SoundTracks.ItemTransfer.Play();
            _storageInventory.RemoveItem(cell.Key, toAdd);
            _playerInventory.AddItem(cell.Key, toAdd);
        }
        private async UniTask DefferedSubscribes()
        {
            await UniTask.NextFrame();
            _villageTrustSystem.OnTrustChanged.AddListener(OnTrustUpdated);
            _appInputSystem.OnMovedInInventory.AddListener(MoveSelectionSelection);
            _appInputSystem.OnInventorySelected.AddListener(OnInventorySelect);
            _playerInventory.OnInventoryUpdated.AddListener(UpdatePlayerInventoryCells);
            _storageInventory.OnInventoryUpdated.AddListener(UpdateStorageInventoryCells);
        }
        private void OnTrustUpdated(float trust)
        {
            _trustText.text = $"Доверие: {trust}";
            foreach (CellPresenter cell in _storageCells)
            {
                if (cell.Cell != null)
                {
                    if (_storageInventoryConfiguration[cell.Cell.Key].TrustRequirement <= _villageTrustSystem.Trust
                        && _storageInventoryConfiguration[cell.Cell.Key].LevelRequirement <= _levelsController.CurrentLevel)
                        cell.IsInteractable = true;
                    else cell.IsInteractable = false;
                }
            }
        }
        private void PrepareInventoryMatrix()
        {
            int count = _storageCells.Length;
            int j = 0;
            int k = 0;
            for (int i = 0; i < count; i++)
            {
                _storageMatrix[j, k] = _storageCells[i];
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
            _audioController.AudioData.SoundTracks.ChangingCells.Play();
            _storageMatrix[_selectionPosition.y, _selectionPosition.x].Highlighter.TurnOffHighlight();
            _selectionPosition = (x, y);
            _storageMatrix[y, x].Highlighter.Highlight();
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
                else _inventoryCells[i].Clear();
            }
        }
        private void UpdateStorageInventoryCells(Cell[] obj)
        {
            int count = obj.Length;
            for (int i = 0; i < count; i++)
            {
                if (obj[i] != null)
                {
                    _storageCells[i].Cell = obj[i];
                    _storageCells[i].SetCount(obj[i].Count);
                    _storageCells[i].SetSprite(_iconsConfiguration[obj[i].Key]);
                }
                else _storageCells[i].Clear();
            }
        }
    }
}