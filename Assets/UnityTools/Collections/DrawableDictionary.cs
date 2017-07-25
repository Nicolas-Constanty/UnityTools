using UnityEngine;

// ReSharper disable once CheckNamespace
namespace UnityTools.Collections
{
    public class DrawableDictionary<TKey, TValue> : MonoBehaviour
    {
        public SerializableDictionary<TKey, TValue> Dictionary = new SerializableDictionary<TKey, TValue>();
    }
}

