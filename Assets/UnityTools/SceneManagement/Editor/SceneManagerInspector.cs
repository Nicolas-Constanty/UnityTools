using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityTools.Inspector;
using UnityTools.SceneManagement.Model;

// ReSharper disable once CheckNamespace
namespace UnityTools.SceneManagement
{
    [CustomEditor(typeof(SceneManagerProfile))]
    public class SceneManagerInspector : Editor
    {
        // ReSharper disable once UnusedMember.Local
        private void OnEnable()
        {
            if (target == null)
                return;

            // Aggregate custom post-fx editors
            var assembly = Assembly.GetAssembly(typeof(SceneManagerInspector));

            var editorTypes = assembly.GetTypes()
                .Where(x => x.IsDefined(typeof(EditorEnableAttribute), false));

            var customEditors = new Dictionary<Type, ModelEditor>();
            foreach (var editor in editorTypes)
            {
                var attr = (EditorEnableAttribute) editor.GetCustomAttributes(typeof(EditorEnableAttribute), false)[0];
                var effectType = attr.Type;
                var alwaysEnabled = attr.AlwaysEnabled;

                var editorInst = (ModelEditor) Activator.CreateInstance(editor);
                editorInst.alwaysEnabled = alwaysEnabled;
                editorInst.profile = target as SceneManagerProfile;
                editorInst.inspector = this;
                customEditors.Add(effectType, editorInst);
            }

            // ... and corresponding models
            var baseType = target.GetType();
            var property = serializedObject.GetIterator();

            while (property.Next(true))
            {
                if (!property.hasChildren)
                    continue;

                var type = baseType;
                var srcObject = ReflectionUtils.GetFieldValueFromPath(serializedObject.targetObject, ref type, property.propertyPath);

                if (srcObject == null)
                    continue;
                ModelEditor editor;
                if (customEditors.TryGetValue(type, out editor))
                {
                    var effect = (AModel)srcObject;

                    if (editor.alwaysEnabled)
                        effect.Enabled = editor.alwaysEnabled;

                    m_CustomEditors.Add(editor, effect);
                    editor.target = effect;
                    editor.serializedProperty = property.Copy();
                    editor.OnPreEnable();
                }
            }
        }
        
        // ReSharper disable once UnusedMember.Local
        private void OnDisable()
        {
            if (m_CustomEditors != null)
            {
                foreach (var editor in m_CustomEditors.Keys)
                    editor.OnDisable();

                m_CustomEditors.Clear();
            }
        }

        readonly Dictionary<ModelEditor, AModel> m_CustomEditors = new Dictionary<ModelEditor, AModel>();
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // Handles undo/redo events first (before they get used by the editors' widgets)
            var e = Event.current;
            if (e.type == EventType.ValidateCommand && e.commandName == "UndoRedoPerformed")
            {
                foreach (var editor in m_CustomEditors)
                    editor.Value.OnValidate();
            }

            foreach (var editor in m_CustomEditors)
            {
                EditorGUI.BeginChangeCheck();
                editor.Key.OnGUI();
                if (EditorGUI.EndChangeCheck())
                    editor.Value.OnValidate();
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}