using System;
using System.Collections.Generic;
using System.Reflection;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace OcUtility
{
    public class WorldGUI
    {
        public string text;
        public int size;
        public Vector3 worldPos;
        public Color color = Color.white;
        public TextAnchor Alignment = TextAnchor.MiddleCenter;
        public bool drawOnGizmos;
        public bool dynamicSizing = true;
        public WorldGUI(string text, Vector3 worldPos, int size = 12)
        {
            this.text = text;
            this.size = size;
            this.worldPos = worldPos;
        }
    }
    public class GUIDrawer : MonoBehaviour
    {
        public static GUIDrawer Instance => _instance;
        static GUIDrawer _instance;
        public Queue<WorldGUI> guis = new Queue<WorldGUI>();
        List<WorldGUI> internalGuis = new List<WorldGUI>();

        void Awake()
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            _instance = this;
            DontDestroyOnLoad(gameObject);
#else
            Destroy(gameObject);
#endif
        }

        void Update()
        {
            var count = guis.Count;
            if(count == 0) return;
            internalGuis.Clear();
            for (int i = 0; i < count; i++)
            {
                internalGuis.Add(guis.Dequeue());
            }
        }

        void OnDestroy()
        {
            if (_instance == this) _instance = null;
        }

        bool AreGizmosVisible()
        {
            Assembly asm = Assembly.GetAssembly(typeof(UnityEditor.Editor));
            Type type = asm.GetType("UnityEditor.GameView");
            if (type != null)
            {
                EditorWindow window = EditorWindow.GetWindow(type);
                FieldInfo gizmosField = type.GetField("m_Gizmos", BindingFlags.NonPublic | BindingFlags.Instance);
                if(gizmosField != null)
                    return (bool)gizmosField.GetValue(window);
            }
            return false;
        }
        
        void OnDrawGizmos()
        {
            if(AreGizmosVisible()) return;
            var count = internalGuis.Count;
            for (int i = 0; i < count; i++)
            {
                var gui = internalGuis[i];
                if(!gui.drawOnGizmos) return;
                
                var wtsPos = SceneView.lastActiveSceneView.camera.WorldToScreenPoint(gui.worldPos);
                
                if(wtsPos.x < 0 || wtsPos.x > Screen.width || 
                   wtsPos.y < 0 || wtsPos.y > Screen.height ||
                   wtsPos.z < 0) continue;
                
                var fontSize = gui.dynamicSizing ? (int)Mathf.Lerp(gui.size, 5, wtsPos.z / 50) : gui.size;
                if(fontSize < 8) return;

                var style = new GUIStyle()
                {
                    fontSize = fontSize,
                    richText = true,
                    normal =
                    {
                        textColor = gui.color
                    },
                    alignment = gui.Alignment
                };
                Handles.Label(gui.worldPos + new Vector3(0, 0.2f), gui.text, style);
            }
        }  
        void OnGUI()
        {
            if(Camera.main == null) return;
            var count = internalGuis.Count;
            for (int i = 0; i < count; i++)
            {
                var gui = internalGuis[i];
                var wtsPos = Camera.main.WorldToScreenPoint(gui.worldPos);
                if(wtsPos.x < 0 || wtsPos.x > Screen.width || 
                   wtsPos.y < 0 || wtsPos.y > Screen.height ||
                   wtsPos.z < 0) continue;
                var rect = new Rect(new Vector2(wtsPos.x, Screen.height-wtsPos.y), new Vector2(100, 20));

                var fontSize = gui.dynamicSizing ? (int)Mathf.Lerp(gui.size, 5, wtsPos.z / 50) : gui.size;
                if(fontSize < 8) return;
                
                var style = new GUIStyle()
                {
                    fontSize = fontSize,
                    richText = true,
                    normal =
                    {
                        textColor = gui.color
                    },
                    alignment = gui.Alignment
                };
                GUI.Label(rect, gui.text, style);
            }
        }
    }
}