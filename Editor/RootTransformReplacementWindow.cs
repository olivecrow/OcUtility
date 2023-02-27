using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace OcUtility.Editor
{
    public class RootTransformReplacementWindow : OdinEditorWindow
    {
        public enum Method
        {
            ToOrigin,
            Position,
            Bounds
        }

        [Flags]
        public enum Options
        {
            None              = 0,
            Y_To_Bottom       = 1 << 0,
            OnlyFirstChildren = 1 << 1,
            
        }

        public enum RotationOption
        {
            None,
            Manual,
            Average
        }

        public bool applyPosition = true;
        public bool applyRotation = true;
        [EnumToggleButtons]public Method method;
        [EnumToggleButtons]public Options options;
        [EnumToggleButtons] public RotationOption rotation;
        [Range(1, 10)]public float gizmoSize = 5;
        [ReadOnly]public Transform selected;
        [ReadOnly]public Vector3 result;
        [ReadOnly] public Quaternion rotationResult = Quaternion.identity;
        [ShowIf(nameof(rotation), RotationOption.Manual)] public Vector3 manualRotation;

        Dictionary<Transform, Vector3> _positionCache;
        Dictionary<Transform, Quaternion> _rotationCache;
        IEnumerable<Transform> _children;

        [MenuItem("GameObject/루트 오브젝트 정렬", true, 50)]
        static bool IsSelectionExist()
        {
            var s = Selection.activeTransform;
            return s != null;
        }
        [MenuItem("GameObject/루트 오브젝트 정렬")]
        public static void ShowWindow()
        {
            var window = GetWindow<RootTransformReplacementWindow>(true);
            window.titleContent = new GUIContent("루트 오브젝트 정렬");
            window.minSize = new Vector2(560, 320);
            window.Show();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            selected = Selection.activeTransform;
            result = selected.position;
            SceneView.duringSceneGui += DrawGizmo;
        }

        void OnDisable()
        {
            SceneView.duringSceneGui -= DrawGizmo;
        }

        void OnValidate()
        {
            if(rotation == RotationOption.Manual) CalcRotation();
        }

        void DrawGizmo(SceneView sceneView)
        {
            var dist = Vector3.Distance(sceneView.camera.transform.position, result);
            var size = dist * 0.01f * gizmoSize;
            Handles.color = ColorExtension.Rainbow(10);
            Handles.DrawWireCube(result, Vector3.one * size);   
            Handles.DrawDottedLine(selected.position, result, 4f);

            var mat = Handles.matrix;
            Handles.matrix = Matrix4x4.TRS(result, Quaternion.identity, Vector3.one * 0.5f);
            Handles.PositionHandle(Vector3.zero, rotationResult);
            Handles.matrix = mat;
        }

        [Button]
        void Calculate()
        {
            switch (method)
            {
                case Method.ToOrigin:
                    if (applyPosition) result = Vector3.zero;
                    else result = selected.position;
                    break;
                case Method.Position:
                {
                    if (options.Has(Options.Y_To_Bottom))
                    {
                        if(applyPosition)result = GetPositionCenterBottom(GetChildren());
                        else result = selected.position;
                    }
                    else
                    {
                        if(applyPosition)result = GetPositionCenter(GetChildren());
                        else result = selected.position;
                    }
                    
                    break;
                }
                case Method.Bounds:
                {
                    var bounds = GetTotalBounds(GetRendererChildren());
                    if (options.Has(Options.Y_To_Bottom))
                    {
                        if(applyPosition) result = new Vector3(bounds.center.x, bounds.min.y, bounds.center.z);
                        else result = selected.position;
                    }
                    else
                    {
                        if(applyPosition) result = bounds.center;
                        else result = selected.position;
                    }
                    
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
            CalcRotation();
        }

        void CalcRotation()
        {
            switch (rotation)
            {
                case RotationOption.None:
                    if (applyRotation) rotationResult = Quaternion.identity;
                    else rotationResult = selected.rotation;
                    break;
                case RotationOption.Manual:
                    if (applyRotation) rotationResult = Quaternion.Euler(manualRotation);
                    else rotationResult = selected.rotation;
                    break;
                case RotationOption.Average:
                    Quaternion rot = selected.childCount == 0 ? selected.rotation : selected.GetChild(0).rotation;
                    for (int i = 0; i < selected.childCount; i++)
                    {
                        var child = selected.GetChild(i);
                        rot = Quaternion.Lerp(rot, child.transform.rotation, 0.5f);
                    }

                    if (applyRotation) rotationResult = rot;
                    else rotationResult = selected.rotation;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        [Button(ButtonSizes.Medium), GUIColor(2,2,0)]
        void Move()
        {
            Calculate();
            _positionCache = new Dictionary<Transform, Vector3>();
            _rotationCache = new Dictionary<Transform, Quaternion>();

            for (int i = 0; i < selected.childCount; i++)
            {
                var child = selected.GetChild(i);
                _positionCache[child] = child.transform.position;
                _rotationCache[child] = child.transform.rotation;
            }

            var allObj = new List<Object>();
            allObj.Add(selected);
            allObj.AddRange(_children);
            Undo.RecordObjects(allObj.ToArray(), "루트 오브젝트 정렬");
            selected.position = result;
            selected.rotation = rotationResult;

            foreach (var pos in _positionCache)
            {
                pos.Key.position = pos.Value;
            }

            foreach (var rot in _rotationCache)
            {
                rot.Key.rotation = rot.Value;
            }
        }

        IEnumerable<Transform> GetChildren()
        {
            if (options.Has(Options.OnlyFirstChildren))
            {
                var count = selected.childCount;
                var list = new List<Transform>();
                for (int i = 0; i < count; i++)
                {
                    list.Add(selected.GetChild(i));
                }

                _children = list;
                return _children;
            }
            _children = selected.GetComponentsInChildren<Transform>()
                .Where(x => x != selected);
            return _children;
        }

        IEnumerable<Renderer> GetRendererChildren()
        {
            if (options.Has(Options.OnlyFirstChildren))
            {
                var count = selected.childCount;
                var list = new List<Renderer>();
                for (int i = 0; i < count; i++)
                {
                    var c = selected.GetChild(i);
                    if(!c.TryGetComponent<Renderer>(out var r)) continue;
                    list.Add(r);
                }

                _children = list.Select(x => x.transform);
                return list;
            }
            else
            {
                var list = selected.GetComponentsInChildren<Renderer>()
                    .Where(x => x.transform != selected);
                _children = list.Select(x => x.transform);
                return list;
            } 
        }

        static Vector3 GetPositionCenter(IEnumerable<Transform> children)
        {
            return children.Sum(x => x.position) / children.Count();
        }
        
        static Vector3 GetPositionCenterBottom(IEnumerable<Transform> children)
        {
            var yBottom = children.Min(x => x.transform.position.y);
            var center = GetPositionCenter(children);
            return new Vector3(center.x, yBottom, center.z);
        }

        static Bounds GetTotalBounds(IEnumerable<Renderer> children)
        {
            var bounds = children.ElementAt(0).bounds;

            foreach (var child in children)
            {
                bounds.Encapsulate(child.bounds);
            }

            return bounds;
        }
    }
}