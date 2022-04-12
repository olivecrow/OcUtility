using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.ShortcutManagement;
#endif

using UnityEngine;
using UnityEngine.Rendering;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

namespace OcUtility
{
    public class Printer
    {
        static List<Vector3> giz_GLVert_1 = new List<Vector3>();
        static List<Vector3> giz_GLVert_2 = new List<Vector3>();
        static Material giz_Mat;
        static Material primitive_Mat;
        static int _dividerCount;
        
        [RuntimeInitializeOnLoadMethod]
        static void Init()
        {
            var shortCutListener = new GameObject("Shortcut Listener // OcUtility").AddComponent<ShortcutListener>();
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

        [Conditional("UNITY_EDITOR")]
        public static void Ray(Vector3 start, Vector3 dir, bool depthTest = false)
        {
            Debug.DrawRay(start, dir, Color.white, 0, depthTest);
        }
        [Conditional("UNITY_EDITOR")]
        public static void Ray(Vector3 start, Vector3 dir, float duration, bool depthTest)
        {
            Debug.DrawRay(start, dir, Color.white, duration, depthTest);
        }
        [Conditional("UNITY_EDITOR")]
        public static void Ray(Vector3 start, Vector3 dir, Color color, bool depthTest = false)
        {
            Debug.DrawRay(start, dir, color, 0, depthTest);
        }
        [Conditional("UNITY_EDITOR")]
        public static void Ray(Vector3 start, Vector3 dir, Color color, float duration, bool depthTest = false)
        {
            Debug.DrawRay(start, dir, color, duration, depthTest);
        }
        [Conditional("UNITY_EDITOR")]
        public static void Ray(Ray ray, bool depthTest = false)
        {
            Debug.DrawRay(ray.origin, ray.direction, Color.white, 0, depthTest);
        }
        [Conditional("UNITY_EDITOR")]
        public static void Ray(Ray ray, Color color, bool depthTest = false)
        {
            Debug.DrawRay(ray.origin, ray.direction, color, 0, depthTest);
        }
        [Conditional("UNITY_EDITOR")]
        public static void Ray(Ray ray, float duration, bool depthTest = false)
        {
            Debug.DrawRay(ray.origin, ray.direction, Color.white, duration, depthTest);
        }
        [Conditional("UNITY_EDITOR")]
        public static void Ray(Ray ray, Color color, float duration, bool depthTest = false)
        {
            Debug.DrawRay(ray.origin, ray.direction, color, duration, depthTest);
        }
        
        [Conditional("UNITY_EDITOR")]
        public static void Line(Vector3 start, Vector3 end, bool depthTest = false)
        {
            Debug.DrawLine(start, end, Color.white, 0, depthTest);
        }
        [Conditional("UNITY_EDITOR")]
        public static void Line(Vector3 start, Vector3 end, float duration, bool depthTest)
        {
            Debug.DrawLine(start, end, Color.white, duration, depthTest);
        }
        [Conditional("UNITY_EDITOR")]
        public static void Line(Vector3 start, Vector3 end, Color color, bool depthTest = false)
        {
            Debug.DrawLine(start, end, color, 0, depthTest);
        }
        [Conditional("UNITY_EDITOR")]
        public static void Line(Vector3 start, Vector3 end, Color color, float duration, bool depthTest = false)
        {
            Debug.DrawLine(start, end, color, duration, depthTest);
        }

        [Conditional("UNITY_EDITOR")]
        public static void DrawDonut(Vector3 center, Vector3 normal, Vector3 from, 
            float angle, float minRadius, float maxRadius, Color color)
        {
#if UNITY_EDITOR
            CreateGizmoMaterial();
            giz_Mat.SetPass(0);
            giz_GLVert_1.Clear();
            giz_GLVert_2.Clear();
            
            
            var resolution = Mathf.CeilToInt(angle / 9);

            for (int i = 0; i < resolution + 1; i++)
            {
                var a = angle * i / resolution;
                var p = Quaternion.AngleAxis(a, normal) * Quaternion.LookRotation(from, normal) * new Vector3(0, 0, maxRadius);
                giz_GLVert_1.Add(center + p);
        
                var inn = p.normalized * minRadius;
                giz_GLVert_2.Add(center + inn);
            }
            
            GL.PushMatrix();
            GL.MultMatrix(Handles.matrix);
            GL.Begin(GL.TRIANGLES);
            int index = 1;
            for (int length = resolution + 1; index < length; ++index)
            {
                GL.Color(color);
                GL.Vertex(giz_GLVert_2[index]);
                GL.Vertex(giz_GLVert_2[index - 1]);
                GL.Vertex(giz_GLVert_1[index - 1]);
            
            
                GL.Vertex(giz_GLVert_1[index - 1]);
                GL.Vertex(giz_GLVert_1[index]);
                GL.Vertex(giz_GLVert_2[index]);

            }
            GL.End();
            GL.PopMatrix();
#endif
        }

        [Conditional("UNITY_EDITOR")]
        public static void DrawDonut(Vector3 center, Vector3 normal, Vector3 from, 
            float angle, Vector2 range, Color color)
        {
            DrawDonut(center, normal, from, angle, range.x, range.y, color);
        }
        
        [Conditional("UNITY_EDITOR")]
        public static void DrawDonut(Vector3 center, Vector3 from, 
            float angle, Vector2 range, Color color)
        {
            DrawDonut(center, Vector3.up, from, angle, range.x, range.y, color);
        }

        [Conditional("UNITY_EDITOR")]
        public static void DrawDonut(Vector3 center, Vector3 normal, 
            float centerAngle, float angle, float minRadius, float maxRadius, Color color)
        {
            var from = Quaternion.AngleAxis(centerAngle, normal) * Vector3.forward;
            DrawDonut(center, normal, from, angle, minRadius, maxRadius, color);
        }

        [Conditional("UNITY_EDITOR")]
        public static void DrawDonut(Vector3 center, Vector3 normal, 
            float centerAngle, float angle, Vector2 range, Color color)
        {
            var from = Quaternion.AngleAxis(centerAngle, normal) * Vector3.forward;
            DrawDonut(center, normal, from, angle, range, color);
        }
        [Conditional("UNITY_EDITOR")]
        public static void DrawDonut(Vector3 center, float centerAngle, 
            float angle, Vector2 range, Color color)
        {
            var from = Quaternion.AngleAxis(centerAngle, Vector3.up) * Vector3.forward;
            DrawDonut(center, Vector3.up, from, angle, range, color);
        }

        [Conditional("UNITY_EDITOR")]
        public static void DrawCross(Vector3 position, float size, Color color)
        {
#if UNITY_EDITOR
            
            CreateGizmoMaterial();
            giz_Mat.SetPass(0);
            
            GL.PushMatrix();
            GL.MultMatrix(Handles.matrix);
            GL.Begin(GL.LINES);
            GL.Color(color);
            
            GL.Vertex(position - Vector3.right * size);
            GL.Vertex(position + Vector3.right * size);
            GL.Vertex(position - Vector3.up * size);
            GL.Vertex(position + Vector3.up * size);
            GL.Vertex(position - Vector3.forward * size);
            GL.Vertex(position + Vector3.forward * size);
            
            GL.End();
            GL.PopMatrix();
#endif
        }


#if UNITY_EDITOR
        static void CreateGizmoMaterial()
        {
            if (giz_Mat) return;
            giz_Mat = new Material(Shader.Find("Hidden/Internal-Colored"));
            giz_Mat.hideFlags = HideFlags.HideAndDontSave;

            giz_Mat.SetFloat("_ZWrite", 0);
        }
        
        static void CreatePrimitiveMaterial()
        {
            if (primitive_Mat) return;
            primitive_Mat = Resources.Load<Material>("e_mat_transparent");
        }  
#endif


#if UNITY_EDITOR
        // 반환형이 void가 아니라서 직접 전처리기로 감싸줌. 성능이 많이 드는 메서드니까
        // 차라리 빌드과정에서 발견되면 제외시키는게 빌드에 포함시키는것보다 나음.
        public static GameObject CreatePrimitive(
            PrimitiveType type, 
            Vector3 pos,
            float uniformScale,
            Color color, 
            float duration)
        {
            CreatePrimitiveMaterial();
            var gao = GameObject.CreatePrimitive(type);
            gao.name = $"Debug {type}";
            gao.transform.position = pos;
            gao.transform.localScale = Vector3.one * uniformScale;
            gao.layer = LayerMask.NameToLayer("TransparentFX");
            var ren = gao.GetComponent<Renderer>();
            ren.sharedMaterial = primitive_Mat;
            ren.material.color = color;
            

            if (duration > 0) wait.time(duration, () => Object.Destroy(gao));
            
            return gao;
        }  
#endif


#if UNITY_EDITOR
        [Shortcut("OcUtility/PrintDivider", KeyCode.LeftBracket, ShortcutModifiers.Alt)]
#endif
        public static void PrintDivider()
        {
            Print($"================={_dividerCount++}================");
        }
#if UNITY_EDITOR
        [Shortcut("OcUtility/PrintColorDivider", KeyCode.RightBracket, ShortcutModifiers.Alt)]
#endif
        public static void PrintColorDivider()
        {
            Print($"================={_dividerCount++}================".DRT(_dividerCount));
        }
#if UNITY_EDITOR
        [Shortcut("OcUtility/ClearLogs", KeyCode.LeftBracket, ShortcutModifiers.Alt | ShortcutModifiers.Action)]
#endif
        public static void ClearLogs()
        {
            var logEntries = Type.GetType("UnityEditor.LogEntries, UnityEditor.dll");
            var clearMethod = logEntries?.GetMethod("Clear", BindingFlags.Static | BindingFlags.Public);
            clearMethod?.Invoke(null, null);
        }

    }
}
