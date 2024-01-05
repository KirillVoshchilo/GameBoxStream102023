using App.Architecture;
using App.Architecture.AppData;
using App.Architecture.AppInput;
using App.Content.Audio;
using App.Content.Player;
using App.Simples;
using App.Simples.CellsInventory;
using Cysharp.Threading.Tasks;
using SimpleComponents.UI;
using TMPro;
using UnityEngine;

namespace App.Content.UI
{
    public sealed class FevroniaMenuPresenter : MonoBehaviour
    {
        [SerializeField] private CellPresenter[] _cellPresenters;
        [SerializeField] private TextMeshProUGUI _currentWoodCount;
        [SerializeField] private TextMeshProUGUI _currentWoodCountShort;
        [SerializeField] private TextMeshProUGUI _woodRequirements;
        [SerializeField] private TextMeshProUGUI _trustText;
        [SerializeField] private SSOKey _requirementResource;
        [SerializeField] private Transform _dialogeTransform;

        private SCSlideShow _dialoge;
        private Inventory _playerInventory;
        private IconsConfiguration _iconsConfiguration;
        private VillageTrustSystem _villageTrustSystem;
        private TrustLevels _trustLevels;
        private AudioStorage _audioController;
        private IAppInputSystem _appInputSystem;
        private readonly CellPresenter[,] _cellsMatrix = new CellPresenter[3, 3];
        private (int x, int y) _selectionPosition;
        private bool _isEnable;
        private int _currentLevel;

        public bool IsEnable
        {
            get => _isEnable;
            set
            {
                _isEnable = value;
                if (value)
                {
                    OnInventoryUpdated(_playerInventory.Cells);
                    PrepareInventoryMatrix();
                    SetSelection(0, 0);
                    ShowTrust(_villageTrustSystem.Trust);
                    ShowGoalCount(_villageTrustSystem.CurrentTrustLevel);
                    ShowWoodQuantity(_villageTrustSystem.Trust);
                    DefferedSubscribes()
                        .Forget();
                }
                else
                {
                    _villageTrustSystem.OnTrustChanged.RemoveListener(OnTrustChanged);
                    _villageTrustSystem.OnTrustLevelChanged.RemoveListener(OnTrustLevelChanged);
                    _appInputSystem.OnMovedInInventory.RemoveListener(OnMovedInInventory);
                    _appInputSystem.OnInventorySelected.RemoveListener(OnInventorySelected);
                    _playerInventory.OnInventoryUpdated.RemoveListener(OnInventoryUpdated);
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
        public int CurrentLevel
        {
            get => _currentLevel;
            set
            {
                ShowGoalCount(value + 1);
                _currentLevel = value;
            }
        }

        public void Construct(PlayerEntity playerEntity,
            Configuration configurations,
            VillageTrustSystem villageTrustSystem,
            IAppInputSystem appInputSystem,
            AudioStorage audioController)
        {
            _audioController = audioController;
            _appInputSystem = appInputSystem;
            _villageTrustSystem = villageTrustSystem;
            _playerInventory = playerEntity.Get<Inventory>();
            _trustLevels = configurations.TrustLevels;
            _iconsConfiguration = configurations.IconsConfiguration;
        }

        private async UniTask DefferedSubscribes()
        {
            await UniTask.NextFrame();
            _villageTrustSystem.OnTrustChanged.AddListener(OnTrustChanged);
            _villageTrustSystem.OnTrustLevelChanged.AddListener(OnTrustLevelChanged);
            _appInputSystem.OnMovedInInventory.AddListener(OnMovedInInventory);
            _appInputSystem.OnInventorySelected.AddListener(OnInventorySelected);
            _playerInventory.OnInventoryUpdated.AddListener(OnInventoryUpdated);
        }

        private void OnTrustChanged(float obj)
        {
            ShowTrust(obj);
            ShowWoodQuantity(obj);
        }

        private void OnTrustLevelChanged(int trustLevel)
        {
            ShowGoalCount(trustLevel);
            ShowWoodQuantity(_villageTrustSystem.Trust);
        }
        private void ShowWoodQuantity(float trust)
        {
            if (_villageTrustSystem.CurrentTrustLevel >= _trustLevels.Length)
            {
                _currentWoodCountShort.text = "0";
                _currentWoodCount.text = $"В наличии {0}";
                return;
            }
            int trustLevel = _villageTrustSystem.CurrentTrustLevel;
            if (_currentLevel < trustLevel)
                trustLevel = _currentLevel;
            float difference = trust;
            if (trustLevel > 0)
            {
                int previousLevel = Mathf.Clamp(trustLevel - 1, 0, trustLevel);
                difference -= _trustLevels[previousLevel];
            }
            _currentWoodCountShort.text = difference.ToString();
            _currentWoodCount.text = $"В наличии {difference}";
        }
        private void ShowTrust(float trust)
            => _trustText.text = $"Доверие: {trust}";
        private void ShowGoalCount(int trustLevel)
        {
            if (trustLevel >= _trustLevels.Length)
                return;
            if (trustLevel != _currentLevel)
            {
                _woodRequirements.text = $"Достаточно дров.";
                return;
            }
            float goal = _trustLevels[trustLevel];
            if (trustLevel > 0)
            {
                int previousLevel = Mathf.Clamp(trustLevel - 1, 0, trustLevel);
                goal -= _trustLevels[previousLevel];
            }
            _woodRequirements.text = $"{goal} дров.";
        }
        private void OnInventorySelected()
        {
            if (_villageTrustSystem.Trust == _trustLevels[_currentLevel]
                && _currentLevel < _trustLevels.Length - 1)
            {
                return;
            }
            Cell cell = _cellsMatrix[_selectionPosition.y, _selectionPosition.x].Cell;
            if (cell == null)
                return;
            if (_requirementResource != cell.Key)
                return;
            int cellIndex = _selectionPosition.y * 3 + _selectionPosition.x;
            int toRemove;
            if (_villageTrustSystem.CurrentTrustLevel >= _trustLevels.Length)
                toRemove = cell.Count;
            else
            {
                toRemove = GetRequiredWood();
                toRemove = Mathf.Clamp(toRemove, 0, cell.Count);
            }
            _villageTrustSystem.AddTrust(toRemove);
            _playerInventory.RemoveItemFromCell(cell.Key, toRemove, cellIndex);
            _audioController.AudioData.SoundTracks.ItemTransfer.Play();
        }
        private int GetRequiredWood()
        {
            int currentLevel = _villageTrustSystem.CurrentTrustLevel;
            return (int)(_trustLevels[currentLevel] - _villageTrustSystem.Trust);
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
        private void OnMovedInInventory(Vector2 vector)
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
            _cellsMatrix[_selectionPosition.y, _selectionPosition.x].Highlighter.TurnOffHighlight();
            _selectionPosition = (x, y);
            _cellsMatrix[y, x].Highlighter.Highlight();
        }
        private void OnInventoryUpdated(Cell[] obj)
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
                else _cellPresenters[i].Clear();
            }
        }
    }
}