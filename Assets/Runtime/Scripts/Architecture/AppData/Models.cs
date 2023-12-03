using System;
using UnityEngine;

namespace App.Architecture.AppData
{
    [Serializable]
    public class Models
    {
        [SerializeField] private Model[] _values;

        public Model[] Values => _values;

        public GameObject Get(Key key)
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