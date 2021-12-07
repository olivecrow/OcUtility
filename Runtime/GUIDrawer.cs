using System;
using System.Collections.Generic;
using System.Reflection;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace OcUtility
{
    public class WorldGUI
    {
        public string text;
        public Vector3 worldPos;
        public bool drawOnGizmos;
        public GUIStyle style;
        public WorldGUI(string text, Vector3 worldPos, GUIStyle style)
        {
            this.text = text;
            this.style = style;
            this.worldPos = worldPos;
        }
    }
    // [ExecuteInEditMode]
    public class GUIDrawer : MonoBehaviour
    {
        public static GUIDrawer Instance => _instance;
        static GUIDrawer _instance;
        public Queue<WorldGUI> guis = new Queue<WorldGUI>();
        List<WorldGUI> internalGuis = new List<WorldGUI>();
        object _gameView;


#if UNITY_EDITOR || DEVELOPMENT_BUILD
        [InitializeOnLoadMethod]
        [RuntimeInitializeOnLoadMethod]
#endif
        static void Init()
        {
            var exists = Resources.FindObjectsOfTypeAll<GUIDrawer>();
            if(exists.Length > 0)
            {
                print("GUI Drawer Exist");
                return;
            }
            
            var guiDrawer = new GameObject("GUI Drawer (Don'tSave) // OcUtility").AddComponent<GUIDrawer>();
            guiDrawer.gameObject.hideFlags = HideFlags.DontSave;
        }
        
        void Awake()
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.Log("GUIDrawer Instantiated");
            _instance = this;
            if(Application.isPlaying)DontDestroyOnLoad(gameObject);
#else
            Destroy(gameObject);
#endif
        }

        void Update()
        {
            if(!Application.isPlaying) return;
            UpdateInternalQueue();
        }

        void OnDestroy()
        {
            if (_instance == this) _instance = null;
        }

        void UpdateInternalQueue()
        {
            if (_instance == null) _instance = this;
            var count = guis.Count;
            if(count == 0) return;
            internalGuis.Clear();
            for (int i = 0; i < count; i++)
            {
                internalGuis.Add(guis.Dequeue());
            }
        }

        bool ShouldHideSceneViewGizmo()
        {
#if UNITY_EDITOR
            if (EditorWindow.focusedWindow == null) return true;
            
            if(Application.isPlaying)
            {
                var typeName = EditorWindow.focusedWindow.GetType().Name;
                if (typeName == "GameView") return true;
            }

            return false;
#else
            return true;
#endif
        }

        static int CalcFontSize(in Vector3 wtsPos, in int size)
        {
            return (int)Mathf.Lerp(size, 5, wtsPos.z / 50);
        }
        
        void OnDrawGizmos()
        {
            if(ShouldHideSceneViewGizmo()) return;
            if (!Application.isPlaying) UpdateInternalQueue();
            var count = internalGuis.Count;
            for (int i = 0; i < count; i++)
            {
                var gui = internalGuis[i];
                if(!gui.drawOnGizmos) return;
                
                var wtsPos = SceneView.lastActiveSceneView.camera.WorldToScreenPoint(gui.worldPos);
                
                if(wtsPos.x < 0 || wtsPos.x > Screen.width || 
                   wtsPos.y < 0 || wtsPos.y > Screen.height ||
                   wtsPos.z < 0) continue;
                var rect = new Rect(new Vector2(wtsPos.x, Screen.height - wtsPos.y), Vector2.zero);

                var fontSize = CalcFontSize(in wtsPos, gui.style.fontSize);
                if (Application.isPlaying) fontSize += 2;
                if(fontSize < 8) return;

                gui.style.fontSize = fontSize;
                
                Handles.Label(gui.worldPos + new Vector3(0, wtsPos.z * 0.075f), gui.text, gui.style);
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
                var rect = new Rect(new Vector2(wtsPos.x, Screen.height-wtsPos.y), Vector2.zero);
        
                var fontSize = CalcFontSize(in wtsPos, gui.style.fontSize);
                if(fontSize < 8) return;

                gui.style.fontSize = fontSize;
             
                GUI.Label(rect, gui.text, gui.style);
            }
        }
    }
}