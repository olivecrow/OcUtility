using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace OcUtility.Editor
{
    public class OcPoolDebugWindow : EditorWindow
    {
        Dictionary<string, List<IPool>> byScene;
        SerializedObject so;
        int _totalPoolCount;
        bool _initialized;
        [MenuItem("Utility/Oc Pool/디버그 윈도우")]
        private static void ShowWindow()
        {
            var window = GetWindow<OcPoolDebugWindow>();
            window.titleContent = new GUIContent("OcPool 디버그");
            window.Show();
        }

        void OnPlayModeChanged(PlayModeStateChange change)
        {
            switch (change)
            {
                case PlayModeStateChange.EnteredEditMode:
                    break;
                case PlayModeStateChange.ExitingEditMode:
                    break;
                case PlayModeStateChange.EnteredPlayMode:
                    break;
                case PlayModeStateChange.ExitingPlayMode:
                    _initialized = false;
                    byScene = null;
                    EditorApplication.playModeStateChanged -= OnPlayModeChanged;
                    break;
            }
        }

        void OnEnable()
        {
            so = new SerializedObject(this);
            if(!_initialized && PoolManager.Initialized) Init();
            
            EditorApplication.playModeStateChanged += OnPlayModeChanged;
        }

        void OnDisable()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeChanged;
        }

        void Init()
        {
            _totalPoolCount = 0;
            byScene = new Dictionary<string, List<IPool>>();
            foreach (var kv in PoolManager._GlobalPool.
                OrderBy(x => x.Value.GetPoolMemberType().Name))
            {
                var sceneName = kv.Value.Folder == null ? 
                    "Generic" : kv.Value.Folder.gameObject.scene.name;
                if (byScene.ContainsKey(sceneName))
                {
                    byScene[sceneName].Add(kv.Value);
                }
                else
                {
                    byScene[sceneName] = new List<IPool>();
                    byScene[sceneName].Add(kv.Value);
                }

                _totalPoolCount++;
            }

            _initialized = true;
        }

        private void OnGUI()
        {
            if (!Application.isPlaying)
            {
                GUILayout.Label($"플레이모드가 아니면 디버그 할 수 없음.");
                return;
            }
            if(GUILayout.Button("새로고침")) Init();
            if(!_initialized) return;
            if(byScene == null || _totalPoolCount != PoolManager._GlobalPool.Count) Init();
            so.Update();
            var labelStyle = new GUIStyle(GUI.skin.label);
            labelStyle.fontStyle = FontStyle.Bold;
            labelStyle.normal.textColor = Color.white;
            var boxStyle = new GUIStyle(GUI.skin.box);
            boxStyle.onFocused.textColor = Color.cyan;
            foreach (var kv in byScene)
            {
                GUILayout.Label($"Scene : {kv.Key} ---- 풀 개수 : {kv.Value.Count}", labelStyle);
                GUI.backgroundColor = Color.black.SetA(0.2f);
                EditorGUILayout.BeginHorizontal();
                GUILayout.Box("Type", GUILayout.MaxWidth(100));
                GUILayout.Box("Pool", GUILayout.ExpandWidth(true));
                GUILayout.Box("활성화", GUILayout.MaxWidth(50));
                GUILayout.Box("총 개수", GUILayout.MaxWidth(50));
                EditorGUILayout.EndHorizontal();
                foreach (var pool in kv.Value)
                {
                    var typeName = pool.GetPoolMemberType().Name;
                    
                    GUI.backgroundColor = ColorExtension.Random(Animator.StringToHash(typeName)).Brighten(0.5f);
                    EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button(typeName, boxStyle, GUILayout.MaxWidth(100)))
                        if(pool.Source is Object o)EditorGUIUtility.PingObject(o);
                    GUI.backgroundColor = Color.clear;
                    
                    if(GUILayout.Button(pool.SourceName, boxStyle, GUILayout.ExpandWidth(true)))
                        if(pool.Folder != null) EditorGUIUtility.PingObject(pool.Folder);
                    
                    GUILayout.Label($"{pool.ActiveCount:N0}", GUILayout.MaxWidth(50));
                    GUILayout.Label($"{pool.TotalCount:N0}", GUILayout.MaxWidth(50));
                    EditorGUILayout.EndHorizontal();   
                }
            }
            GUI.backgroundColor = Color.white;
            so.ApplyModifiedProperties();
        }
        
    }
}