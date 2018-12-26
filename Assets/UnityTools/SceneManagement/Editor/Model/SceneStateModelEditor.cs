using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityTools.Inspector;
using Settings = UnityTools.SceneManagement.Model.SceneStatesModel.Settings;

// ReSharper disable once CheckNamespace
namespace UnityTools.SceneManagement.Model
{
    [EditorEnable(typeof(SceneStatesModel))]
    public class SceneStateModelEditor : ModelEditor
    {
        private SerializedProperty m_Method;
        private SerializedProperty m_StateCollection;
        static readonly string[] r_MethodNames =
        {
            "Free Script States",
            "GameManager States"
        };

        private static readonly ReorderableListSettings r_DefaultSettings = new ReorderableListSettings()
        {
            Draggable = true,
            DisplayAddButton = false,
            DisplayHeader = true,
            DisplayRemoveButton = false
        };
        

        private ReorderableListLayout m_List;

        //Called when SceneStateModel is enable
        public override void OnEnable()
        {
            m_Method = FindSetting((Settings x) => x.Method);
            m_StateCollection = FindSetting((Settings x) => x.SceneStatesSettings.SceneStates);
            m_List = new ReorderableListLayout(m_SettingsProperty.serializedObject, m_StateCollection, r_DefaultSettings);
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
            DisplaySceneMode();
            GUILayout.Space(5);
            switch (m_Method.intValue)
            {
                case (int) SceneStatesModel.Method.FreeScript:
                    break;
                case (int) SceneStatesModel.Method.GameManager:
                    OnGameManagerState();
                    break;
            }
            GUILayout.Space(5);
            EditorGUILayout.EndVertical();
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
                        element.FindPropertyRelative("Enabled"), GUIContent.none);
                    offset += 30;
                    EditorGUI.PropertyField(
                        new Rect(rect.x + offset, rect.y, 80, EditorGUIUtility.singleLineHeight),
                        element.FindPropertyRelative("State"), GUIContent.none);
                    offset += 80;
                    EditorGUI.PropertyField(
                        new Rect(rect.x + offset, rect.y, 60, EditorGUIUtility.singleLineHeight),
                        element.FindPropertyRelative("Loading"), GUIContent.none);
                    offset += 60;
                    EditorGUI.PropertyField(
                        new Rect(rect.x + offset, rect.y, rect.width - offset, EditorGUIUtility.singleLineHeight),
                        element.FindPropertyRelative("Scene"), GUIContent.none);

                };
        }

        private void OnSelected()
        {
            m_List.onSelectCallback = l => {
                var prefab = l.serializedProperty.GetArrayElementAtIndex(l.index).FindPropertyRelative("Scene").objectReferenceValue as GameObject;
                if (prefab)
                    EditorGUIUtility.PingObject(prefab.gameObject);
            };
        }

        private void OnHeader()
        {
            m_List.drawHeaderCallback = rect =>
            {
                rect.x -= 5;
                GUIContent label = new GUIContent("Scene States");
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
                SceneStatesModel.SceneStatesSettings defaultValue = SceneStatesModel.SceneStatesSettings.DefaultSettings;
                element.FindPropertyRelative("State").enumValueIndex = (int)defaultValue.SceneStates[0].State;
                element.FindPropertyRelative("Scene").objectReferenceValue = (Object) defaultValue.SceneStates[0].Scene.Handle();
                element.FindPropertyRelative("Loading").enumValueIndex = (int)defaultValue.SceneStates[0].Loading;
                element.FindPropertyRelative("Enabled").boolValue = defaultValue.SceneStates[0].Enabled;
            };
        }

        // ReSharper disable once UnusedMember.Local
        private void OnRemove()
        {
            m_List.onCanRemoveCallback = l => l.count > 1;
            m_List.onRemoveCallback = l =>
            {
                if (EditorUtility.DisplayDialog("Warning!",
                    "Are you sure you want to delete the scene state?", "Yes", "No"))
                {
                    ReorderableList.defaultBehaviours.DoRemoveButton(l);
                }
            };
        }

        private void DisplaySceneMode()
        {
            EditorGUILayout.LabelField("Mode", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            m_Method.intValue = EditorGUILayout.Popup(m_Method.intValue, r_MethodNames);
            EditorGUI.indentLevel--;
        }

        private void ToogleScenes(bool value)
        {
            for (int i = 0; i < m_List.serializedProperty.arraySize; i++)
            {
                var element = m_List.serializedProperty.GetArrayElementAtIndex(i);
                element.FindPropertyRelative("Enabled").boolValue = value;
            }
        }

        private void OnGameManagerState()
        {
            m_List.DoLayoutList();
            GUICenter.Button("Load all scenes", () => ToogleScenes(true));
            GUICenter.Button("Unload all scenes", () => ToogleScenes(false));
        }
    }
}

