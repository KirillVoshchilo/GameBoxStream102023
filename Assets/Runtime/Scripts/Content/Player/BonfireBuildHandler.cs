using App.Architecture.AppData;
using App.Content.Entities;
using Unity.VisualScripting;
using UnityEngine;

namespace App.Content.Player
{
    public class BonfireBuildHandler
    {
        private PlayerData _playerData;



        public BonfireBuildHandler(PlayerData playerData)
        {
            _playerData = playerData;
            _playerData.PlayerAnimationsEvents.OnBonfireSetted.AddListener(OnBonfireSetted);
        }

        private void OnBonfireSetted()
        {
            if (!CheckRequirements(out Alternatives alternative))
                return;
            foreach (ItemCount item in alternative.Requirements)
            {
                _playerData.PlayerInventory.RemoveItem(item.Key, item.Count);
            }
            _playerData.BonfireFactory.BuildBonfire(_playerData.BonfireTargetPosition.position);
        }

        private bool CheckRequirements(out Alternatives alternative)
        {
            foreach (Alternatives alt in _playerData.BonfireBuildRequirements.Alternatives)
            {
                if (CheckAlternative(alt))
                {
                    alternative = alt;
                    return true;
                }
            }
            alternative = null;
            return false;
        }
        private bool CheckAlternative(Alternatives alternatives)
        {
            foreach (ItemCount itemCount in alternatives.Requirements)
            {
                if (_playerData.PlayerInventory.GetCount(itemCount.Key) < itemCount.Count)
                    return false;
            }
            return true;
        }
    }
}