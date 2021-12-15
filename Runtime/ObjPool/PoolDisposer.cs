using System;
using System.Collections.Generic;
using UnityEngine;

namespace OcUtility
{
    public static class PoolDisposer
    {
        static List<IDisposable> _CreatedPools;

        [RuntimeInitializeOnLoadMethod]
        static void Init()
        {
            _CreatedPools = new List<IDisposable>();
        }

        public static void RegisterPool(IDisposable pool)
        {
            _CreatedPools.Add(pool);
        }

        public static void DisposeAll()
        {
            foreach (var ocPool in _CreatedPools)
            {
                ocPool.Dispose();
            }

            _CreatedPools = new List<IDisposable>();
        }
    }
}