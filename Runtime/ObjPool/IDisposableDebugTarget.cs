using System;

namespace OcUtility
{
    public interface IDisposableDebugTarget
    {
        Type GetGenericType();
        void DebugType();
        void DebugLog();
        void Dispose();
    }
}