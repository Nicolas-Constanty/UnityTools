using System;
using System.Collections.Generic;
using UnityEngine;
using UnityTools.Inspector;

namespace UnityTools.SceneManagement.Model
{
    [Serializable]
    public class LevelSceneModel : AModel
    {
        [Serializable]
        public class SceneCollectionSettings
        {
            [Serializable]
            public class SceneCollectionEditor
            {
                public SceneCollection Collection;
                public bool Build;
                public bool Enable;
                public Priority PriorityLevel;

                public enum Priority
                {
                    Emergency,
                    Hight,
                    Normal,
                    Low,
                    Script
                }

                public static SceneCollectionEditor DefaultSettings
                {
                    get
                    {
                        return new SceneCollectionEditor()
                        {
                            Collection = null,
                            Build = true,
                            Enable = true,
                            PriorityLevel = Priority.Normal
                        };
                    }
                }
            }

            public List<SceneCollectionEditor> SceneCollection;

            public static SceneCollectionSettings DefaultSettings
            {
                get {
                    return new SceneCollectionSettings()
                    {
                        SceneCollection = ((Func<List<SceneCollectionEditor>>)(() =>
                            {
                                List<SceneCollectionEditor> sc =
                                    new List<SceneCollectionEditor> {SceneCollectionEditor.DefaultSettings};
                                return sc;
                            }
                        ))()
                    };
                }
            }
        }

        [Serializable]
        public struct Settings
        {
            public SceneCollectionSettings SceneCollectionSettings;

            public static Settings DefaultSettings
            {
                get
                {
                    return new Settings
                    {
                        SceneCollectionSettings = SceneCollectionSettings.DefaultSettings
                    };
                }
            }
        }

        [SerializeField]
        private Settings m_Settings = Settings.DefaultSettings;
        public Settings settings
        {
            get { return m_Settings; }
            set { m_Settings = value; }
        }

        public override void Reset()
        {
            m_Settings = Settings.DefaultSettings;
        }
    }
}

