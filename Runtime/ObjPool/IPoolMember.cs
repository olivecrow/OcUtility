using UnityEngine;

namespace OcUtility
{
    public interface IPoolMember<T> where T : MonoBehaviour, IPoolMember<T>
    {
        GameObject gameObject { get; }
        Transform transform { get; }
        OcPool<T> Pool { get; set; }
        void WakeUp();
        void Sleep();
    }
}