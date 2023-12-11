using System;
using UnityEngine;

namespace App.Simples
{
    [CreateAssetMenu]
    public sealed class SSOKey : ScriptableObject, IEquatable<SSOKey>
    {
        [SerializeField] private string _name;

        public string Value => _name;

        public bool Equals(SSOKey other)
        {
            if (other == null)
                return false;
            if (_name == other.Value)
                return true;
            else return false;
        }
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            SSOKey key = obj as SSOKey;
            if (key == null)
                return false;
            else return Equals(key);
        }
        public override int GetHashCode()
            => _name.GetHashCode();

        public static bool operator ==(SSOKey a, SSOKey b)
        {
            if (((object)a) == null || ((object)b) == null)
                return System.Object.Equals(a, b);

            return a.Equals(b);
        }
        public static bool operator !=(SSOKey a, SSOKey b)
            => !(a == b);
    }
}