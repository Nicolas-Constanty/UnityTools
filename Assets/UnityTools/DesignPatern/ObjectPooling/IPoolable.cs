using System;

// ReSharper disable once CheckNamespace
namespace UnityTools.DesignPatern
{ 
    public interface IPoolable
    {
        event Action OnObjectLifetimeOver;
        void Use();
        void SetFree();
    }
}