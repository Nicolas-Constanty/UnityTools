using UnityEngine;
using System;

// ReSharper disable once CheckNamespace
namespace UnityTools.Collections
{
    [Serializable]
    public class DictionaryStringInt : SerializableDictionary<string, int>
    {
        public override AbscractDictionary GetDico()
        {
            return this;
        }
    }

    [Serializable]
    public class DictionaryStringBool : SerializableDictionary<string, bool>
    {
        public override AbscractDictionary GetDico()
        {
            return this;
        }
    }

    [Serializable]
    public class DictionaryIntString : SerializableDictionary<int, string>
    {

        public override AbscractDictionary GetDico()
        {
            return this;
        }
    }

    [Serializable]
    public class DictionaryIntInt : SerializableDictionary<int, int>
    {

        public override AbscractDictionary GetDico()
        {
            return this;
        }
    }

    [Serializable]
    public class DictionaryStringString : SerializableDictionary<string, string>
    {

        public override AbscractDictionary GetDico()
        {
            return this;
        }
    }

    [Serializable]
    public class DictionaryStringVector2 : SerializableDictionary<string, Vector2>
    {

        public override AbscractDictionary GetDico()
        {
            return this;
        }
    }

    [Serializable]
    public class DictionaryStringVector3 : SerializableDictionary<string, Vector3>
    {

        public override AbscractDictionary GetDico()
        {
            return this;
        }
    }

    [Serializable]
    public class DictionaryIntVector2 : SerializableDictionary<int, Vector2>
    {

        public override AbscractDictionary GetDico()
        {
            return this;
        }
    }

    [Serializable]
    public class DictionaryIntVector3 : SerializableDictionary<int, Vector3>
    {

        public override AbscractDictionary GetDico()
        {
            return this;
        }
    }
}
