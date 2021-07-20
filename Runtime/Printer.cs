using System;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace OcUtility
{
    public class Printer
    {
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void Print(object value, LogType type = LogType.Log)
        {
            switch (type)
            {
                case LogType.Error:
                    Debug.LogError(value);
                    break;
                case LogType.Assert:
                    Debug.LogAssertion(value);
                    break;
                case LogType.Warning:
                    Debug.LogWarning(value);
                    break;
                case LogType.Log:
                    Debug.Log(value);
                    break;
                case LogType.Exception:
                    Debug.LogException(value as Exception);
                    break;
            }
        }
    }
}
