
using System;
using UnityEditor;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace UnityTools.Common
{
    public static class Utils
    {
        //private static int FindIndex<T>(this IEnumerable<T> array, Func<bool> func)
        //{

        //}

        public static T ToEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value);
        }

        public static Transform FindDeepChild(this Transform aParent, string aName)
        {
            var result = aParent.Find(aName);
            if (result != null)
                return result;
            foreach (Transform child in aParent)
            {
                result = child.FindDeepChild(aName);
                if (result != null)
                    return result;
            }
            return null;
        }

        public static int GetLoadLevel(LOD[] lods)
        {
            int lodIndex = 0;
            bool find = false;
            foreach (var item in lods)
            {
                foreach (var rend in item.renderers)
                {
                    if (rend.isVisible)
                    {
                        find = true;
                        break;
                    }
                }
                if (find)
                    break;
                ++lodIndex;
            }
            return lodIndex;
        }

        public static Vector3 GetNearPointBoxCollider(Vector3 origin, BoxCollider b)
        {
            Vector3 vec = b.transform.TransformPoint(b.center + new Vector3(0, 0, b.size.z) * 0.5f);
            Vector3 tmp = b.transform.TransformPoint(b.center + new Vector3(0, 0, -b.size.z) * 0.5f);
            if (Vector3.Distance(origin, tmp) < Vector3.Distance(origin, vec))
                vec = tmp;
            tmp = b.transform.TransformPoint(b.center + new Vector3(b.size.x, 0, 0) * 0.5f);
            if (Vector3.Distance(origin, tmp) < Vector3.Distance(origin, vec))
                vec = tmp;
            tmp = b.transform.TransformPoint(b.center + new Vector3(-b.size.x, 0, 0) * 0.5f);
            if (Vector3.Distance(origin, tmp) < Vector3.Distance(origin, vec))
                vec = tmp;
            return vec;
        }

        public static T CreateAndRenameAsset<T>() where T : ScriptableObject
        {
            T asset = ScriptableObject.CreateInstance<T>();

            //Create asset and start renaming it
            ProjectWindowUtil.CreateAsset(asset, "Assets/" + typeof(T).Name + ".asset");
            return asset;
        }

        public static T CreateAndRenameAsset<T>(string filename) where T : ScriptableObject
        {
            T asset = ScriptableObject.CreateInstance<T>();

            //Create asset and start renaming it
            ProjectWindowUtil.CreateAsset(asset, "As1ets/" + filename + ".asset");
            return asset;
        }
    }
}
