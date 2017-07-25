using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityTools.DesignPatern;
using UnityTools.SceneManagement;
using UnityTools.SceneManagement.Model;
using LevelScene = UnityTools.SceneManagement.Model.LevelSceneModel.SceneCollectionSettings.SceneCollectionEditor;

// ReSharper disable once CheckNamespace
namespace UnityTools
{
    public class SceneManager : Singleton<SceneManager>
    {
        public SceneManagerProfile SceneManagerProfile;
        public bool AutoLoadFirstLevel = true;
        [System.Serializable]
        public class FloatEvent : UnityEvent<float> { }
        [HideInInspector]
        public FloatEvent LoadingEvent;
        [HideInInspector]
        public FloatEvent TransitionInEvent;
        [HideInInspector]
        public FloatEvent TransitionOutEvent;


        private List<LevelScene> m_LevelCollectionEditor;
        private List<SceneStatesModel.SceneStatesSettings.SceneState> m_SceneStates;

        private readonly List<Scene> m_LoadedScenes = new List<Scene>();
        private LevelScene m_LastLevel;
        private int m_CurrentLevel;

        // ReSharper disable once UnusedMember.Local
        private void Start()
        {
            if (!CheckProfile()) return;
            m_LevelCollectionEditor = SceneManagerProfile.LevelSceneModel.settings.SceneCollectionSettings.SceneCollection;
            m_SceneStates = SceneManagerProfile.SceneStatesModel.settings.SceneStatesSettings.SceneStates;
            if (AutoLoadFirstLevel)
                LoadLevel(m_LevelCollectionEditor[m_CurrentLevel++]);
        }

        public void LoadLevel(int levelIndex)
        {
            LoadLevel(m_LevelCollectionEditor[levelIndex]);
        }

        public static void LoadLevel(LevelScene level)
        {
            Instance.StartCoroutine("LoadAsyncLevel", level);
        }

        // ReSharper disable once UnusedMember.Local
        private IEnumerator LoadAsyncLevel(LevelScene level)
        {
           
            if (m_LastLevel != null && level != m_LastLevel && m_LastLevel.Collection.TransitionScene.Scene)
            {
                var transition = m_LastLevel.Collection.TransitionScene;
                AsyncOperation loadingLevel = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(AssetDatabase.GetAssetPath(transition.Scene), LoadSceneMode.Additive);
                while (!loadingLevel.isDone)
                {
                    yield return null;
                }
               
                if (transition.Mode == SceneCollection.TransitionSceneSettings.TransitionMode.Curve)
                {
                    float time = 0;
                    float max = transition.In.keys[transition.In.length - 1].time;
                    while (time < max)
                    {
                        float value = transition.In.Evaluate(time);
                        TransitionInEvent.Invoke(value);
                        time += Time.deltaTime;
                        yield return new WaitForEndOfFrame();
                    }
                }
                UnloadLevel(m_LastLevel);
            }
            int chunck = level.Collection.SceneReferences.Count;
            float percent = 0;
            for (int i = 0; i < chunck; i++)
            {
                var levelScene = level.Collection.SceneReferences[i];
               var async = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(AssetDatabase.GetAssetPath(levelScene), LoadSceneMode.Additive);
                while (!async.isDone)
                {
                    percent += Mathf.Clamp01(async.progress / .9f);
                    LoadingEvent.Invoke(percent / chunck);
                    yield return null;
                }
            }
            LoadingEvent.Invoke(1);
            if (m_LastLevel != null && level != m_LastLevel && m_LastLevel.Collection.TransitionScene.Scene)
            {
                var transition = m_LastLevel.Collection.TransitionScene;
                if (transition.Mode == SceneCollection.TransitionSceneSettings.TransitionMode.Curve)
                {
                    float time = 0;
                    float max = transition.In.keys[transition.Out.length - 1].time;
                    while (time < max)
                    {
                        float value = transition.Out.Evaluate(time);
                        TransitionOutEvent.Invoke(value);
                        time += Time.deltaTime;
                        yield return null;
                    }
                }
                UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(AssetDatabase.GetAssetPath(transition.Scene));
            }
            m_LastLevel = level;
            LoadingEvent.Invoke(0);
        }

        private void UnloadLevel(LevelScene level)
        {
            foreach (SceneAsset t in level.Collection.SceneReferences)
            {
                UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(AssetDatabase.GetAssetPath(t));
            }
        }

        private struct SceneInfo
        {
            public readonly LevelScene Level;
            public readonly SceneAsset Scene;
            public readonly LoadSceneMode Mode;

            // ReSharper disable once UnusedMember.Local
            public SceneInfo(LevelScene level, SceneAsset scene, LoadSceneMode mode) : this()
            {
                Level = level;
                Scene = scene;
                Mode = mode;
            }
        }

        //private static void LoadSceneLevel(LevelScene l, SceneAsset s, LoadSceneMode mode = LoadSceneMode.Single)
        //{
        //    string scenePath = AssetDatabase.GetAssetPath(s);
        //    if (Instance.IsLoaded(scenePath)) return;
        //    UnityEngine.SceneManagement.SceneManager.sceneLoaded += SceneManagerOnSceneLoaded;
        //    Instance.StartCoroutine("LoadCoroutine", new SceneInfo() { Level = l, Scene = s, Mode = mode });
        //}

        //private static void LoadMultipleSceneAdditive(IEnumerable<string> scenes)
        //{
        //    foreach (var scene in scenes)
        //    {
        //        LoadScene(scene, LoadSceneMode.Additive);
        //    }
        //}

        //// ReSharper disable once UnusedMember.Local
        //private static void LoadMultipleScene(IEnumerable<string> scenes)
        //{
        //    foreach (var scene in scenes)
        //    {
        //        LoadScene(scene);
        //    }
        //}

        private bool CheckProfile()
        {
            if (SceneManagerProfile == null)
            {
                Debug.LogWarning("Assign SceneManagerProfile before press play");
                return false;
            }
            return true;
        }

        //private static void LoadScene(string scene, LoadSceneMode mode = LoadSceneMode.Single)
        //{
        //    if (Instance.IsLoaded(scene)) return;
        //    UnityEngine.SceneManagement.SceneManager.sceneLoaded += SceneManagerOnSceneLoaded;
        //    Instance.StartCoroutine("LoadCooutine", new SceneInfo() {Path = scene, Mode = mode});
        //    //Instance.StartCoroutine("LoadCoroutine", new LoadSceneManager.SceneInfo() { SceneName = scene, Mode = mode });
        //}

        private static void SceneManagerOnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            UnityEngine.SceneManagement.SceneManager.sceneLoaded -= SceneManagerOnSceneLoaded;
            Instance.AddScene(scene);
            UnityEngine.SceneManagement.SceneManager.SetActiveScene(scene);
        }

        private void AddScene(Scene scene)
        {
            m_LoadedScenes.Add(scene);
        }

        //private bool IsLoaded(string sceneName)
        //{
        //    foreach (var scene in m_LoadedScenes)
        //    {
        //        if (scene.name == sceneName)
        //            return true;
        //    }
        //    return false;
        //}

        public void UnloadScenes()
        {
            foreach (var scene in m_LoadedScenes)
            {
                UnloadScene(scene.name);
            }
        }

        private static void UnloadScene(string scene)
        {
            UnityEngine.SceneManagement.SceneManager.sceneUnloaded += SceneManagerOnSceneUnloaded;
            UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(scene);
        }

        private static void SceneManagerOnSceneUnloaded(Scene scene)
        {
            UnityEngine.SceneManagement.SceneManager.sceneUnloaded -= SceneManagerOnSceneUnloaded;
            Instance.RemoveScene(scene);
        }

        private void RemoveScene(Scene scene)
        {
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += SceneManagerOnSceneLoaded;
            m_LoadedScenes.Remove(scene);
        }

        // ReSharper disable once UnusedMember.Local
        private IEnumerator LoadCoroutine(SceneInfo info)
        {
            if (m_LastLevel != null && info.Level != m_LastLevel && m_LastLevel.Collection.TransitionScene.Scene)
            {
                print("Transition");
                AsyncOperation loadingLevel = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(AssetDatabase.GetAssetPath(m_LastLevel.Collection.TransitionScene.Scene), LoadSceneMode.Additive);
                while (!loadingLevel.isDone)
                {
                    yield return new  WaitForSeconds(2);
                }
            }
            m_LastLevel = info.Level;
            AsyncOperation loading = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(AssetDatabase.GetAssetPath(info.Scene), info.Mode);
            while (!loading.isDone)
            {
                float percent = Mathf.Clamp01(loading.progress / .9f);
                LoadingEvent.Invoke(percent);
                yield return null;
            }
            LoadingEvent.Invoke(0);
        }

        public void LoadNextLevel()
        {
            if (m_LevelCollectionEditor == null || m_LevelCollectionEditor.Count == 0 || m_CurrentLevel >= m_LevelCollectionEditor.Count) return;
            LoadLevel(m_LevelCollectionEditor[m_CurrentLevel++]);
        }

        //public void LoadPreviousLevel()
        //{
        //    if (Levels.Length == 0 || _currentLevel == 0) return;
        //    if (_currentLevelName != null)
        //        UnloadScene(_currentLevelName);
        //    _currentLevelName = Levels[_currentLevel--];
        //    LoadScene(_currentLevelName, LoadSceneMode.Additive);
        //}

        //public void LoadLevelIndex(uint idx)
        //{
        //    if (Levels.Length == 0 || _currentLevel >= Levels.Length) return;
        //    _currentLevel = idx;
        //    _currentLevelName = Levels[_currentLevel];
        //    LoadScene(_currentLevelName);
        //}


        //public void RestartGame()
        //{
        //    if (Levels.Length == 0 || _currentLevel == 0) return;
        //    if (_currentLevelName != null)
        //        UnloadScene(_currentLevelName);
        //    _currentLevel = 0;
        //    _currentLevelName = Levels[_currentLevel++];
        //    LoadScene(_currentLevelName, LoadSceneMode.Additive);
        //}
    }
}


