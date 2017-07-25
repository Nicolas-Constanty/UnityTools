using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityTools.Common;

// ReSharper disable once CheckNamespace
namespace UnityTools.SceneManagement
{
    public class SceneManagementWindow : EditorWindow
    {

        // Use this for initialization
        [MenuItem("Window/Scene Management")]
        public static void ShowWindow()
        {

           var w = GetWindow<SceneManagementWindow>("Manager");
            GUIContent c = new GUIContent()
            {
                image = Resources.Load("UnityTools", typeof(Texture2D)) as Texture,
                text = "Manager"
            };
            w.titleContent = c;
        }

        // Update is called once per frame
        // ReSharper disable once UnusedMember.Local
        // ReSharper disable once InconsistentNaming
        private void OnGUI()
        {
            //Texture2D t = EditorGUIUtility.Load("Icons/UnityEditor.DebugInspectorWindow.png") as Texture2D;
            GUILayout.Label("UnityTools @SceneManagement");
            if (GUILayout.Button("Create SceneManagerProfile"))
            {
                Utils.CreateAndRenameAsset<SceneManagerProfile>();
            }

            if (GUILayout.Button("Create SceneCollection"))
            {
                SceneCollection sc = Utils.CreateAndRenameAsset<SceneCollection>();
                for (int i = 0; i < EditorSceneManager.loadedSceneCount; i++)
                {
                    string path = EditorSceneManager.GetSceneAt(i).path;
                    sc.SceneReferences.Add(AssetDatabase.LoadAssetAtPath<SceneAsset>(path));
                }
            }
        }
    }
}
