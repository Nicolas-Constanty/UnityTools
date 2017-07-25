using UnityEngine;

// ReSharper disable once CheckNamespace
namespace UnityTools.DesignPatern
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T s_Instance;

        // ReSharper disable once FieldCanBeMadeReadOnly.Local
        // ReSharper disable once StaticMemberInGenericType
        private static object s_LockThis = new object();

        protected virtual void Awake()
        {
            DontDestroyOnLoad(transform.gameObject);
        }

        public static T Instance
        {
            get
            {
                //if (applicationIsQuitting)
                //{
                //    Debug.LogWarning("[Singleton] Instance '" + typeof(T) +
                //        "' already destroyed on application quit." +
                //        " Won't create again - returning null.");
                //    return null;
                //}

                lock (s_LockThis)
                {
                    if (s_Instance == null)
                    {
                        s_Instance = (T)FindObjectOfType(typeof(T));

                        if (FindObjectsOfType(typeof(T)).Length > 1)
                        {
                            Debug.LogError("[Singleton] Something went really wrong " +
                                " - there should never be more than 1 singleton!" +
                                " Reopening the scene might fix it.");
                            return s_Instance;
                        }

                        if (s_Instance == null)
                        {
                            GameObject singleton = new GameObject();
                            s_Instance = singleton.AddComponent<T>();
                            singleton.name = "(singleton) " + typeof(T);

                            DontDestroyOnLoad(singleton);

                            Debug.Log("[Singleton] An instance of " + typeof(T) +
                                " is needed in the scene, so '" + singleton +
                                "' was created with DontDestroyOnLoad.");
                        }
                        else
                        {
                            Debug.Log("[Singleton] Using instance already created: " +
                                      s_Instance.gameObject.name);
                        }
                    }
                    return s_Instance;
                }
            }
        }

        //private static bool applicationIsQuitting = false;

        public void OnDestroy()
        {
            //applicationIsQuitting = true;
        }
    }
}