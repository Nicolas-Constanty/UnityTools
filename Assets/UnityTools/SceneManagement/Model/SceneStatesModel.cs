using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityTools.Inspector;

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

        [Serializable]
        public struct SceneStatesSettings
        {
            [Serializable]
            public struct SceneState
            {
                public GameManager.State State;
                public SceneAsset Scene;
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

