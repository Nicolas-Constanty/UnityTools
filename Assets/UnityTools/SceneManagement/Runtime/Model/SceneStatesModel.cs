using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityTools.Inspector;

#if UNITY_EDITOR
using UnityEditor;
#endif

// ReSharper disable once CheckNamespace
namespace UnityTools.SceneManagement.Model
{
    [Serializable]
    public class SceneStatesModel : AModel
    {
        public enum Method
        {
            FreeScript,
            GameManager
        }

        interface IScene
        {
            string Path();
            object Handle();
        }

        [System.Serializable]
        public class SceneHandler : IScene
        {
#if UNITY_EDITOR
            private SceneAsset _handle;
            public SceneHandler(SceneAsset s)
            {
                _handle = s;
            }

            public string Path()
            {
                return AssetDatabase.GetAssetPath(_handle);
            }

            public object Handle()
            {
                return _handle;
            }
#else
            private Scene _handle;

            public SceneHandler(Scene s)
            {
                _handle = s;
            }

            public string Path()
            {
                return _handle.path;
            }

            public object Handle()
            {
                return _handle;
            }
#endif
        }

        [Serializable]
        public struct SceneStatesSettings
        {
            [Serializable]
            public struct SceneState
            {
                public GameManager.State State;
                public SceneHandler Scene;
                public Loading Loading;
                public bool Enabled;
            }

            public enum Loading
            {
                Additive,
                Normal
            }

            public List<SceneState> SceneStates;

            public static SceneStatesSettings DefaultSettings
            {
                get
                {
                    return new SceneStatesSettings
                    {
                        SceneStates = ((Func<List<SceneState>>)(() =>
                            {
                                List<SceneState> st = new List<SceneState>();
                                foreach (GameManager.State state in Enum.GetValues(typeof(GameManager.State)))
                                {
                                    st.Add(new SceneState()
                                    {
                                        State = state,
                                        Scene = null,
                                        Loading = Loading.Additive,
                                        Enabled = true
                                    });
                                }
                                return st;
                            }
                        ))()
                    };
                }
            }
        }

        [Serializable]
        public struct Settings
        {
            public Method Method;
            public SceneStatesSettings SceneStatesSettings;

            public static Settings DefaultSettings
            {
                get
                {
                    return new Settings
                    {
                        Method = Method.GameManager,
                        SceneStatesSettings = SceneStatesSettings.DefaultSettings
                    };
                }
            }
        }

        [SerializeField]
        Settings m_Settings = Settings.DefaultSettings;
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

