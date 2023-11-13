using System;
using UnityEngine;

namespace App.Architecture.AppData
{
    [CreateAssetMenu]
    public sealed class Key : ScriptableObject, IEquatable<Key>
    {
        [SerializeField] private string _name;

        public string Value
            => _name;

        public bool Equals(Key other)
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
            Key key = obj as Key;
            if (key == null)
                return false;
            else return Equals(key);
        }
        public override int GetHashCode()
            => _name.GetHashCode();

        public static bool operator ==(Key a, Key b)
        {
            if (((object)a) == null || ((object)b) == null)
                return System.Object.Equals(a, b);

            return a.Equals(b);
        }
        public static bool operator !=(Key a, Key b)
            => !(a == b);
    }
}