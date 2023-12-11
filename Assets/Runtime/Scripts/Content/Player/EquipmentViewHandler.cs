using App.Content.Tree;
using App.Simples;
using App.Simples.CellsInventory;
using UnityEngine;

namespace App.Content.Player
{
    public sealed class EquipmentViewHandler
    {
        private PlayerData _playerData;

        public EquipmentViewHandler(PlayerData playerData)
        {
            _playerData = playerData;
            playerData.AppInputSystem.OnInteractionStarted.AddListener(OnInteractionStarted);
            playerData.AppInputSystem.OnInteractionCanceled.AddListener(OnInteractionCanceled);
            playerData.AppInputSystem.OnInteractionPerformed.AddListener(OnInteractionPerformed);
        }

        private void OnInteractionPerformed()
        {
            StopChoping();
        }
        private void OnInteractionCanceled()
        {
            StopChoping();
        }
        private void OnInteractionStarted()
        {
            if (_playerData.InteractionEntity == null)
                return;
            if (_playerData.InteractionEntity.Entity == null)
                return;
            StartChoping();
        }
        private void StopChoping()
        {
            if (_playerData.CurrentAxeModel != null)
                Object.Destroy(_playerData.CurrentAxeModel);
        }
        private void StartChoping()
        {
            if (_playerData.InteractionEntity.Entity is not TreeEntity)
                return;
            SSOKey currentAxe = DefineCurrentAxe();
            GameObject prefab = _playerData.Configuration.Models.Get(currentAxe);
            GameObject instance = Object.Instantiate(prefab, _playerData.AxeParent);
            _playerData.CurrentAxeModel = instance;
        }

        private SSOKey DefineCurrentAxe()
        {
            Cell[] cells = _playerData.PlayerInventory.Cells;
            int count = cells.Length;
            for (int i = 0; i < count; i++)
            {
                if (cells[i] == null)
                    continue;
                SSOKey category = _playerData.Configuration.EquipmentConfigurations.GetUpperCategory(cells[i].Key);
                if (category == null)
                    continue;
                if (category == _playerData.AxeCategory)
                    return cells[i].Key;
            }
            return null;
        }
    }
}