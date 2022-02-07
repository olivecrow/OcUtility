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

        public bool hideGizmo = true;
        [HideIf(nameof(hideGizmo)), Indent()]public Color gizmoColor = Color.red;
        [HideIf(nameof(hideGizmo)), Indent()]public bool blinkGizmo;

        [ShowInInspector][ShowIf(nameof(_renameAsset)), DelayedProperty]
        public string nameInputField
        {
            get => asset == null ? name : asset.name;
            set
            {
                AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(asset), GetValidName(value));
                name = value;
                _renameAsset = false;
            }
        }
        bool _renameAsset;

        [ShowIf("@asset == null")][Button]
        void CreateAsset()
        {
            var assetName = name == "GameObject" ? "New Comment" : name;
            
            var baseFolder = "Assets/Editor Default Resources/Editor Comments";
            var folderPath = $"{baseFolder}/{gameObject.scene.name}";
            
            asset = CreateAsset(folderPath, assetName);

            if (asset.name != "GameObject")
            {
                name = asset.name;
            }
        }
        [Button, ShowIf("@asset != null && !_renameAsset")][HideIf(nameof(_renameAsset))]
        void Rename()
        {
            _renameAsset = true;
        }
        
        void OnDrawGizmos()
        {
            var color = blinkGizmo ? gizmoColor.SetA(Time.realtimeSinceStartup % 0.5f) : gizmoColor; 
            Gizmos.color = color;

            var dist = Vector3.Distance(SceneView.lastActiveSceneView.camera.transform.position, transform.position);
            Gizmos.DrawSphere(transform.position, dist * 0.01f);
        }


        // static-------
        static string GetValidName(string targetName)
        {
            for (int i = 0; true; i++)
            {
                var nextName = i == 0 ? targetName : $"{targetName} {i}";
                var exist = AssetDatabase.FindAssets($"t:EditorCommentAsset")
                    .Select(AssetDatabase.GUIDToAssetPath).Select(AssetDatabase.LoadAssetAtPath<EditorCommentAsset>);

                if (exist.All(x => x.name != nextName))
                {
                    return nextName;
                }
            }
        }
        
        public static EditorCommentAsset CreateAsset(string folderPath, string fileName)
        {
            var comment = ScriptableObject.CreateInstance<EditorCommentAsset>();
            comment.name = GetValidName(fileName);

            
            if (!AssetDatabase.IsValidFolder(folderPath))
            {
                System.IO.Directory.CreateDirectory(folderPath);
                AssetDatabase.ImportAsset(folderPath);
            }
            
            AssetDatabase.CreateAsset(comment, $"{folderPath}/{comment.name}.asset");

            return comment;
        }
    }
}
#endif