using System;
using UnityEngine;

namespace App.Architecture.AppData
{
    [Serializable]
    public class Model
    {
        [SerializeField] private Key _key;
        [SerializeField] private GameObject _mesh;

        public Key Key => _key;
        public GameObject Mesh => _mesh;
    }
}