using App.Architecture.AppData;
using System;
using UnityEngine;

namespace App.Content.Entities
{
    [Serializable]
    public sealed class InteractionRequirementsComp
    {
        [SerializeField] private ItemCount[] _requirements;

        public ItemCount[] Requirements
            => _requirements;
    }
}