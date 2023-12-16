using App.Simples;
using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

namespace App.Architecture.AppData
{
    [Serializable]
    public sealed class Models
    {
        [SerializeField] private Model[] _values;

        private Dictionary<SSOKey, Model> _modelsDictionary;

        public GameObject this[SSOKey key]
        {
            get
            {
                if (_modelsDictionary == null)
                {
                    _modelsDictionary = new Dictionary<SSOKey, Model>();
                    foreach (Model model in _values)
                        _modelsDictionary.Add(model.Key, model);
                }
                return _modelsDictionary[key].Mesh;
            }
        }
    }
}