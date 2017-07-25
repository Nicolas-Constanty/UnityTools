using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityTools.Common;
using UnityTools.SceneManagement.Style;

// ReSharper disable once CheckNamespace
namespace UnityTools.SceneManagement
{
    // ReSharper disable once InconsistentNaming
    public static class EditorGUITools
    {
        static EditorGUITools()
        {
            r_GuiContentCache = new Dictionary<string, GUIContent>();
        }

        #region GUIContent caching

        static readonly Dictionary<string, GUIContent> r_GuiContentCache;

        public static GUIContent GetContent(string textAndTooltip)
        {
            if (string.IsNullOrEmpty(textAndTooltip))
                return GUIContent.none;

            GUIContent content;

            if (!r_GuiContentCache.TryGetValue(textAndTooltip, out content))
            {
                var s = textAndTooltip.Split('|');
                content = new GUIContent(s[0]);

                if (s.Length > 1 && !string.IsNullOrEmpty(s[1]))
                    content.tooltip = s[1];

                r_GuiContentCache.Add(textAndTooltip, content);
            }

            return content;
        }

        #endregion

        public static bool Header(string title, SerializedProperty group, Action resetAction)
        {
            var rect = GUILayoutUtility.GetRect(16f, 22f, SceneManagementStyles.Header.HeaderStyle);
            GUI.Box(rect, title, SceneManagementStyles.Header.HeaderStyle);

            var display = group == null || group.isExpanded;

            var foldoutRect = new Rect(rect.x + 4f, rect.y + 2f, 13f, 13f);
            var e = Event.current;

            var popupRect = new Rect(rect.x + rect.width - SceneManagementStyles.PaneOption.Icon.width - 5f,
                rect.y + SceneManagementStyles.PaneOption.Icon.height / 2f + 1f,
                SceneManagementStyles.PaneOption.Icon.width, SceneManagementStyles.PaneOption.Icon.height);

            GUI.DrawTexture(popupRect, SceneManagementStyles.PaneOption.Icon);

            if (e.type == EventType.Repaint)
                SceneManagementStyles.Header.HeaderFoldout.Draw(foldoutRect, false, false, display, false);

            if (e.type == EventType.MouseDown)
            {
                if (popupRect.Contains(e.mousePosition))
                {
                    var popup = new GenericMenu();
                    popup.AddItem(GetContent("Reset"), false, () => resetAction());
                    popup.AddSeparator(string.Empty);
                    popup.AddItem(GetContent("Copy Settings"), false, () => CopySettings(group));

                    if (CanPaste(group))
                        popup.AddItem(GetContent("Paste Settings"), false, () => PasteSettings(group));
                    else
                        popup.AddDisabledItem(GetContent("Paste Settings"));

                    popup.ShowAsContext();
                }
                else if (rect.Contains(e.mousePosition) && group != null)
                {
                    display = !display;

                    if (group != null)
                        group.isExpanded = !group.isExpanded;

                    e.Use();
                }
            }
            return display;
        }

        public static bool Header(string title, SerializedProperty group, SerializedProperty enabledField, Action resetAction)
        {
            var field = ReflectionUtils.GetFieldInfoFromPath(enabledField.serializedObject.targetObject, enabledField.propertyPath);
            object parent = null;
            PropertyInfo prop = null;

            if (field != null && field.IsDefined(typeof(GetSetAttribute), false))
            {
                var attr = (GetSetAttribute)field.GetCustomAttributes(typeof(GetSetAttribute), false)[0];
                parent = ReflectionUtils.GetParentObject(enabledField.propertyPath, enabledField.serializedObject.targetObject);
                prop = parent.GetType().GetProperty(attr.Name);
            }

            var display = group == null || group.isExpanded;
            var enabled = enabledField.boolValue;

            var rect = GUILayoutUtility.GetRect(16f, 22f, SceneManagementStyles.Header.HeaderStyle);
            GUI.Box(rect, title, SceneManagementStyles.Header.HeaderStyle);

            var toggleRect = new Rect(rect.x + 4f, rect.y + 4f, 13f, 13f);
            var e = Event.current;

            var popupRect = new Rect(rect.x + rect.width - SceneManagementStyles.PaneOption.Icon.width - 5f, rect.y + SceneManagementStyles.PaneOption.Icon.height / 2f + 1f, SceneManagementStyles.PaneOption.Icon.width, SceneManagementStyles.PaneOption.Icon.height);
            GUI.DrawTexture(popupRect, SceneManagementStyles.PaneOption.Icon);

            if (e.type == EventType.Repaint)
                SceneManagementStyles.Header.HeaderCheckbox.Draw(toggleRect, false, false, enabled, false);

            if (e.type == EventType.MouseDown)
            {
                const float kOffset = 2f;
                toggleRect.x -= kOffset;
                toggleRect.y -= kOffset;
                toggleRect.width += kOffset * 2f;
                toggleRect.height += kOffset * 2f;

                if (toggleRect.Contains(e.mousePosition))
                {
                    enabledField.boolValue = !enabledField.boolValue;

                    if (prop != null)
                        prop.SetValue(parent, enabledField.boolValue, null);

                    e.Use();
                }
                else if (popupRect.Contains(e.mousePosition))
                {
                    var popup = new GenericMenu();
                    popup.AddItem(GetContent("Reset"), false, () => resetAction());
                    popup.AddSeparator(string.Empty);
                    popup.AddItem(GetContent("Copy Settings"), false, () => CopySettings(group));

                    if (CanPaste(group))
                        popup.AddItem(GetContent("Paste Settings"), false, () => PasteSettings(group));
                    else
                        popup.AddDisabledItem(GetContent("Paste Settings"));

                    popup.ShowAsContext();
                }
                else if (rect.Contains(e.mousePosition) && group != null)
                {
                    display = !display;
                    group.isExpanded = !group.isExpanded;
                    e.Use();
                }
            }

            return display;
        }

        static void CopySettings(SerializedProperty settings)
        {
            var t = typeof(SceneManagerProfile);
            var settingsStruct = ReflectionUtils.GetFieldValueFromPath(settings.serializedObject.targetObject, ref t, settings.propertyPath);
            var serializedString = t.ToString() + '|' + JsonUtility.ToJson(settingsStruct);
            EditorGUIUtility.systemCopyBuffer = serializedString;
        }

        static bool CanPaste(SerializedProperty settings)
        {
            var data = EditorGUIUtility.systemCopyBuffer;

            if (string.IsNullOrEmpty(data))
                return false;

            var parts = data.Split('|');

            if (string.IsNullOrEmpty(parts[0]))
                return false;

            var field = ReflectionUtils.GetFieldInfoFromPath(settings.serializedObject.targetObject, settings.propertyPath);
            return parts[0] == field.FieldType.ToString();
        }

        static void PasteSettings(SerializedProperty settings)
        {
            Undo.RecordObject(settings.serializedObject.targetObject, "Paste effect settings");
            var field = ReflectionUtils.GetFieldInfoFromPath(settings.serializedObject.targetObject, settings.propertyPath);
            var json = EditorGUIUtility.systemCopyBuffer.Substring(field.FieldType.ToString().Length + 1);
            var obj = JsonUtility.FromJson(json, field.FieldType);
            var parent = ReflectionUtils.GetParentObject(settings.propertyPath, settings.serializedObject.targetObject);
            field.SetValue(parent, obj, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, CultureInfo.CurrentCulture);
        }
    }
}
