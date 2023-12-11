using App.Simples;
using System;
using UnityEngine;

namespace App.Architecture.AppData
{
    [Serializable]
    public sealed class Models
    {
        [SerializeField] private Model[] _values;

        public GameObject Get(SSOKey key)
        {
            foreach (Model model in _values)
            {
                if (model.Key == key)
                    return model.Mesh;
            }
            return null;
        }
    }
}