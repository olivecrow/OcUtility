using System;
using System.Collections.Generic;
using System.Reflection;

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace OcUtility
{
    internal static class PoolDisposer
    {
        static List<IDisposableDebugTarget> _CreatedPools;
        static bool _initialized;
        
        [RuntimeInitializeOnLoadMethod]
        static void Init()
        {
#if UNITY_EDITOR
            Application.quitting += () => _initialized = false;
#endif
            if(_initialized) return;
            _CreatedPools = new List<IDisposableDebugTarget>();
            _initialized = true;
        }

        internal static void RegisterPool(IDisposableDebugTarget pool)
        {
            if(_CreatedPools == null) Init();
            _CreatedPools.Add(pool);
        }

        internal static void UnRegisterPool(IDisposableDebugTarget pool)
        {
            _CreatedPools.Remove(pool);
        }
        
        internal static void DisposeAll()
        {
            foreach (var ocPool in _CreatedPools)
            {
                ocPool.Dispose();
            }

            _CreatedPools = new List<IDisposableDebugTarget>();
            _initialized = false;
        }

#if UNITY_EDITOR
        [MenuItem("Utility/OcPool/Debug All Types")]
        internal static void DebugAllTypes()
        {
            if (!Application.isPlaying)
            {
                Printer.Print($"플레이 모드 진입 시에만 사용 가능", LogType.Warning);
                return;
            }
            var calledType = new List<Type>();
            foreach (var ocPool in _CreatedPools)
            {
                if(calledType.Contains(ocPool.GetGenericType())) continue;
                ocPool.DebugType();
                calledType.Add(ocPool.GetGenericType());
            }
        }
        
        [MenuItem("Utility/OcPool/Debug All Pools")]
        internal static void DebugAllPools()
        {
            if (!Application.isPlaying)
            {
                Printer.Print($"플레이 모드 진입 시에만 사용 가능", LogType.Warning);
                return;
            }
            foreach (var ocPool in _CreatedPools)
            {
                ocPool.DebugLog();
            }
        }
#endif
    }
}