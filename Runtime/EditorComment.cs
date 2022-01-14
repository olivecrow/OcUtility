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
        const string FolderPath = "Assets/Editor Default Resources/Editor Comments";
        bool _renameAsset;

        [ShowIf("@asset == null")][Button]
        void CreateAsset()
        {
            var comment = ScriptableObject.CreateInstance<EditorCommentAsset>();
            comment.name = GetValidName("New Comment");
            
            if (!AssetDatabase.IsValidFolder(FolderPath))
            {
                AssetDatabase.CreateFolder("Assets/Editor Default Resources", "Editor Comments");
            }
            
            AssetDatabase.CreateAsset(comment, $"{FolderPath}/{comment.name}.asset");
            asset = comment;
            if (name == "GameObject")
            {
                Undo.RecordObject(gameObject, "Rename");
                name = comment.name;
            }
        }
        [Button, ShowIf("@asset != null && !_renameAsset")][HideIf(nameof(_renameAsset))]
        void Rename()
        {
            _renameAsset = true;
        }

        string GetValidName(string targetName)
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
        void OnDrawGizmos()
        {
            Gizmos.color = ColorExtension.Rainbow(5f).SetA(0.54f);

            var dist = Vector3.Distance(SceneView.lastActiveSceneView.camera.transform.position, transform.position);
            Gizmos.DrawSphere(transform.position, dist * 0.01f);
        }



    }
}
#endif