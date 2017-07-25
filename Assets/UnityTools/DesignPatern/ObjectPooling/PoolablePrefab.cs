using UnityEngine;

// ReSharper disable once CheckNamespace
namespace UnityTools.DesignPatern
{
    public class PoolablePrefab : APoolable
    {
        
        public GameObject PrefabInstance { get; protected set; }

        public PoolablePrefab(IObjectPool<APoolable> pool, GameObject prefab, float lifetime) : base(pool)
        {
            Lifetime = lifetime;
            PrefabInstance = prefab;
            PrefabInstance.SetActive(false);
        }

        public override void Use()
        {
            IsObjectAvailable = false;
            PrefabInstance.SetActive(true);
        }

        public override void Use(Vector3 pos, Space space = Space.World)
        {
            Use();
            if (Space.World == space)
                PrefabInstance.transform.position = pos;
            else
                PrefabInstance.transform.localPosition = pos;
        }

        public override void Use(Vector3 pos, Quaternion rot)
        {
            Use(pos);
            PrefabInstance.transform.rotation = rot;
        }

        public override void Use(Vector3 pos, Quaternion rot, Vector3 scale)
        {
            Use(pos, rot);
            PrefabInstance.transform.localScale = scale;
        }

        public override void SetFree()
        {
            IsObjectAvailable = true;
            PrefabInstance.SetActive(false);
        }
    }
}