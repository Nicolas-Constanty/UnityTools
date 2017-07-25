using UnityEngine;

// ReSharper disable once CheckNamespace
namespace UnityTools.SceneManagement.Style
{
    public class Header
    {
        public GUIStyle HeaderStyle;
        public GUIStyle HeaderCheckbox;
        public GUIStyle HeaderFoldout;

        public Header()
        {
            HeaderStyle = new GUIStyle("ShurikenModuleTitle")
            {
                font = (new GUIStyle("Label")).font,
                border = new RectOffset(15, 7, 4, 4),
                fixedHeight = 22,
                contentOffset = new Vector2(20f, -2f)
            };

            HeaderCheckbox = new GUIStyle("ShurikenCheckMark");
            HeaderFoldout = new GUIStyle("Foldout");
        }
    }
}

