using App.Simples;
using System;
using UnityEngine;

namespace App.Architecture.AppData
{
    [Serializable]
    public sealed class Model
    {
        [SerializeField] private SSOKey _key;
        [SerializeField] private GameObject _mesh;

        public SSOKey Key => _key;
        public GameObject Mesh => _mesh;
    }
}