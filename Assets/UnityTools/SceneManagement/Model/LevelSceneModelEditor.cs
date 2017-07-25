using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityTools.Inspector;
using Settings = UnityTools.SceneManagement.Model.LevelSceneModel.Settings;

// ReSharper disable once CheckNamespace
namespace UnityTools.SceneManagement.Model
{
    [EditorEnable(typeof(LevelSceneModel))]
    public class LevelSceneCollectionEditor : ModelEditor {

        private SerializedProperty m_Collections;
        private ReorderableListLayout m_List;

        private static readonly ReorderableListSettings r_DefaultSettings = new ReorderableListSettings()
        {
            Draggable = true,
            DisplayAddButton = true,
            DisplayHeader = true,
            DisplayRemoveButton = true
        };

        public override void OnEnable()
        {
            m_Collections = FindSetting((Settings x) => x.SceneCollectionSettings.SceneCollection);
            m_List = new ReorderableListLayout(m_SettingsProperty.serializedObject, m_Collections, r_DefaultSettings);
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
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.Space(5);
            OnDrawCollection();
            GUILayout.Space(5);
            EditorGUILayout.EndVertical();
        }

        private void OnDrawCollection()
        {
            m_List.DoLayoutList();
        }

        private void OnSelected()
        {
            m_List.onSelectCallback = l => {
                var prefab = l.serializedProperty.GetArrayElementAtIndex(l.index).FindPropertyRelative("Collection").objectReferenceValue as GameObject;
                if (prefab)
                    EditorGUIUtility.PingObject(prefab.gameObject);
            };
        }

        private void OnHeader()
        {
            m_List.drawHeaderCallback = rect =>
            {
                rect.x -= 5;
                GUIContent label = new GUIContent("Level Collections");
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
                LevelSceneModel.SceneCollectionSettings defaultValue = LevelSceneModel.SceneCollectionSettings.DefaultSettings;
                element.FindPropertyRelative("Collection").objectReferenceValue = null;
                element.FindPropertyRelative("Build").boolValue = defaultValue.SceneCollection[0].Build;
                element.FindPropertyRelative("Enable").boolValue = defaultValue.SceneCollection[0].Enable;
                element.FindPropertyRelative("PriorityLevel").enumValueIndex = (int)defaultValue.SceneCollection[0].PriorityLevel;
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
                    float offset = 0;
                    EditorGUI.PropertyField(
                        new Rect(rect.x, rect.y, 30, EditorGUIUtility.singleLineHeight),
                        element.FindPropertyRelative("Build"), GUIContent.none);
                    offset += 30;
                    EditorGUI.PropertyField(
                        new Rect(rect.x + offset, rect.y, 30, EditorGUIUtility.singleLineHeight),
                        element.FindPropertyRelative("Enable"), GUIContent.none);
                    offset += 30;
                    EditorGUI.PropertyField(
                        new Rect(rect.x + offset, rect.y, 60, EditorGUIUtility.singleLineHeight),
                        element.FindPropertyRelative("PriorityLevel"), GUIContent.none);
                    offset += 60;
                    EditorGUI.PropertyField(
                        new Rect(rect.x + offset, rect.y, rect.width - offset, EditorGUIUtility.singleLineHeight),
                        element.FindPropertyRelative("Collection"), GUIContent.none);

                };
        }
    }
}

