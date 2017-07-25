using System;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace UnityTools.DesignPatern
{
    public abstract class APoolable : IPoolable
    {
        public float Lifetime { get; protected set; }

        private bool m_IsObjectAvailable = true;
        private readonly IObjectPool<APoolable> m_ObjectPoolReference;

        public event Action OnObjectLifetimeOver;

        public bool IsObjectAvailable
        {
            get { return m_IsObjectAvailable; }

            protected set { m_IsObjectAvailable = value; }
        }

        protected APoolable(IObjectPool<APoolable> objectPool)
        {
            m_ObjectPoolReference = objectPool;
            OnObjectLifetimeOver += APoolable_OnObjectLifetimeOver;
        }

        protected void APoolable_OnObjectLifetimeOver()
        {
            IsObjectAvailable = true;
        }

        public abstract void SetFree();

        public abstract void Use();

        public abstract void Use(Vector3 pos, Space space = Space.World);

        public abstract void Use(Vector3 pos, Quaternion rot);

        public abstract void Use(Vector3 pos, Quaternion rot, Vector3 scale);

        public IObjectPool<APoolable> GetObjectPoolRef()
        {
            return m_ObjectPoolReference;
        }
    }
}
