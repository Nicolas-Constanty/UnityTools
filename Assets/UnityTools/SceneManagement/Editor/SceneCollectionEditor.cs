using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityTools.Inspector;
using UnityTools.SceneManagement.Model;

// ReSharper disable once CheckNamespace
namespace UnityTools.SceneManagement
{
    [CustomEditor(typeof(SceneCollection))]
    // ReSharper disable once CheckNamespace
    public class SceneCollectionEditor : UnityEditor.Editor
    {

        private SceneCollection m_T;
        private SerializedObject m_GetTarget;
        private SerializedProperty m_SceneReferenceProperty;
        private SerializedProperty m_Name;
        private SerializedProperty m_TransitionScene;
        private ReorderableListLayout m_List;

        private static readonly ReorderableListSettings r_DefaultSettings = new ReorderableListSettings()
        {
            Draggable = true,
            DisplayAddButton = true,
            DisplayHeader = true,
            DisplayRemoveButton = true
        };

        // ReSharper disable once UnusedMember.Local
        private void OnEnable()
        {
            m_T = (SceneCollection) target;
            m_GetTarget = new SerializedObject(m_T);
            m_SceneReferenceProperty = m_GetTarget.FindProperty("SceneAssets");
            m_Name = m_GetTarget.FindProperty("CollectionName");
            m_TransitionScene = m_GetTarget.FindProperty("TransitionScene");

            m_List = new ReorderableListLayout(m_GetTarget, m_SceneReferenceProperty, r_DefaultSettings);
            DrawElementList();

            OnSelected();
            if (r_DefaultSettings.DisplayHeader)
                OnHeader();
            if (r_DefaultSettings.DisplayAddButton)
                OnAdd();
            if (r_DefaultSettings.DisplayRemoveButton)
                OnRemove();
        }

        public override void OnInspectorGUI()
        {
            //Update our list
            m_GetTarget.Update();
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.Space(5);
            EditorGUILayout.PropertyField(m_Name, new GUIContent("Name"), EditorStyles.boldFont);
            GUILayout.Space(10);
            OnDrawCollection();
            GUILayout.Space(10);
            EditorGUILayout.LabelField("Transition Scene", EditorStyles.boldLabel);
            GUILayout.Space(5);
            EditorGUI.indentLevel++;
            SerializedProperty mode = m_TransitionScene.FindPropertyRelative("Mode");
            EditorGUILayout.PropertyField(mode);
            if (mode.enumValueIndex == 1)
            {
                GUILayout.Space(2);
                EditorGUILayout.PropertyField(m_TransitionScene.FindPropertyRelative("In"));
                GUILayout.Space(2);
                EditorGUILayout.PropertyField(m_TransitionScene.FindPropertyRelative("Out"));
                GUILayout.Space(2);
                EditorGUILayout.PropertyField(m_TransitionScene.FindPropertyRelative("Scene"));
            }
            else if (mode.enumValueIndex == 0)
            {
                GUILayout.Space(2);
                EditorGUILayout.PropertyField(m_TransitionScene.FindPropertyRelative("Scene"));
            }
            EditorGUI.indentLevel--;
            //EditorGUILayout.PropertyField(m_TransitionScene, new GUIContent("Transition Scene"), true);
            GUILayout.Space(10);
            if (GUILayout.Button("Load Collection"))
                m_T.Load();
            if (GUILayout.Button("Load Additive Collection"))
                m_T.LoadAdditive();
            if (GUILayout.Button("Create from OpenScene"))
                m_T.CopyFromOpenScenes();
            if (GUILayout.Button("Append OpenScene"))
                m_T.AppendOpenScenes();
            GUILayout.Space(5);
            EditorGUILayout.EndVertical();
            m_GetTarget.ApplyModifiedProperties();
        }

        private void OnDrawCollection()
        {
            m_List.DoLayoutList();
        }

        private void OnSelected()
        {
            m_List.onSelectCallback = l => {
                var prefab = l.serializedProperty.GetArrayElementAtIndex(l.index).objectReferenceValue as GameObject;
                if (prefab)
                    EditorGUIUtility.PingObject(prefab.gameObject);
            };
        }

        private void OnHeader()
        {
            m_List.drawHeaderCallback = rect =>
            {
                rect.x -= 5;
                GUIContent label = new GUIContent("Scene Collection");
                EditorGUI.LabelField(rect, label, EditorStyles.boldLabel);
            };
        }

        // ReSharper disable once UnusedMember.Local
        private void OnAdd()
        {
            m_List.onAddCallback = l =>
            {
                var index = l.serializedProperty.arraySize;
                l.serializedProperty.arraySize++;
                l.index = index;
                var element = l.serializedProperty.GetArrayElementAtIndex(index);
                element.objectReferenceValue = null;
            };
        }

        // ReSharper disable once UnusedMember.Local
        private void OnRemove()
        {
            m_List.onCanRemoveCallback = l => l.count >= 1;
            m_List.onRemoveCallback = l =>
            {
                if (EditorUtility.DisplayDialog("Warning!",
                    "Are you sure you want to delete the scene collection?", "Yes", "No"))
                {
                    ReorderableList.defaultBehaviours.DoRemoveButton(l);
                }
            };
        }

        private void DrawElementList()
        {
            m_List.drawElementCallback =
                (rect, index, isActive, isFocused) => {
                    var element = m_List.serializedProperty.GetArrayElementAtIndex(index);
                    if (index != 0)
                        rect.y += 2;
                    EditorGUI.PropertyField(
                        new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight),
                        element, GUIContent.none, true);
                };
        }
    }

}
