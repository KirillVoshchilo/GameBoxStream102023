using System;
using UnityEngine;

[Serializable]
public class ScarecrowData
{
    [SerializeField] private GameObject _secondLevelModel;
    [SerializeField] private GameObject _thirdLevelModel;
    [SerializeField] private float _secondLevelTrust;
    [SerializeField] private float _thirdLevelTrust;

    public GameObject SecondLevelModel => _secondLevelModel; 
    public GameObject ThirdLevelModel => _thirdLevelModel;
    public float SecondLevelTrust  => _secondLevelTrust;
    public float ThirdLevelTrust => _thirdLevelTrust;
}