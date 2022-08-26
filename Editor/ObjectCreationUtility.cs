using System;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace OcUtility.Editor
{
    public class ObjectCreationUtility : OdinEditorWindow
    {
        public enum BoundStyle
        {
            Bounds,
            Sphere,
            Circle
        }
        
        [MenuItem("Utility/오브젝트 생성기")]
        private static void ShowWindow()
        {
            var window = GetWindow<ObjectCreationUtility>();
            window.titleContent = new GUIContent("오브젝트 생성기");
            window.Show();
        }

        public bool prefabAssetOnly;
        [ShowIf(nameof(prefabAssetOnly))][AssetsOnly] public GameObject[] prefabs;
        [HideIf(nameof(prefabAssetOnly))] public GameObject[] targets;
        public int count = 100;
        
        [Space]
        [Header("Position")]
        public BoundStyle boundStyle;
        
        [ShowIf(nameof(boundStyle), BoundStyle.Bounds), Indent()]
        public Bounds bounds = new Bounds(Vector3.zero, new Vector3(100, 10, 100));
        
        [ShowIf(nameof(boundStyle), BoundStyle.Sphere), Indent()]
        public BoundingSphere boundingSphere = new BoundingSphere(Vector3.zero, 50);
        
        [ShowIf(nameof(boundStyle), BoundStyle.Circle), Indent()]
        public Vector3 circleCenter;
        [ShowIf(nameof(boundStyle), BoundStyle.Circle), Indent()]
        public float radius = 50;

        [Space]
        [Header("Rotation")]
        [MinMaxSlider(0, 360, true)]public Vector2 rotationXRange;
        [MinMaxSlider(0, 360, true)]public Vector2 rotationYRange = new Vector2(0, 360);
        [MinMaxSlider(0, 360, true)]public Vector2 rotationZRange;
        
        [Space]
        [Header("Scale")]
        [MinMaxSlider(0, 10, true)]public Vector2 scaleRange = Vector2.one;

        protected override void OnEnable()
        {
            base.OnEnable();
            SceneView.duringSceneGui += DrawGizmos;
        }

        void OnDisable()
        {
            SceneView.duringSceneGui -= DrawGizmos;
        }


        [Button]
        void Create()
        {
            var undoID = Undo.GetCurrentGroup();
            var folderName = prefabAssetOnly ? 
                string.Join(" | ", prefabs.Select(x => x.name)) : 
                string.Join(" | ", targets.Select(x => x.name));
            var folder = new GameObject($"{folderName} [{count}]").transform;

            Undo.RegisterCreatedObjectUndo(folder.gameObject, "Create Objects");
            Undo.RecordObject(folder, "instanced folder");
            
            for (int i = 0; i < count; i++)
            {
                var original = prefabAssetOnly ? prefabs[Random.Range(0, prefabs.Length)] : targets[Random.Range(0, targets.Length)];

                if (prefabAssetOnly)
                {
                    var gao = PrefabUtility.InstantiatePrefab(original, folder) as GameObject;
                    Undo.RegisterCreatedObjectUndo(gao, "Create Objects");
                    Undo.RecordObject(gao.transform, "Update Transform");
                    UpdateTransform(gao.transform);
                }
                else
                {
                    var gao = Instantiate(original, folder);
                    Undo.RegisterCreatedObjectUndo(gao, "Create Objects");
                    Undo.RecordObject(gao.transform, "Update Transform");
                    UpdateTransform(gao.transform);
                }
            }
            Undo.CollapseUndoOperations(undoID);
        }

        void DrawGizmos(SceneView sceneView)
        {
            switch (boundStyle)
            {
                case BoundStyle.Bounds:
                    Handles.DrawWireCube(bounds.center, bounds.size);
                    break;
                case BoundStyle.Sphere:
                    Handles.DrawWireDisc(boundingSphere.position, Vector3.up, boundingSphere.radius);
                    Handles.DrawWireDisc(boundingSphere.position, Vector3.right, boundingSphere.radius);
                    Handles.DrawWireDisc(boundingSphere.position, Vector3.forward, boundingSphere.radius);
                    break;
                case BoundStyle.Circle:
                    Handles.DrawWireDisc(circleCenter, Vector3.up, radius);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        void UpdateTransform(Transform t)
        {
            UpdatePosition(t);
            UpdateRotation(t);
            UpdateScale(t);
        }

        void UpdatePosition(Transform t)
        {
            switch (boundStyle)
            {
                case BoundStyle.Bounds:
                    var x = Random.Range(bounds.min.x, bounds.max.x);
                    var y = Random.Range(bounds.min.y, bounds.max.y);
                    var z = Random.Range(bounds.min.z, bounds.max.z);
                    t.position = new Vector3(x, y, z);
                    break;
                case BoundStyle.Sphere:
                    t.position = Random.insideUnitSphere * boundingSphere.radius + boundingSphere.position;
                    break;
                case BoundStyle.Circle:
                    t.position = (Random.insideUnitCircle * radius).ToXZ() + circleCenter;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        void UpdateRotation(Transform t)
        {
            t.rotation = Quaternion.Euler(rotationXRange.random(), rotationYRange.random(), rotationZRange.random());
        }

        void UpdateScale(Transform t)
        {
            t.localScale = Vector3.one * scaleRange.random();
        }
    }
}