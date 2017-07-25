using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityTools.DesignPatern;

// ReSharper disable once CheckNamespace
namespace UnityTools.SceneManagement
{
    // ReSharper disable once CheckNamespace
    public class __LoadSceneManager : Singleton<__LoadSceneManager>
    {
        [Tooltip("Base scenes are load with additive parameter")] public string[] BaseScenes;

        [Tooltip("One is load in addition with base scenes")] public string[] Levels;

        public bool ExecInEditor = false;
        [System.Serializable]
        public class FloatEvent : UnityEvent<float> { }
        public FloatEvent LoadingEvent;

        private uint m_CurrentLevel;
        private string m_CurrentLevelName;

        private readonly List<Scene> m_LoadedScenes = new List<Scene>();

        //#if !UNITY_EDITOR
        // ReSharper disable once UnusedMember.Local
        private void Start()
        {
            for (int i = 0; i < UnityEngine.SceneManagement.SceneManager.sceneCount; i++)
            {
                m_LoadedScenes.Add(UnityEngine.SceneManagement.SceneManager.GetSceneAt(i));
            }
            if (!Application.isEditor || ExecInEditor)
            {
                LoadMultipleSceneAdditive(BaseScenes);
                LoadNextLevel();
            }
        }

        private static void LoadMultipleSceneAdditive(IEnumerable<string> scenes)
        {
            foreach (var scene in scenes)
            {
                LoadScene(scene, LoadSceneMode.Additive);
            }
        }

        // ReSharper disable once UnusedMember.Local
        private static void LoadMultipleScene(IEnumerable<string> scenes)
        {
            foreach (var scene in scenes)
            {
                LoadScene(scene);
            }
        }

        private bool IsLoaded(string sceneName)
        {
            foreach (var scene in m_LoadedScenes)
            {
                if (scene.name == sceneName)
                    return true;
            }
            return false;
        }
        public class SceneInfo
        {
            public string SceneName;
            public LoadSceneMode Mode;
        }
        private static void LoadScene(string scene, LoadSceneMode mode = LoadSceneMode.Single)
        {
            if (Instance.IsLoaded(scene)) return;
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += SceneManagerOnSceneLoaded;
            Instance.StartCoroutine("LoadCoroutine", new SceneInfo(){ SceneName = scene, Mode = mode});
        }

        private static void SceneManagerOnSceneLoaded(Scene arg0, LoadSceneMode loadSceneMode)
        {
            UnityEngine.SceneManagement.SceneManager.sceneLoaded -= SceneManagerOnSceneLoaded;
            //Fading fade = Instance.GetComponent<Fading>();
            //if (fade != null)
            //    fade.OnLevelWasMoaded();
            Instance.AddScene(arg0);
            UnityEngine.SceneManagement.SceneManager.SetActiveScene(arg0);
        }

        private void AddScene(Scene arg0)
        {
            m_LoadedScenes.Add(arg0);
        }

        private void RemoveScene(Scene arg0)
        {
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += SceneManagerOnSceneLoaded;
            m_LoadedScenes.Remove(arg0);
        }

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
            //Fading fade = Instance.GetComponent<Fading>();
            //if (fade != null)
            //    fade.BeginFade(1);
            UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(scene);
        }

        // ReSharper disable once UnusedMember.Local
        private IEnumerator LoadCoroutine(SceneInfo info)
        {
            //Fading fade = Instance.GetComponent<Fading>();
            float fadeTime = Time.fixedDeltaTime;
            //if (fade != null)
            //    fadeTime = fade.BeginFade(1);
            yield return new WaitForSeconds(fadeTime);
            AsyncOperation loading =  UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(info.SceneName, info.Mode);
            while (!loading.isDone)
            {
                float percent = Mathf.Clamp01(loading.progress / .9f);
                LoadingEvent.Invoke(percent);
                yield return null;
            }
            LoadingEvent.Invoke(0);
        }

        private static void SceneManagerOnSceneUnloaded(Scene arg0)
        {
            UnityEngine.SceneManagement.SceneManager.sceneUnloaded -= SceneManagerOnSceneUnloaded;
            Instance.RemoveScene(arg0);
        }

        public void LoadNextLevel()
        {
            if (Levels == null || Levels.Length == 0 || m_CurrentLevel >= Levels.Length) return;
            if (m_CurrentLevelName != null)
                UnloadScene(m_CurrentLevelName);
            m_CurrentLevelName = Levels[m_CurrentLevel++];
            LoadScene(m_CurrentLevelName, LoadSceneMode.Additive);
        }

        public void LoadPreviousLevel()
        {
            if (Levels.Length == 0 || m_CurrentLevel == 0) return;
            if (m_CurrentLevelName != null)
                UnloadScene(m_CurrentLevelName);
            m_CurrentLevelName = Levels[m_CurrentLevel--];
            LoadScene(m_CurrentLevelName, LoadSceneMode.Additive);
        }

        public void LoadLevelIndex(uint idx)
        {
            if (Levels.Length == 0 || m_CurrentLevel >= Levels.Length) return;
            m_CurrentLevel = idx;
            m_CurrentLevelName = Levels[m_CurrentLevel];
            LoadScene(m_CurrentLevelName);
        }

        public void RestartGame()
        {
            if (Levels.Length == 0 || m_CurrentLevel == 0) return;
            if (m_CurrentLevelName != null)
                UnloadScene(m_CurrentLevelName);
            m_CurrentLevel = 0;
            m_CurrentLevelName = Levels[m_CurrentLevel++];
            LoadScene(m_CurrentLevelName, LoadSceneMode.Additive);
        }
    }
}