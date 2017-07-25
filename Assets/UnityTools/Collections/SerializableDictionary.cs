using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace UnityTools.Collections
{
    [Serializable]
    public class SerializableDictionary<TKey, TValue> : AbscractDictionary, IDictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField]
        protected List<TKey> keys = new List<TKey>();

        [SerializeField]
        protected List<TValue> values = new List<TValue>();

        protected Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();

        private int m_IntIndex;

        public ICollection<TKey> Keys
        {
            get
            {
                return dictionary.Keys;
            }
        }

        public ICollection<TValue> Values
        {
            get
            {
                return dictionary.Values;
            }
        }

        public int Count
        {
            get
            {
                return dictionary.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public TValue this[TKey key]
        {
            get
            {
                return dictionary[key];
            }

            set
            {
                dictionary[key] = value;
            }
        }

        // save the dictionary to lists
        public void OnBeforeSerialize()
        {
            keys.Clear();
            values.Clear();
            foreach (KeyValuePair<TKey, TValue> pair in this)
            {
                keys.Add(pair.Key);
                values.Add(pair.Value);
            }
        }

        // load dictionary from lists
        public void OnAfterDeserialize()
        {
            dictionary.Clear();

            if (keys.Count != values.Count)
                throw new Exception(string.Format("there are {0} keys and {1} values after deserialization. Make sure that both key and value types are serializable.", keys.Count, values.Count));


            for (int i = 0; i < keys.Count; i++)
            {
                try
                {
                    dictionary.Add(keys[i], values[i]);
                }
                catch (ArgumentException)
                {
                    //Debug.LogWarning("An element with the same key already exists in the dictionary.");
                    if (typeof(TKey) == typeof(int))
                    {
                        var k = (TKey) Convert.ChangeType(GetIntIndex(), typeof(TKey));
                        if (k != null)
                        dictionary.Add(k, values[i]);
                    }
                    else if (typeof(TKey) == typeof(string))
                    {
                        var k = (TKey) Convert.ChangeType(GetStringIndex(), typeof(TKey));
                        if (k != null)
                            dictionary.Add(k, values[i]);
                    }
                    return;
                }
            }
        }

        private string GetStringIndex()
        {
            if (typeof(TKey) == typeof(int))
            {
                Debug.LogWarning("Dictionnary currently use string as key use GetIntIndex instead");
                return (-1).ToString();
            }
            foreach (var elem in dictionary)
            {
                if ((string)Convert.ChangeType(elem.Key, typeof(string)) == "")
                {
                    ++m_IntIndex;
                    return m_IntIndex.ToString();
                }
            }
            return m_IntIndex.ToString();
        }

        public int GetIntIndex()
        {
            if (typeof(TKey) == typeof(string))
            {
                Debug.LogWarning("Dictionnary currently use string as key use GetStringIndex instead");
                return -1;
            }
            if (dictionary.Select(elem => Convert.ChangeType(elem.Key, typeof(int))).All(v => (int?) v != m_IntIndex))
                return m_IntIndex;
            ++m_IntIndex;
            return m_IntIndex;
        }

        public bool ContainsKey(TKey key)
        {
            return dictionary.ContainsKey(key);
        }

        public bool Remove(TKey key)
        {
            return dictionary.Remove(key);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return dictionary.TryGetValue(key, out value);
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            dictionary.Add(item.Key, item.Value);
        }

        public void Clear()
        {
            dictionary.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return (dictionary as ICollection<KeyValuePair<TKey, TValue>>).Contains(item);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            (dictionary as ICollection<KeyValuePair<TKey, TValue>>).CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return (dictionary as ICollection<KeyValuePair<TKey, TValue>>).Remove(item);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return dictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return dictionary.GetEnumerator();
        }

        public void Add(TKey key, TValue value)
        {
            dictionary.Add(key, value);
        }

        public override AbscractDictionary GetDico()
        {
            return this;
        }
    }
}
