using System;
using UnityEditor;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace UnityTools.Inspector
{
    // ReSharper disable once InconsistentNaming
    public static class GUICenter
    {
        public static void Button(string text, Action action)
        {
            GUIContent btnTxt = new GUIContent(text);
            var r = GUILayoutUtility.GetRect(btnTxt, GUI.skin.button, GUILayout.ExpandWidth(false));
            r.center = new Vector2(EditorGUIUtility.currentViewWidth / 2, r.center.y);
            if (!GUI.Button(r, btnTxt, GUI.skin.button)) return;
            action();
        }
    }
}

