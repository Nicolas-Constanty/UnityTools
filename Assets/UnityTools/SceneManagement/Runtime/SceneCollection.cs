using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using UnityEngine;
using UnityEngine.UI;
using UnityTools.Collections;
using UnityTools.SceneManagement.Model;


#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

// ReSharper disable once CheckNamespace
namespace UnityTools.SceneManagement
{
    [CreateAssetMenu(fileName = "SceneCollection", menuName = "SceneManagement/Scene Collection")]
    public class SceneCollection : ScriptableObject
    {
        public bool Expand;
        public struct SceneAssetInfo
        {
            public SceneStatesModel.SceneHandler SceneAsset;
            public readonly string Path;

            public SceneAssetInfo(string p)
            {
                Path = p;
                SceneAsset = null;
            }
        }

        public string CollectionName = "Collection Name";
        public List<string> SceneReferences = new List<string>();

        public TransitionSceneSettings TransitionScene;

        [System.Serializable]
        public class TransitionSceneSettings
        {
            public enum TransitionMode
            {
                Auto,
                Curve,
                Custom
            }

            public TransitionMode Mode;
            public AnimationCurve In = AnimationCurve.EaseInOut(0, 0, 1 , 1);
            public AnimationCurve Out = AnimationCurve.EaseInOut(0, 1, 1, 0);

            public string scenePath;
#if UNITY_EDITOR
            public SceneAsset Scene;
#endif
        }

#if UNITY_EDITOR

        public List<SceneAsset> SceneAssets = new List<SceneAsset>();

        public void OnValidate()
        {
            TransitionScene.scenePath = TransitionScene.Scene ? AssetDatabase.GetAssetPath(TransitionScene.Scene) : "";
            SceneReferences.Clear();
            foreach (SceneAsset sceneAsset in SceneAssets)
            {
                SceneReferences.Add(AssetDatabase.GetAssetPath(sceneAsset));
            }
        }

        public void CopyFromOpenScenes()
        {
            if (SceneAssets.Count > 0)
            {
                SceneAssets.Clear();
                SceneReferences.Clear();
            }
            AppendOpenScenes();
        }

        public void AppendOpenScenes()
        {
            for (int i = 0; i < EditorSceneManager.loadedSceneCount; i++)
            {
                string path = EditorSceneManager.GetSceneAt(i).path;
                var sa = AssetDatabase.LoadAssetAtPath<SceneAsset>(path);
                SceneAssets.Add(sa);
                SceneReferences.Add(AssetDatabase.GetAssetPath(sa));
                AddToBuild(sa);
            }
        }

        public void Load()
        {
            for (int i = 0; i < SceneAssets.Count; i++)
            {
                SceneAsset sa = SceneAssets[i];
                EditorSceneManager.OpenScene(AssetDatabase.GetAssetPath(sa), i == 0 ? OpenSceneMode.Single : OpenSceneMode.Additive);
                AddToBuild(sa);
            }
        }

        public void LoadAdditive()
        {
            foreach (var scene in SceneAssets)
            {
                EditorSceneManager.OpenScene(AssetDatabase.GetAssetPath(scene), OpenSceneMode.Additive);
                AddToBuild(scene);
            }
        }

        public void AddToBuild(SceneAsset sa)
        {
            EditorBuildSettingsScene[] oldSettings = EditorBuildSettings.scenes;
            List<EditorBuildSettingsScene> newSettings = new List<EditorBuildSettingsScene>();
            
            string p = AssetDatabase.GetAssetPath(sa);
            for (int i = 0; i < oldSettings.Length; i++)
            {
                if (oldSettings[i].path != p)
                {
                    newSettings.Add(oldSettings[i]);
                }
            }
            newSettings.Add(new EditorBuildSettingsScene(p, true));

            EditorBuildSettings.scenes = newSettings.ToArray();
        }

#endif

        public void Unload()
        {
            
        }
    }
}