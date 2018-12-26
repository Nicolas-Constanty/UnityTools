#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

// ReSharper disable once CheckNamespace
namespace UnityTools.Collections
{
    [CustomEditor(typeof(DictionaryUnity))]
    public class DictionaryScript : Editor
    {
        public string[] Options1 = new string[] { "Int", "String" };
        private int m_Index1;
        public string[] Options2 = new string[] { "Int", "String", "Vector2", "Vector3" };
        private int m_Index2;
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            DictionaryUnity dico = (DictionaryUnity)target;
            m_Index1 = EditorGUILayout.Popup(m_Index1, Options1);
            m_Index2 = EditorGUILayout.Popup(m_Index2, Options2);
            if (GUILayout.Button("Create"))
            {
                dico.CreateDictionary(m_Index1, m_Index2);
            }
        }
    }
}
#endif
