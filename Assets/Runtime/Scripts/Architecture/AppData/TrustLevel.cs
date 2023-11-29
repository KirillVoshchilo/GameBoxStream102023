using System;
using UnityEngine;

namespace App.Architecture.AppData
{
    [Serializable]
    public class TrustLevel
    {
        [SerializeField] private float _trust;
        [SerializeField] private GameObject _scarecrow;

        public float Trust => _trust;
        public GameObject Scarecrow  => _scarecrow; 
    }
}