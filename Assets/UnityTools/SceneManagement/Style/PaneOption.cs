using UnityEditor;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace UnityTools.SceneManagement.Style
{
    public class PaneOption
    {

        public Texture2D Icon;

        public PaneOption()
        {
            if (EditorGUIUtility.isProSkin)
                Icon = (Texture2D)EditorGUIUtility.LoadRequired("Builtin Skins/DarkSkin/Images/pane options.png");
            else
                Icon = (Texture2D)EditorGUIUtility.LoadRequired("Builtin Skins/LightSkin/Images/pane options.png");
        }
    }
}

