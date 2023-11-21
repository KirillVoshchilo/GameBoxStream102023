﻿using System;
using UnityEngine;

namespace App.Content.Entities
{
    [Serializable]
    public sealed class InteractionRequirementsComp
    {
        [SerializeField] private Alternatives[] _alternatives;
        
        public Alternatives[] Alternatives
            => _alternatives;
    }
}