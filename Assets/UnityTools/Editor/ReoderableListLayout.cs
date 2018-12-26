using System;
using System.Collections;
using UnityEditor;
using UnityEditorInternal;

// ReSharper disable once CheckNamespace
namespace UnityTools.Inspector
{
    public class ReorderableListSettings
    {
        public bool Draggable;
        public bool DisplayHeader;
        public bool DisplayAddButton;
        public bool DisplayRemoveButton;

    }
    public class ReorderableListLayout : ReorderableList, IEnumerator, IEnumerable
    {
        private int m_Position;

        public ReorderableListLayout(IList elements, Type elementType) : base(elements, elementType)
        {
            m_Position = -1;
        }

        public ReorderableListLayout(IList elements, Type elementType, bool draggable, bool displayHeader, bool displayAddButton, bool displayRemoveButton) : base(elements, elementType, draggable, displayHeader, displayAddButton, displayRemoveButton)
        {
            m_Position = -1;
        }

        public ReorderableListLayout(SerializedObject serializedObject, SerializedProperty elements) : base(serializedObject, elements)
        {
            m_Position = -1;
        }

        public ReorderableListLayout(SerializedObject serializedObject, SerializedProperty elements, bool draggable, bool displayHeader, bool displayAddButton, bool displayRemoveButton) : base(serializedObject, elements, draggable, displayHeader, displayAddButton, displayRemoveButton)
        {
            m_Position = -1;
        }

        public ReorderableListLayout(SerializedObject serializedObject, SerializedProperty elements, ReorderableListSettings settings) : base(
            serializedObject, elements, settings.Draggable, settings.DisplayHeader, settings.DisplayAddButton, settings.DisplayRemoveButton)
        {
            m_Position = -1;
        }

        public bool MoveNext()
        {
            m_Position++;
            return (m_Position < serializedProperty.arraySize);
        }

        public void Reset()
        {
            m_Position = 0;
        }

        public object Current
        {
            get { return serializedProperty.GetArrayElementAtIndex(m_Position); }
        }
        public IEnumerator GetEnumerator()
        {
            return this;
        }
    }
}
