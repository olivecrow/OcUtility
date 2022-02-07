using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace OcUtility
{
    public abstract class PoolMember<T> : MonoBehaviour, IPoolMember<T> where T : PoolMember<T>
    {
        public UnityEvent OnWakeUp;
        public UnityEvent OnSleep;

        public OcPool<T> Pool { get; set; }
        public bool IsParentChanged { get; private set; }
        /// <summary> 오브젝트를 활성화하고 OnWakeUp 콜백을 실행함. OnEnable에서 호출하지 말 것. </summary>
        public virtual void WakeUp()
        {
            gameObject.SetActive(true);
            OnWakeUp?.Invoke();
        }

        /// <summary> OnSleep 콜백을 실행하고 오브젝트를 비활성화한 후, 원래의 풀로 되돌아감. OnDisable에서 호출하지 말 것. </summary>
        public virtual void Sleep()
        {
            OnSleep?.Invoke();
            gameObject.SetActive(false);
            if(Pool != null)
            {
                Pool.Return(this as T);
                if (IsParentChanged && Pool.Folder != null)
                {
                    transform.SetParent(Pool.Folder);
                    IsParentChanged = false;
                }
            }
        }

        void OnTransformParentChanged()
        {
            IsParentChanged = true;
        }
    }
}
