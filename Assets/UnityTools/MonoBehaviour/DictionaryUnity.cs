using UnityEngine;
using UnityTools.Collections;

// ReSharper disable once CheckNamespace
namespace UnityTools
{
    public class DictionaryUnity : MonoBehaviour
    {

        public AbscractDictionary Dico = new DictionaryIntInt();

        //private DictionaryIntInt IntInt = null;
        //private DictionaryIntString stringint = null;
        //private DictionaryIntVector2 IntVector2 = null;
        //private DictionaryIntVector3 IntVector3 = null;

        //private DictionaryStringInt StringInt = null;
        //private DictionaryStringString StringString = null;
        //private DictionaryStringVector2 StringVector2 = null;
        //private DictionaryStringVector3 StringVector3 = null;

        public int Key;
        public int Value;

        public AbscractDictionary CreateDictionary(int key, int value)
        {
            Key = key;
            Value = value;
            switch (key)
            {
                case 0:
                    switch (value)
                    {
                        case 0:
                            Dico = new DictionaryIntInt();
                            break;
                        case 1:
                            Dico = new DictionaryIntString();
                            break;
                        case 2:
                            Dico = new DictionaryIntVector2();
                            break;
                        case 3:
                            Dico = new DictionaryIntVector3();
                            break;
                        default:
                            Dico = new DictionaryIntInt();
                            break;
                    }
                    break;
                case 1:
                    switch (value)
                    {
                        case 0:
                            Dico = new DictionaryStringInt();
                            break;
                        case 1:
                            Dico = new DictionaryStringString();
                            break;
                        case 2:
                            Dico = new DictionaryStringVector2();
                            break;
                        case 3:
                            Dico = new DictionaryStringVector3();
                            break;
                        default:
                            Dico = new DictionaryIntInt();
                            break;
                    }
                    break;
                default:
                    Dico = new DictionaryIntInt();
                    break;
            }
            return Dico;
        }
    }
}
