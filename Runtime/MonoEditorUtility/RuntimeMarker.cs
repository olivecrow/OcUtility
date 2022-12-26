using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using Object = UnityEngine.Object;

namespace OcUtility
{
    public class RuntimeMarker : MonoBehaviour
    {
#if UNITY_EDITOR
        [InlineButton("CreateAsset", ShowIf = "@asset == null")]
        [InlineEditor(InlineEditorObjectFieldModes.Foldout)]
        public MarkerSO asset;
        public List<Marker> markers => asset == null ? null : asset.markers;
        [Range(0, 5)]public float gizmoSize = 1;
        void OnValidate()
        {
            if(markers == null) return;
            for (int i = 0; i < markers.Count; i++)
            {
                markers[i].marker = this;
            }
        }

        void CreateAsset()
        {
            if (!AssetDatabase.IsValidFolder("Assets/Editor Default Resources"))
            {
                AssetDatabase.CreateFolder("Assets", "Editor Default Resources");
            }else
            {
                asset = AssetDatabase.LoadAssetAtPath<MarkerSO>("Assets/Editor Default Resources/Runtime Markers");
            }

            if (asset == null)
            {
                asset = ScriptableObject.CreateInstance<MarkerSO>();
                asset.name = "Runtime Markers";
                
                EditorUtility.SetDirty(this);
                EditorUtility.SetDirty(asset);
                AssetDatabase.CreateAsset(asset, $"Assets/Editor Default Resources/{asset.name}.asset");
                AssetDatabase.SaveAssets();
            }
        }

        void OnDrawGizmosSelected()
        {
            if(markers == null) return;
            Gizmos.color = Color.white.SetA(0.1f);
            for (int i = 0; i < markers.Count; i++)
            {
                var m = markers[i];
                Gizmos.DrawSphere(m.worldPosition, m.radius);
                m.marker = this;
            }
        }

        void OnDrawGizmos()
        {
            if(markers == null) return;

            var sceneView = SceneView.lastActiveSceneView;
            var camPos = sceneView.camera.transform.position;
            for (int i = 0; i < markers.Count; i++)
            {
                var m = markers[i];
                var size = (m.worldPosition - camPos).magnitude * 0.01f * gizmoSize;
                Gizmos.color = m.color;
                Gizmos.DrawSphere(m.worldPosition, size);
                Gizmos.DrawWireSphere(m.worldPosition, m.radius);
                
                
                if (!string.IsNullOrWhiteSpace(m.title))
                {
                    Handles.color = m.color;
                    var guiStyle = new GUIStyle
                    {
                        fontSize = 14,
                        normal =
                        {
                            textColor = m.color
                        },
                        alignment = TextAnchor.LowerCenter
                    };
                    Handles.Label(m.worldPosition + Vector3.up * size, m.title, guiStyle);
                }
            }
        }

        [Serializable]
        public class Marker
        {
            [HideLabel]public Color color = ColorExtension.SystemRandom();
            [LabelWidth(50)]public string title = "새 마커";
            [HideInInspector]public RuntimeMarker marker;
            [InlineButton("StartPinMarker", "Pin")]
            [InlineButton("Focus")]
            public Vector3 worldPosition;
            public float radius;

            [TextArea]public string comment;

            void StartPinMarker()
            {
                SceneView.duringSceneGui += SceneGUI;
            }

            void EndPinMarker()
            {
                SceneView.duringSceneGui -= SceneGUI;
            }

            void Focus()
            {
                SceneView.lastActiveSceneView.LookAt(worldPosition);
            }

            void SceneGUI(SceneView sceneView)
            {
                var e = Event.current;
                if (e.isMouse)
                {
                    var ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
                    if (Physics.Raycast(ray, out var hit))
                    {
                        worldPosition = hit.point;
                        EditorUtility.SetDirty(marker);
                    }
                }
                
                if (e.type == EventType.MouseDown && e.button == 0)
                {
                    EndPinMarker();
                }
                else
                {
                    HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
                }
            }
        }
#endif
    }
}