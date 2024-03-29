using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace OcUtility.Editor
{
    public class ObjectPlaceUtility : OdinEditorWindow
    {
        public enum Shape
        {
            None,
            Line,
            Circle
        }

        public enum RotationType
        {
            None,
            Same,
            LookAt
        }

        public bool isActive;
        [ShowInInspector][HideLabel]string _count;
        [EnumToggleButtons, LabelWidth(100)]public Shape shape;

        [HorizontalGroup("line"), LabelWidth(100)] [HideIf(nameof(shape), Shape.Circle)]
        public Vector3 start;
        [HorizontalGroup("line"), LabelText("~"), LabelWidth(50)] [HideIf(nameof(shape), Shape.Circle)]
        public Vector3 end;
        
        

        [HorizontalGroup("circle"), LabelWidth(100)][ShowIf(nameof(shape), Shape.Circle)]
        public Vector3 center = new Vector3(0,0,0);
        
        [HorizontalGroup("circle"), LabelWidth(100)][ShowIf(nameof(shape), Shape.Circle)]
        public Vector3 axis = new Vector3(0,1,0);

        [HorizontalGroup("circle"), LabelWidth(100)] public float radius;

        [Space]
        [EnumToggleButtons, LabelWidth(100)] public RotationType rotationType;
        
        [LabelWidth(100)][ShowIf(nameof(rotationType), RotationType.Same)]
        public Vector3 rotation;

        [HorizontalGroup("lookAt")][LabelWidth(100)][ShowIf(nameof(rotationType), RotationType.LookAt)]
        public Vector3 lookAtPoint;

        [HorizontalGroup("lookAt")][LabelWidth(50), LabelText("Invert")] [ShowIf(nameof(rotationType), RotationType.LookAt)]
        public bool invertRotation;
        [LabelWidth(100)][ShowIf(nameof(rotationType), RotationType.LookAt)]
        public Vector3 lookAtAxis = new Vector3(0,1,0);

        public Vector3 rotationOffset;

        [InlineButton(nameof(Reseed))]public int seed;
        
        Vector3 CenterOfAll => Selection.count > 0 ? 
            Selection.transforms.Select(x => x.position).Sum() / Selection.count : Vector3.zero;

        Shape _lastShape;

        [HideInInspector]public List<GameObject> targets;
        [MenuItem("Utility/오브젝트 배치 유틸리티")]
        static void Open()
        {
            GetWindow<ObjectPlaceUtility>(true);
        }

        void Reseed()
        {
            seed = Random.Range(int.MinValue, int.MaxValue);
        }

        void Awake()
        {
            targets = new List<GameObject>();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            
            if(start == Vector3.zero)
            {
                start = Selection.activeTransform != null ? Selection.activeTransform.position : Vector3.zero;
                end = start + Vector3.forward;
                center = start;
                lookAtPoint = start + Vector3.right;
            }
            OnSelectionChange();
            SceneView.duringSceneGui += DrawHandles;
            Tools.hidden = true;
        }

        void OnDisable()
        {
            Tools.hidden = false;
            SceneView.duringSceneGui -= DrawHandles;
        }

        void OnSelectionChange()
        {
            targets = Selection.gameObjects.Where(x => x.gameObject.scene.IsValid()).OrderBy(x => x.name).ToList();
            _count = $"오브젝트 수 : {targets.Count}";
        }

        void DrawHandles(SceneView sceneView)
        {
            switch (shape)
            {
                case Shape.Line:
                    start = Handles.DoPositionHandle(start, Quaternion.identity);
                    Handles.Label(start, "start");
                    end = Handles.DoPositionHandle(end, Quaternion.identity);
                    Handles.Label(end, "end");
                    break;
                case Shape.Circle:
                    center = Handles.DoPositionHandle(center, Quaternion.identity);
                    Handles.Label(center, "center");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            switch (rotationType)
            {
                case RotationType.Same:
                    rotation = Handles.DoRotationHandle(Quaternion.Euler(rotation), CenterOfAll).eulerAngles;
                    Handles.ArrowHandleCap(0, CenterOfAll, Quaternion.Euler(rotation), 1f, EventType.Repaint);
                    break;
                case RotationType.LookAt:
                    lookAtPoint = Handles.DoPositionHandle(lookAtPoint, Quaternion.identity);
                    Handles.Label(lookAtPoint, "LookAtPoint");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        void Update()
        {
            if(!isActive) return;
            if(targets == null || targets.Count < 2) return;
            var isShapeChanged = shape != _lastShape;
            switch (shape)
            {
                case Shape.Line:
                    if (isShapeChanged) start = CenterOfAll;
                    for (int i = 0; i < targets.Count; i++)
                    {
                        targets[i].transform.position = Vector3.Lerp(start, end, (float) i / (targets.Count - 1));
                        SetRotation(targets[i].transform);
                    }
                    break;
                case Shape.Circle:
                    if (isShapeChanged) center = CenterOfAll;
                    for (int i = 0; i < targets.Count; i++)
                    {
                        var angle = (float)i / targets.Count * 360f;
                        var rot = Quaternion.AngleAxis(angle, axis);
                        var pos = center + rot * Vector3.forward * radius;
                        
                        targets[i].transform.position = pos;
                        SetRotation(targets[i].transform);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _lastShape = shape;
        }
        
        void SetRotation(Transform target)
        {
            switch (rotationType)
            {
                case RotationType.Same:
                {
                    target.rotation = Quaternion.Euler(rotation) * Quaternion.Euler(rotationOffset);
                    break;
                }
                case RotationType.LookAt:
                {
                    target.LookAt(lookAtPoint, lookAtAxis);
                    if(invertRotation) target.Rotate(lookAtAxis, 180);
                    target.rotation *=Quaternion.Euler(rotationOffset);
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
