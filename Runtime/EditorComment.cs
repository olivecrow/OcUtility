#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEditor;
using UnityEditor.Presets;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace OcUtility
{
    public class EditorComment : MonoBehaviour
        , IHierarchyIconDrawable
    {
        public Object IconTarget => this;
        public Texture2D OverrideIcon => null;
        public Color IconTint { get; }
        [InlineEditor(InlineEditorModes.FullEditor, Expanded = true, DrawHeader = false)]
        
        public EditorCommentAsset asset;

        const string FolderPath = "Assets/Editor Default Resources/Editor Comments";

        [DisableInPrefabs][ShowIf("@asset == null")][Button]
        void CreateAsset()
        {
            asset = EditorCommentAsset.CreateAsset($"{FolderPath}/{gameObject.scene.name}", name);
        }
        
        void OnDrawGizmos()
        {
            Gizmos.color = ColorExtension.Rainbow(5f).SetA(0.54f);

            var dist = Vector3.Distance(SceneView.lastActiveSceneView.camera.transform.position, transform.position);
            Gizmos.DrawSphere(transform.position, dist * 0.01f);
        }
    }
}
#endif