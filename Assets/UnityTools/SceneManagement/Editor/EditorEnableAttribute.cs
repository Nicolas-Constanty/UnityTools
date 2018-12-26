using System;

namespace UnityTools.SceneManagement
{
    public class EditorEnableAttribute : Attribute
    {
        public readonly Type Type;
        public readonly bool AlwaysEnabled;

        public EditorEnableAttribute(Type type, bool alwaysEnabled = false)
        {
            this.Type = type;
            this.AlwaysEnabled = alwaysEnabled;
        }
    }
}