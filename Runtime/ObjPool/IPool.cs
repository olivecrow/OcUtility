using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace OcUtility
{
    public interface IPool
    {
        int TotalCount { get; }
        int ActiveCount { get; }
        int SleepCount { get; }
        object Source { get; }
        string SourceName { get; }
        Transform Folder { get; }
        Type GetPoolMemberType();
        IEnumerable<object> GetAllMembers();
        IEnumerable<object> GetActiveMembers();
        IEnumerable<object> GetSleepMembers();
        void Dispose();
    }
}