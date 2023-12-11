using System;
using UnityEngine;

namespace App.Architecture.AppData
{
    [Serializable]
    public sealed class InteractionRequirementsComp
    {
        [SerializeField] private Alternatives[] _alternatives;

        public Alternatives[] Alternatives => _alternatives;
    }
}