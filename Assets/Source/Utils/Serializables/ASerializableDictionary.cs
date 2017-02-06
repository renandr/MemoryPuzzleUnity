using UnityEngine;
using System.Collections.Generic;

namespace GGS.CakeBox.Utils
{
    //
    // Unity doesn't know how to serialize a Dictionary
    // So this is a simple extension of a dictionary that saves as two lists.
    // By Pablo Bollansée

    //
    // Usage is a little strange though, for some reason you can't use it directly in unity.
    // You have to make a non-generic instance of it, and then use it. This is luckily quite easy:
    // 
    // [System.Serializable]
    // class MyDictionary : SerializableDictionary<KeyType, ValueType> {}
    //
    // Then make an instance of this like this:
    //
    // [SerializeField]
    // private MyDictionary _dictionary = new MyDictionary();
    //
    // Now you can use it in exactly the same way as a notmal Dictionary. Everything just works.

    [System.Serializable]
    public abstract class ASerializableDictionary<TKey, TValue> : ISerializationCallbackReceiver
    {
        public Dictionary<TKey, TValue> Dictionary { get; private set; }

        // We save the keys and values in two lists because Unity does understand those.
        [SerializeField]
        private List<TKey> _keys;
        [SerializeField]
        private List<TValue> _values;

        protected ASerializableDictionary()
        {
            Dictionary = new Dictionary<TKey, TValue>();
        }


        // Before the serialization we fill these lists
        public void OnBeforeSerialize()
        {
            _keys = new List<TKey>(Dictionary.Count);
            _values = new List<TValue>(Dictionary.Count);
            foreach (var kvp in Dictionary)
            {
                _keys.Add(kvp.Key);
                _values.Add(kvp.Value);
            }

        }

        // After the serialization we create the dictionary from the two lists
        public void OnAfterDeserialize()
        {
            Dictionary = new Dictionary<TKey, TValue>();
            //Dictionary.Clear();
            for (int i = 0; i < Mathf.Min(_keys.Count, _values.Count); i++)
            {
                Dictionary.Add(_keys[i], _values[i]);
            }
        }


        public void Add(TKey key, TValue value)
        {
            Dictionary.Add(key, value);
        }

        public bool Remove(TKey key)
        {
            return Dictionary.Remove(key);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return Dictionary.TryGetValue(key, out value);
        }

        public bool ContainsKey(TKey key)
        {
            return Dictionary.ContainsKey(key);
        }

        public bool ContainsValue(TValue value)
        {
            return Dictionary.ContainsValue(value);
        }

        public void Clear()
        {
            Dictionary.Clear();
        }

        public int Count
        {
            get
            {
                return Dictionary.Count;
            }
        }

        public TValue this[TKey key]
        {
            get
            {
                return Dictionary[key];
            }

            set
            {
                Dictionary[key] = value;
            }
        }

    }
}