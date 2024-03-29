using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace OcUtility.Editor
{
    [CustomEditor(typeof(DistanceCalculator))]
    public class DistanceCalculatorEditor : OdinEditor
    {
        static DistanceCalculator _target;
        List<float> distances;
        [MenuItem("GameObject/거리 계산", priority = 50)]
        static void Initialize()
        {
            var point = new GameObject("거리계산", typeof(DistanceCalculator));
            point.hideFlags = HideFlags.DontSave;
            
            if(Selection.activeGameObject != null)
            {
                point.transform.position = Selection.activeGameObject.transform.position;
            }
            
            Selection.activeGameObject = point;
        }
        protected void Awake()
        {
            _target = target as DistanceCalculator;
            Selection.selectionChanged += OnSelectionChanged;
        }

        void OnSelectionChanged()
        {
            if (_target == null)
            {
                var t = FindObjectsOfType<DistanceCalculator>();
                foreach (var distanceCalculator in t)
                {
                    distanceCalculator.ClearPoints();
                    DestroyImmediate(distanceCalculator.gameObject);
                    Printer.Print("숨어있던 녀석을 잡음!");
                }
                Selection.selectionChanged -= OnSelectionChanged;
                return;
            }
            if(_target != null && Selection.activeGameObject == _target.gameObject)
            {
                return;
            }
            DestroyImmediate(_target.gameObject);
            Selection.selectionChanged -= OnSelectionChanged;
        }

        void OnDestroy()
        {
            _target.ClearPoints();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            EditorGUILayout.HelpBox(
                "Alt + LeftClick : 새 포인트 생성\n" +
                "Ctrl + RightClick : 마지막 포인트 위치 설정\n" +
                "* MacOS 에선 Ctrl대신 Command를 사용함.",
                MessageType.Info);
            
            var totalDistance = distances == null || distances.Count == 0 ? 0 : distances.Sum(x => x);
            var camDistance = SceneViewController.DistanceFromSceneViewCam(_target.points[0].position);
            EditorGUILayout.LabelField($"총 거리 : {totalDistance : 0.00}m");
            EditorGUILayout.LabelField($"마지막 지점과 카메라 까지의 거리 : {camDistance: 0.00}m");
        }

        void OnSceneGUI()
        {
            if(_target.points == null || _target.points.Count == 0) return;
            Handles.BeginGUI();
            var camDistance = SceneViewController.DistanceFromSceneViewCam(_target.points[0].position);
            var sceneViewSize = SceneView.lastActiveSceneView.position.size;
            var alignCenter = new GUIStyle
            {
                alignment = TextAnchor.MiddleCenter,
                normal =
                {
                    textColor = Color.white
                },
                fontSize = 20
            };
            var rect = new Rect(sceneViewSize / 2 + new Vector2(0, sceneViewSize.y * 0.4f), Vector2.one);
            GUI.Label(rect, $"마지막 지점과 카메라 까지의 거리 : {camDistance : 0.00}m", alignCenter);
            Handles.EndGUI();
            
            bool isControlDown = Application.platform == RuntimePlatform.OSXEditor ? Event.current.command : Event.current.control;

            var sceneViewCam = SceneViewController.CamTransform;
            if (Event.current.alt && Event.current.OnLeftClick(SceneView.lastActiveSceneView.position))
            {
                var result = SceneViewController.RaycastSceneViewAll(out var hits);
                if(result == 0) return;
                var highest = hits.GetMinElement(x => Vector3.SqrMagnitude(x.point - sceneViewCam.position), result);

                var newPoint = new GameObject($"p_{_target.points.Count}");
                newPoint.hideFlags = HideFlags.HideAndDontSave;
                newPoint.transform.position = highest.point;
                _target.points.Add(newPoint.transform);
                if (distances == null) distances = new List<float>();
                distances.Add(Vector3.Distance(_target.points[_target.points.Count - 2].position, _target.points[_target.points.Count - 1].position));
            }
            else if (isControlDown && Event.current.button == 1)
            {
                var result = SceneViewController.RaycastSceneViewAll(out var hits);
                if(result == 0) return;

                var highest = hits.GetMinElement(x => Vector3.SqrMagnitude(x.point - sceneViewCam.position), result);
                
                _target.points[_target.points.Count - 1].position = highest.point;
                
                if(distances != null && distances.Count > 0)
                {
                    distances[distances.Count - 1] = Vector3.Distance(_target.points[_target.points.Count - 2].position, _target.points[_target.points.Count - 1].position);
                    Repaint();
                }
            }
            
            
        }

        [DrawGizmo(GizmoType.Selected)]
        static void DrawGizmo(DistanceCalculator component, GizmoType gizmoType)
        {
            if(_target.points == null) return;
            for (int i = 0; i < _target.points.Count; i++)
            {
                var p = _target.points[i];
                Gizmos.color = Color.cyan.SetA(0.5f);
                Gizmos.DrawCube(p.position, Vector3.one * 0.2f);
                Handles.color = Color.red;
                Handles.DrawWireCube(p.position, Vector3.one * (SceneViewController.DistanceFromSceneViewCam(p.position) * 0.05f));
                Handles.Label(p.position, p.position.ToString());
                if(i < _target.points.Count - 1)
                {
                    var center = Vector3.Lerp(p.position, _target.points[i + 1].position, 0.5f);
                    Handles.Label(center, $"{Vector3.Distance(p.position, _target.points[i + 1].position) : 0.00}m");
                    Handles.DrawDottedLine(p.position, _target.points[i + 1].position, 10);
                }
            }
        }
    }
}
