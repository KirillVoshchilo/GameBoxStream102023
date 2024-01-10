using System;
using UnityEngine;

[Serializable]
public struct SData<T1, T2>
{
    [SerializeField] private T1 _value;
    [SerializeField] private T2 _value2;

    public T1 Value  => _value; 
    public T2 Value2  => _value2;
}
