using System;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace OcUtility
{
    public class Printer
    {
        [RuntimeInitializeOnLoadMethod]
        static void Init()
        {
            var guiDrawer = new GameObject("GUI Drawer // instantiated by OcUtility.Printer").AddComponent<GUIDrawer>();
        }
        
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

        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void Ray(Vector3 start, Vector3 dir, bool depthTest = false)
        {
            Debug.DrawRay(start, dir, Color.white, 0, depthTest);
        }
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void Ray(Vector3 start, Vector3 dir, float duration, bool depthTest)
        {
            Debug.DrawRay(start, dir, Color.white, duration, depthTest);
        }
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void Ray(Vector3 start, Vector3 dir, Color color, bool depthTest = false)
        {
            Debug.DrawRay(start, dir, color, 0, depthTest);
        }
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void Ray(Vector3 start, Vector3 dir, Color color, float duration, bool depthTest = false)
        {
            Debug.DrawRay(start, dir, color, duration, depthTest);
        }
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void Ray(Ray ray, bool depthTest = false)
        {
            Debug.DrawRay(ray.origin, ray.direction, Color.white, 0, depthTest);
        }
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void Ray(Ray ray, Color color, bool depthTest = false)
        {
            Debug.DrawRay(ray.origin, ray.direction, color, 0, depthTest);
        }
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void Ray(Ray ray, float duration, bool depthTest = false)
        {
            Debug.DrawRay(ray.origin, ray.direction, Color.white, duration, depthTest);
        }
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void Ray(Ray ray, Color color, float duration, bool depthTest = false)
        {
            Debug.DrawRay(ray.origin, ray.direction, color, duration, depthTest);
        }
        
        
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void Line(Vector3 start, Vector3 end, bool depthTest = false)
        {
            Debug.DrawLine(start, end, Color.white, 0, depthTest);
        }
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void Line(Vector3 start, Vector3 end, float duration, bool depthTest)
        {
            Debug.DrawLine(start, end, Color.white, duration, depthTest);
        }
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void Line(Vector3 start, Vector3 end, Color color, bool depthTest = false)
        {
            Debug.DrawLine(start, end, color, 0, depthTest);
        }
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void Line(Vector3 start, Vector3 end, Color color, float duration, bool depthTest = false)
        {
            Debug.DrawLine(start, end, color, duration, depthTest);
        }

        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void WorldGUI(
            string text, 
            in Vector3 worldPos, 
            in int size = 15, 
            TextAnchor align = TextAnchor.MiddleCenter,
            bool drawOnGizmos = false,
            bool dynamicSizing = true)
        {
            if(GUIDrawer.Instance == null) return;
            var gui = new WorldGUI(text, worldPos, size)
            {
                Alignment = align,
                drawOnGizmos = drawOnGizmos,
                dynamicSizing = dynamicSizing
            };
            GUIDrawer.Instance.guis.Enqueue(gui);
        }
    }
}
