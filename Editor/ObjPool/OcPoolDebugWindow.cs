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
        Dictionary<IPool, bool> _foldout;
        Dictionary<IPool, Vector2> _foldoutScrollPosition;
        SerializedObject so;
        int _totalPoolCount;
        bool _initialized;
        Vector2 _scrollViewPosition;
        [MenuItem("Utility/Oc Pool 디버그 윈도우")]
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
            if (_foldout == null) _foldout = new Dictionary<IPool, bool>();
            if (_foldoutScrollPosition == null) _foldoutScrollPosition = new Dictionary<IPool, Vector2>();
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
            boxStyle.richText = true;
            _scrollViewPosition = EditorGUILayout.BeginScrollView(_scrollViewPosition, false, true);
            foreach (var kv in byScene)
            {
                GUILayout.Label($"Scene : {kv.Key} ---- 풀 개수 : {kv.Value.Count}", labelStyle);
                GUI.backgroundColor = Color.black.SetA(0.5f);
                EditorGUILayout.BeginHorizontal();
                GUILayout.Box("Type", GUILayout.MaxWidth(Style.TypeWidth));
                GUILayout.Box("Pool", GUILayout.ExpandWidth(true));
                GUILayout.Box("활성화", GUILayout.MaxWidth(Style.ActiveCountWidth));
                GUILayout.Box("총 개수", GUILayout.MaxWidth(Style.TotalCountWidth));
                EditorGUILayout.EndHorizontal();
                foreach (var pool in kv.Value)
                {
                    var typeName = pool.GetPoolMemberType().Name;
                    
                    GUI.backgroundColor = ColorExtension.Random(Animator.StringToHash(typeName)) * 2;
                    EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button(typeName, boxStyle, GUILayout.MaxWidth(Style.TypeWidth)))
                        if(pool.Source is Object o)EditorGUIUtility.PingObject(o);

                    var poolName = pool.Folder == null ? $"{pool.SourceName} | {("Folder is NULL").Rich(Color.red)}" : pool.SourceName;
                    
                    if(!_foldout.ContainsKey(pool)) _foldout.Add(pool, false);
                    GUI.backgroundColor = Color.white;
                    var st = new GUIStyle(GUI.skin.label);
                    st.fixedWidth = 50;
                    st.normal.textColor = Color.white;
                    _foldout[pool] = EditorGUILayout.Foldout(_foldout[pool], "", true, st);
                    if(GUILayout.Button(poolName, boxStyle, GUILayout.ExpandWidth(true)))
                        if (pool.Folder != null) EditorGUIUtility.PingObject(pool.Folder);

                    GUILayout.Label($"{pool.ActiveCount:N0}", GUILayout.MaxWidth(Style.ActiveCountWidth));
                    GUILayout.Label($"{pool.TotalCount:N0}", GUILayout.MaxWidth(Style.TotalCountWidth));
                    EditorGUILayout.EndHorizontal();
                    
                    GUI.backgroundColor = Color.white;
                    if (_foldout[pool])
                    {
                        EditorGUILayout.BeginHorizontal();
                        if(!_foldoutScrollPosition.ContainsKey(pool)) _foldoutScrollPosition.Add(pool, Vector2.zero);
                        _foldoutScrollPosition[pool] = 
                            EditorGUILayout.BeginScrollView(
                                _foldoutScrollPosition[pool], true, true, 
                                GUILayout.ExpandHeight(false), GUILayout.MaxHeight(300), GUILayout.MinWidth(400));
                        var memberIsComponent = pool.Source is MonoBehaviour;
                        foreach (var member in pool.GetAllMembers())
                        {
                            EditorGUILayout.BeginHorizontal();
                            if(memberIsComponent)
                            {
                                if(member == null)
                                {
                                    null_member();
                                    continue;
                                }
                                var m = member as MonoBehaviour;
                                if (m == null)
                                {
                                    null_member();
                                    continue;
                                }
                                GUI.contentColor = m.isActiveAndEnabled ? Color.white : Color.white.SetA(0.5f);
                                if (GUILayout.Button(m.name, GUI.skin.label))
                                    EditorGUIUtility.PingObject(m);

                                void null_member()
                                {
                                    GUI.color = Color.red;
                                    GUILayout.Label("NULL");
                                    GUI.color = Color.white;
                                }
                            }
                            else
                            {
                                GUI.contentColor = pool.GetActiveMembers().Contains(member) ? Color.white : Color.white.SetA(0.5f);
                                GUILayout.Label(member.ToString());
                            }

                            EditorGUILayout.EndHorizontal();
                        }
                        EditorGUILayout.EndScrollView();
                        EditorGUILayout.EndHorizontal();
                    }
                }
            }
            EditorGUILayout.EndScrollView();
            GUI.backgroundColor = Color.white;
            so.ApplyModifiedProperties();
        }
        
    }

    internal static class Style
    {
        public const int TypeWidth = 150;
        public const int ActiveCountWidth = 50;
        public const int TotalCountWidth = 50;
        public static float NameWidth(float windowWidth) => windowWidth - TypeWidth - ActiveCountWidth - TotalCountWidth;
    }
}