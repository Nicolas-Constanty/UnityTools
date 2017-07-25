using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

// ReSharper disable once CheckNamespace
namespace UnityTools.SceneManagement
{
    [CreateAssetMenu(fileName = "SceneCollection", menuName = "SceneManagement/Scene Collection")]
    public class SceneCollection : ScriptableObject
    {
        public bool Expand;
        public struct SceneAssetInfo
        {
            public SceneAsset SceneAsset;
            public readonly string Path;

            public SceneAssetInfo(string p)
            {
                Path = p;
                SceneAsset = null;
            }
        }

        public string CollectionName = "Collection Name";
        public List<SceneAsset> SceneReferences = new List<SceneAsset>();
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

            public SceneAsset Scene;
        }

        public void Load()
        {
            for (int i = 0; i < SceneReferences.Count; i++)
            {
                SceneAsset sa = SceneReferences[i];
                EditorSceneManager.OpenScene(AssetDatabase.GetAssetPath(sa), i == 0 ? OpenSceneMode.Single : OpenSceneMode.Additive);
                AddToBuild(sa);
            }
        }

        public void LoadAdditive()
        {
            foreach (var scene in SceneReferences)
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

        public void Unload()
        {
            
        }
    }
}