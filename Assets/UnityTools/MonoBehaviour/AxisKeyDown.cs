using UnityEngine;
using System.Collections.Generic;
using System;
using UnityTools.Collections;

// ReSharper disable once CheckNamespace
namespace UnityTools
{
    public class AxisKeyDown : MonoBehaviour
    {

        public DictionaryStringBool Axes = new DictionaryStringBool()
        {
            { "Fire1", false },
            { "Fire2", false },
            { "Fire3", false },
            { "Jump", false },
            { "Submit", false },
            { "Cancel", false }
        };
        public Dictionary<string, Action> Callbacks = new Dictionary<string, Action>();
        private List<string> m_Keys;
        
        // ReSharper disable once UnusedMember.Local
        private void Start()
        {
            m_Keys = new List<string>(Axes.Keys);
        }
        
        // ReSharper disable once UnusedMember.Local
        private void Update()
        {
            foreach (string axis in m_Keys)
            {
                float input = Input.GetAxisRaw(axis);
                if (Math.Abs(input) > 0)
                {
                    if (Axes[axis]) continue;
                    if (Callbacks.ContainsKey(axis))
                        Callbacks[axis]();
                    Axes[axis] = true;
                }
                else
                {
                    Axes[axis] = false;
                }
            }
        }
    }
}
