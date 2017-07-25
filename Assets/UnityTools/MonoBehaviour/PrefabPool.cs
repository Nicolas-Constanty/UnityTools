using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityTools.DesignPatern;

// ReSharper disable once CheckNamespace
namespace UnityTools
{
    public class PrefabPool : MonoBehaviour, IObjectPool<APoolable>
    {
        [SerializeField] private bool m_Parent = false;
#pragma warning disable 649
        [SerializeField] private GameObject m_Prefab;
#pragma warning restore 649

        public int PoolSize
        {
            get { return m_PoolSize; }
            protected set { m_PoolSize = value; }
        }

        public float PoolObjectLifeTime
        {
            get { return m_PoolObjectLifeTime; }
            protected set { m_PoolObjectLifeTime = value; }
        }

        private readonly List<PoolablePrefab> m_PrefabList = new List<PoolablePrefab>();
        [SerializeField] private int m_PoolSize = 10;
        [SerializeField] private float m_PoolObjectLifeTime = 0.5f;
        private readonly Queue<PoolablePrefab> m_PoolablePrefabs = new Queue<PoolablePrefab>(); 

        protected void Awake()
        {
            if (m_Prefab == null)
            {
                enabled = false;
                return;
            }
            GameObject go = new GameObject();
            go.name = "PrefabPool";
            if (m_Parent)
                go.transform.SetParent(transform);
            for (int i = 0; i < PoolSize; ++i)
            {
                GameObject inst = Instantiate(m_Prefab);
                inst.transform.SetParent(go.transform);
                m_PrefabList.Add(new PoolablePrefab(this, inst, m_PoolObjectLifeTime));
            }
        }

        public APoolable GetObject(Vector3 pos, Space space = Space.World)
        {
            var obj = m_PrefabList.Find(x => x.IsObjectAvailable);
            if (obj == null) return null;
            m_PoolablePrefabs.Enqueue(obj);
            obj.Use(pos, space);
            Invoke("SetFree", obj.Lifetime);
            return obj;
        }

        public APoolable GetObject(Vector3 pos, Quaternion rot)
        {
            var obj = m_PrefabList.Find(x => x.IsObjectAvailable);
            if (obj == null) return null;
            m_PoolablePrefabs.Enqueue(obj);
            obj.Use(pos, rot);
            Invoke("SetFree", obj.Lifetime);
            return obj;
        }

        public APoolable GetObject(Vector3 pos, Quaternion rot, Vector3 scale)
        {
            var obj = m_PrefabList.Find(x => x.IsObjectAvailable);
            if (obj == null) return null;
            m_PoolablePrefabs.Enqueue(obj);
            obj.Use(pos, rot, scale);
            Invoke("SetFree", obj.Lifetime);
            return obj;
        }

        public APoolable GetObject()
        {
            var obj = m_PrefabList.Find(x => x.IsObjectAvailable);
            if (obj == null) return null;
            m_PoolablePrefabs.Enqueue(obj);
            obj.Use();
            Invoke("SetFree", obj.Lifetime);
            return obj;
        }

        // ReSharper disable once UnusedMember.Local
        private void SetFree()
        {
            if (m_PoolablePrefabs.Any())
            {
                m_PoolablePrefabs.Dequeue().SetFree();
            }
        }
    }

}
