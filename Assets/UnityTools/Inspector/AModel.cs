using System;
using UnityEngine;
using UnityTools.Common;

// ReSharper disable once CheckNamespace
namespace UnityTools.Inspector
{
    [Serializable]
    public abstract class AModel
    {
        [SerializeField, GetSet("enabled")]
        bool m_Enabled;
        public bool Enabled
        {
            get { return m_Enabled; }
            set
            {
                m_Enabled = value;

                if (value)
                    OnValidate();
            }
        }

        public abstract void Reset();

        public virtual void OnValidate()
        { }
    }
}