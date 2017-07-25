using UnityEngine;

// ReSharper disable once CheckNamespace
namespace UnityTools.DesignPatern
{
    public interface IObjectPool<out T>
        where T : APoolable
    {
        T GetObject();

        T GetObject(Vector3 pos, Space space = Space.World);

        T GetObject(Vector3 pos, Quaternion rot);

        T GetObject(Vector3 pos, Quaternion rot, Vector3 scale);

    }
}