using App.Simples;
using System.Collections.Generic;
using UnityEngine;

namespace App.Content.Entities
{
    public class Blackboard
    {
        private readonly SEvent<string> _onVariableAdded = new();
        private readonly SEvent<string> _onVariableChanged = new();
        private readonly SEvent<string> _onVariableRemoved = new();
        private readonly Dictionary<string, int> _intDictionary = new();
        private readonly Dictionary<string, float> _floatDictionary = new();
        private readonly Dictionary<string, string> _stringDictionary = new();
        private readonly Dictionary<string, char> _charDictionary = new();

        public SEvent<string> OnVariableAdded
            => _onVariableAdded;
        public SEvent<string> OnVariableChanged
            => _onVariableChanged;
        public SEvent<string> OnVariableRemoved
            => _onVariableRemoved;

        public T GetVariable<T>(string key)
        {
            Dictionary<string, T> dictionary = GetDictionary<T>();
            if (dictionary.TryGetValue(key, out T value))
                return value;
            Debug.Log("Cannot getVariable");
            return default;
        }
        public bool HasVariable<T>(string key)
        {
            Dictionary<string, T> dictionary = GetDictionary<T>();
            return dictionary.ContainsKey(key);
        }
        public bool TryGetVariable<T>(string key, out T value)
        {
            Dictionary<string, T> dictionary = GetDictionary<T>();
            value = default;
            if (dictionary.TryGetValue(key, out T innerValue))
            {
                value = innerValue;
                return true;
            }
            return false;
        }
        public void AddVariable<T>(string key, T value)
        {
            Dictionary<string, T> dictionary = GetDictionary<T>();
            dictionary.Add(key, value);
        }
        public void ChangeVariable<T>(string key, T value)
        {
            Dictionary<string, T> dictionary = GetDictionary<T>();
            dictionary[key] = value;
        }
        public void RemoveVariable<T>(string key)
        {
            Dictionary<string, T> dictionary = GetDictionary<T>();
            dictionary.Remove(key);
        }
        public virtual void Clear()
        {
            _intDictionary.Clear();
            _floatDictionary.Clear();
            _stringDictionary.Clear();
            _charDictionary.Clear();
        }

        protected virtual Dictionary<string, T> GetDictionary<T>()
        {
            if (typeof(T) == typeof(int))
                return _intDictionary as Dictionary<string, T>;
            if (typeof(T) == typeof(float))
                return _floatDictionary as Dictionary<string, T>;
            if (typeof(T) == typeof(string))
                return _stringDictionary as Dictionary<string, T>;
            if (typeof(T) == typeof(char))
                return _charDictionary as Dictionary<string, T>;
            return null;
        }
    }
}