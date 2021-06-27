using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace OcUtility
{
    public class PoolMember : MonoBehaviour
    {
        public UnityEvent OnWakeUp;
        public UnityEvent OnSleep;
        public OcPool Pool { get; set; }
        public void WakeUp()
        {
            gameObject.SetActive(true);
            OnWakeUp?.Invoke();
        }

        public void Sleep()
        {
            OnSleep?.Invoke();
            gameObject.SetActive(false);
            Pool.Return(this);
        }
    }
}
