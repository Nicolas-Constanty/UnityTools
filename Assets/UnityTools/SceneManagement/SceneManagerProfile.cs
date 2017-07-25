using UnityEngine;
using UnityTools.SceneManagement.Model;

// ReSharper disable once CheckNamespace
namespace UnityTools.SceneManagement
{
    [CreateAssetMenu(fileName = "SceneLoaderProfile", menuName = "SceneManagement/Scene Loader")]
    public class SceneManagerProfile : ScriptableObject {

        public SceneStatesModel SceneStatesModel = new SceneStatesModel();

        public LevelSceneModel LevelSceneModel = new LevelSceneModel();
    }
}
