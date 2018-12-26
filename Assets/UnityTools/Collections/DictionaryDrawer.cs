#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;

// ReSharper disable once CheckNamespace
namespace UnityTools.Collections
{
    [CustomPropertyDrawer(typeof(AbscractDictionary), true)]
    public class DictionaryDrawer : PropertyDrawer
    {

        int m_Size;
        private Rect GetNextPos(ref Rect position)
        {
            Rect rect = new Rect(position.xMin, position.yMin, position.width, EditorGUIUtility.singleLineHeight);
            Single height = EditorGUIUtility.singleLineHeight + 1f;
            position = new Rect(position.xMin, position.yMin + height, position.width, height);
            return rect;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            bool expanded = property.isExpanded;
            Rect contentPosition = GetNextPos(ref position);
            property.isExpanded = EditorGUI.Foldout(contentPosition, property.isExpanded, label);

            if (expanded)
            {
                EditorGUI.indentLevel++;
                SerializedProperty keys = property.FindPropertyRelative("keys");
                SerializedProperty values = property.FindPropertyRelative("values");
                contentPosition = GetNextPos(ref position);
                GUIStyle st = new GUIStyle();
                st.alignment = TextAnchor.MiddleCenter;
                EditorGUI.LabelField(contentPosition, "   Keys     Values", st);
                m_Size = keys.arraySize;
                float w;
                Rect r0;
                Rect r1;
                for (int i = 0; i < m_Size; i++)
                {
                    contentPosition = GetNextPos(ref position);
                    contentPosition = EditorGUI.IndentedRect(contentPosition);
                    w = contentPosition.width / 2f;
                    r0 = new Rect(contentPosition.xMin, contentPosition.yMin, w, contentPosition.height);
                    r1 = new Rect(r0.xMax, contentPosition.yMin, w + w - r0.xMax, contentPosition.height);

                    SerializedProperty key = keys.GetArrayElementAtIndex(i);
                    SerializedProperty value = values.GetArrayElementAtIndex(i);
                    EditorGUI.PropertyField(r0, key, GUIContent.none, false);
                    EditorGUI.PropertyField(r1, value, GUIContent.none, false);
                }
                GetNextPos(ref position);
                contentPosition = GetNextPos(ref position);
                contentPosition.width = 230;
                contentPosition.x = (position.width + 14) / 2f - contentPosition.width / 2f;
                if (GUI.Button(contentPosition, "+"))
                {
                    keys.arraySize++;
                    values.arraySize++;
                    setProperty(keys.GetArrayElementAtIndex(m_Size), null);
                    setProperty(values.GetArrayElementAtIndex(m_Size), null);
                }
                contentPosition = GetNextPos(ref position);
                contentPosition.width = 230;
                contentPosition.x = (position.width + 14) / 2f - contentPosition.width / 2f;
                if (GUI.Button(contentPosition, "-"))
                {
                    keys.arraySize = Mathf.Max(keys.arraySize - 1, 0);
                    values.arraySize = keys.arraySize;
                }
            }
        }

        private void setProperty(SerializedProperty property, object value)
        {
            switch (property.propertyType)
            {
                case SerializedPropertyType.Integer:
                    property.intValue = Convert.ToInt32(value);
                    break;
                case SerializedPropertyType.String:
                    property.stringValue = Convert.ToString(value);
                    break;
                case SerializedPropertyType.Boolean:
                    property.boolValue = Convert.ToBoolean(value);
                    break;
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (property.isExpanded)
                return base.GetPropertyHeight(property, label) * (m_Size + 6);  // assuming original is one row
            else
                return base.GetPropertyHeight(property, label);
        }
    }
}
#endif
