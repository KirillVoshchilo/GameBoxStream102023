using App.Architecture;
using App.Architecture.AppData;
using App.Architecture.AppInput;
using App.Content.Entities;
using App.Content.Player;
using App.Logic;
using UnityEngine;
using VContainer;

namespace App.Content.Buildings
{
    public sealed class BuildingSiteEntity : MonoBehaviour, IEntity, IDestructable
    {
        [SerializeField] private BuildingSiteData _buildingSiteData;

        [Inject]
        public void Construct(PlayerEntity player,
        WorldCanvasStorage worldCanvasStorage,
        IAppInputSystem appInputSystem)
        {
            _buildingSiteData.AppInputSystem = appInputSystem;
            _buildingSiteData.PlayerInventory = player.Get<PlayerInventory>();
            _buildingSiteData.WorldCanvasStorage = worldCanvasStorage;
            _buildingSiteData.InteractableComp.OnFocusChanged.AddListener(OnInteractionFocusChanged);
            Debug.Log("Сконструировал BuildingSiteEntity.");
        }
        public void Destruct()
        {
            CloseRequirementsPanel();
            DisableInteraction();
            CloseInteractionIcon();
            _buildingSiteData.InteractableComp.OnFocusChanged.RemoveListener(OnInteractionFocusChanged);
        }
        public T Get<T>() where T : class
        {
            if (typeof(T) == typeof(InteractionComp))
                return _buildingSiteData.InteractableComp as T;
            return null;
        }

        private void OnInteractionFocusChanged(bool obj)
        {
            if (obj)
            {
                ShowRequirementsPanel();
                CheckInteractable();
                if (_buildingSiteData.IsInteractable)
                {
                    ShowInteractionIcon();
                    EnableInteraction();
                }

            }
            else
            {
                CloseRequirementsPanel();
                if (_buildingSiteData.IsInteractable == true)
                    DisableInteraction();
                CloseInteractionIcon();
            }
        }
        private void ShowRequirementsPanel()
        {
            _buildingSiteData.RequirementsPanel.SetPosition(_buildingSiteData.RequirementsPanelPosition);
            _buildingSiteData.RequirementsPanel.gameObject.SetActive(true);
            _buildingSiteData.RequirementsPanel.FillWithItems(_buildingSiteData.BuildRequirements);
            _buildingSiteData.RequirementsPanel.IsEnable = true;
        }
        private void CloseRequirementsPanel()
        {
            _buildingSiteData.RequirementsPanel.IsEnable = false;
            _buildingSiteData.RequirementsPanel.Clear();
            _buildingSiteData.RequirementsPanel.gameObject.SetActive(false);
        }
        private void EnableInteraction()
        {
            _buildingSiteData.AppInputSystem.OnInteractionPerformed.AddListener(OnInteractionPerformed);
            _buildingSiteData.AppInputSystem.SetInteractionTime(_buildingSiteData.InteractTime);
        }
        private void DisableInteraction()
            => _buildingSiteData.AppInputSystem.OnInteractionPerformed.RemoveListener(OnInteractionPerformed);
        private void OnInteractionPerformed()
        {
            foreach (ItemCount item in _buildingSiteData.BuildRequirements)
                _buildingSiteData.PlayerInventory.RemoveItem(item.Key, item.Count);
            _buildingSiteData.BuildingFactory.Parent = _buildingSiteData.BuilderTransform;
            _buildingSiteData.BuildingFactory.Create();
            Destruct();
            Destroy(gameObject);
        }
        private void ShowInteractionIcon()
        {
            _buildingSiteData.InteractIcon.SetPosition(_buildingSiteData.InteractionIconPosition);
            _buildingSiteData.InteractIcon.gameObject.SetActive(true);
            _buildingSiteData.InteractIcon.IsEnable = true;
            _buildingSiteData.InteractIcon.OpenTip();
            _buildingSiteData.InteractIcon.HoldMode = false;
        }
        private void CloseInteractionIcon()
        {
            _buildingSiteData.InteractIcon.CloseProgress();
            _buildingSiteData.InteractIcon.CloseTip();
            _buildingSiteData.InteractIcon.IsEnable = false;
            _buildingSiteData.InteractIcon.gameObject.SetActive(false);
        }
        private void CheckInteractable()
        {
            foreach (ItemCount item in _buildingSiteData.BuildRequirements)
            {
                if (_buildingSiteData.PlayerInventory.GetCount(item.Key) < item.Count)
                {
                    _buildingSiteData.IsInteractable = false;
                    return;
                }
            }
            _buildingSiteData.IsInteractable = true;
        }
    }
}