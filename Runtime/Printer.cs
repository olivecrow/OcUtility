using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace OcUtility
{
    public class Printer
    {
        static List<Vector3> giz_DonutOutVert = new List<Vector3>();
        static List<Vector3> giz_DonutInVert = new List<Vector3>();
        static Material giz_Mat;

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

        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")][Obsolete]
        public static void WorldGUI(
            string text, 
            in Vector3 worldPos, 
            in int size = 15, 
            TextAnchor align = TextAnchor.MiddleCenter,
            bool drawOnGizmos = false,
            bool dynamicSizing = true)
        {
            if(GUIDrawer.Instance == null) return;
            var style = new GUIStyle()
            {
                normal = { textColor = Color.white },
                richText = true,
                fontSize = size,
                alignment = TextAnchor.MiddleCenter
            };
            
            var gui = new WorldGUI(text, worldPos, style)
            {
                drawOnGizmos = drawOnGizmos,
            };
            GUIDrawer.Instance.guis.Enqueue(gui);
        }
        
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void Label(
            in Vector3 worldPos, 
            string text,
            bool drawOnGizmos = false,
            GUIStyle style = null)
        {
            if(GUIDrawer.Instance == null) return;

            if (style == null)
                style = new GUIStyle()
                {
                    normal = { textColor = Color.white },
                    richText = true,
                    fontSize = 15,
                    alignment = TextAnchor.MiddleCenter
                };
            
            EnQueueLabel(text, worldPos, drawOnGizmos, style);
        }
        
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void Label(
            in Vector3 worldPos, 
            string text,
            GUIStyle style = null)
        {
            if(GUIDrawer.Instance == null) return;

            if (style == null)
                style = new GUIStyle()
                {
                    normal = { textColor = Color.white },
                    richText = true,
                    fontSize = 15,
                    alignment = TextAnchor.MiddleCenter
                };
            
            EnQueueLabel(text, worldPos, false, style);
        }
        
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void Label(
            in Vector3 worldPos, 
            string text,
            int size)
        {
            if(GUIDrawer.Instance == null) return;

            var style = new GUIStyle()
                {
                    normal = { textColor = Color.white },
                    richText = true,
                    fontSize = size,
                    alignment = TextAnchor.MiddleCenter
                };
            
            EnQueueLabel(text, worldPos, false, style);
        }
        
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void Label(
            in Vector3 worldPos, 
            string text,
            Color color)
        {
            if(GUIDrawer.Instance == null) return;

            var style = new GUIStyle()
            {
                normal = { textColor = color },
                richText = true,
                fontSize = 15,
                alignment = TextAnchor.MiddleCenter
            };
            
            EnQueueLabel(text, worldPos, false, style);
        }
        
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void Label(
            in Vector3 worldPos, 
            string text,
            int size,
            Color color)
        {
            if(GUIDrawer.Instance == null) return;

            var style = new GUIStyle()
            {
                normal = { textColor = color },
                richText = true,
                fontSize = size,
                alignment = TextAnchor.MiddleCenter
            };
            
            EnQueueLabel(text, worldPos, false, style);
        }

        static void EnQueueLabel(string text, in Vector3 worldPos, bool drawOnGizmos, GUIStyle style)
        {
            var gui = new WorldGUI(text, worldPos, style)
            {
                drawOnGizmos = drawOnGizmos
            };
            GUIDrawer.Instance.guis.Enqueue(gui);
        }
        
        
        
        [Conditional("UNITY_EDITOR")]
        public static void DrawDonut(Vector3 center, Vector3 normal, Vector3 from, float angle, float minRadius, float maxRadius, Color color)
        {
            CreateGizmoMaterial();
            giz_Mat.SetPass(0);
            giz_DonutOutVert.Clear();
            giz_DonutInVert.Clear();
            
            
            var resolution = Mathf.CeilToInt(angle / 9);

            for (int i = 0; i < resolution + 1; i++)
            {
                var a = angle * i / resolution;
                var p = Quaternion.AngleAxis(a, normal) * Quaternion.LookRotation(from, normal) * new Vector3(0, 0, maxRadius);
                giz_DonutOutVert.Add(center + p);
        
                var inn = p.normalized * minRadius;
                giz_DonutInVert.Add(center + inn);
            }

            GL.PushMatrix();
            GL.MultMatrix(Handles.matrix);
            GL.Begin(4);
            int index = 1;
            for (int length = resolution + 1; index < length; ++index)
            {
                GL.Color(color);
                GL.Vertex(giz_DonutInVert[index]);
                GL.Vertex(giz_DonutInVert[index - 1]);
                GL.Vertex(giz_DonutOutVert[index - 1]);
            
            
                GL.Vertex(giz_DonutOutVert[index - 1]);
                GL.Vertex(giz_DonutOutVert[index]);
                GL.Vertex(giz_DonutInVert[index]);

            }
            GL.End();
            GL.PopMatrix();
        }

        static void CreateGizmoMaterial()
        {
            if (giz_Mat) return;
            giz_Mat = new Material(Shader.Find("Hidden/Internal-Colored"));
            giz_Mat.hideFlags = HideFlags.HideAndDontSave;

            giz_Mat.SetInt("_ZWrite", 0);
        }

    }
}
