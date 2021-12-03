using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace OcUtility.Editor
{
    public class ObjectPlaceUtility : OdinEditorWindow
    {
        public enum Shape
        {
            Line,
            Arc,
            Circle
        }

        public enum RotationType
        {
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

        [HideInInspector]public List<GameObject> targets;
        [HideInInspector]public int _undoID;
        [MenuItem("Utility/오브젝트 배치 유틸리티")]
        static void Open()
        {
            GetWindow<ObjectPlaceUtility>(true);
        }

        void Awake()
        {
            targets = new List<GameObject>();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            OnSelectionChange();
        }

        void OnSelectionChange()
        {
            targets = Selection.gameObjects.OrderBy(x => x.name).ToList();
            _count = $"오브젝트 수 : {targets.Count}";
        }
        void Update()
        {
            if(!isActive) return;
            if(targets == null || targets.Count < 2) return;
            switch (shape)
            {
                case Shape.Line:
                    for (int i = 0; i < targets.Count; i++)
                    {
                        Undo.RecordObject(targets[i].transform, "오브젝트 배치");
                        targets[i].transform.position = Vector3.Lerp(start, end, (float) i / (targets.Count - 1));
                        SetRotation(targets[i].transform);
                    }
                    break;
                case Shape.Arc:
                    for (int i = 0; i < targets.Count; i++)
                    {
                        Undo.RecordObject(targets[i].transform, "오브젝트 배치");
                        targets[i].transform.position = Vector3.Slerp(start, end, (float) i / (targets.Count - 1));
                        SetRotation(targets[i].transform);
                    }
                    break;
                case Shape.Circle:
                    for (int i = 0; i < targets.Count; i++)
                    {
                        Undo.RecordObject(targets[i].transform, "오브젝트 배치");
                        var angle = (float)i / targets.Count * 360f;
                        var rot = Quaternion.AngleAxis(angle, axis);
                        var pos = rot * Vector3.forward * radius;
                        
                        targets[i].transform.position = pos;
                        SetRotation(targets[i].transform);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        void SetRotation(Transform target)
        {
            switch (rotationType)
            {
                case RotationType.Same:
                {
                    target.rotation = Quaternion.Euler(rotation);
                    break;
                }
                case RotationType.LookAt:
                {
                    target.LookAt(lookAtPoint, lookAtAxis);
                    if(invertRotation) target.Rotate(lookAtAxis, 180);
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        [MenuItem("GameObject/루트 오브젝트를 가운데로")]
        static void RootToCenter()
        {
            if(Selection.activeGameObject == null) return;
            var root = Selection.activeGameObject.transform;
            var undoIndex = Undo.GetCurrentGroup();
            Undo.RecordObject(root, "루트 오브젝트를 가운데로");
            var posSum = Vector3.zero;
            var cachedPos = new List<Vector3>();
            for (int i = 0; i < root.childCount; i++)
            {
                posSum += root.GetChild(i).position;
                cachedPos.Add(root.GetChild(i).position);
            }

            var center = posSum / root.childCount;
            root.transform.position = center;
            for (int i = 0; i < root.childCount; i++)
            {
                Undo.RecordObject(root.GetChild(i), "루트 오브젝트를 가운데로");
                root.GetChild(i).position = cachedPos[i];
                Undo.CollapseUndoOperations(undoIndex);
            }
        }
        
    }
}
