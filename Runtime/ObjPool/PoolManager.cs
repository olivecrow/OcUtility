using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[assembly:InternalsVisibleTo("Unity.OcUtility.Editor")]
namespace OcUtility
{
    
    internal static class PoolManager
    {
        internal static bool Initialized => _initialized;
        internal static Dictionary<int, IPool> _GlobalPool;
        static bool _initialized;
        
        [RuntimeInitializeOnLoadMethod]
        internal static void Init()
        {
            if(_initialized) return;
#if UNITY_EDITOR
            Application.quitting += Release;
#endif
            _GlobalPool = new Dictionary<int, IPool>();
            _initialized = true;
        }
#if UNITY_EDITOR
        static void Release()
        {
            _GlobalPool = null;
            _initialized = false;
            Application.quitting -= Release;
        }
#endif

        internal static bool HasPool(int hash)
        {
            return _GlobalPool.ContainsKey(hash);
        }

        internal static IPool GetPool(int hash)
        {
            return _GlobalPool[hash];
        }
        internal static void RegisterPool(int hash, IPool pool)
        {
            if(_GlobalPool == null) Init();
            _GlobalPool.Add(hash, pool);
        }

        internal static void UnRegisterPool(int hash)
        {
            if(_initialized || _GlobalPool == null) return;
            _GlobalPool.Remove(hash);
        }
        
        internal static void DisposeAll()
        {
            if(_GlobalPool == null) return;
            foreach (var ocPool in _GlobalPool)
            {
                ocPool.Value.Dispose();
            }

            _GlobalPool = null;
            _initialized = false;
        }
    }
}