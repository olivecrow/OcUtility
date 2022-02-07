using System;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEditor.Experimental;
using UnityEngine;

namespace OcUtility.Editor
{
    public class EditorCommentWindow : OdinMenuEditorWindow
    {
        [MenuItem("Utility/Editor Comment Window")]
        static void Open()
        {
            var wnd = GetWindow<EditorCommentWindow>(true);
            wnd.minSize = new Vector2(700, 600);
            wnd.position = new Rect(500, 500, 700, 600);
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree();
            MenuWidth = 220;
            var assetGUIDs = AssetDatabase.FindAssets("t:EditorCommentAsset");
            for (int i = 0; i < assetGUIDs.Length; i++)
            {
                var path = AssetDatabase.GUIDToAssetPath(assetGUIDs[i]);
                var asset = AssetDatabase.LoadAssetAtPath<EditorCommentAsset>(path);

#if UNITY_2021_1_OR_NEWER
                tree.Add(path.Replace(".asset","").Replace("Assets/", ""), asset, EditorGUIUtility.GetIconForObject(asset));
#else
                tree.Add(path.Replace(".asset","").Replace("Assets/", ""), asset, EditorGUIUtility.ObjectContent(asset, asset.GetType()).image);
#endif
                
            }

            return tree;
        }

        protected override void OnBeginDrawEditors()
        {
            var selected = MenuTree.Selection.SelectedValue as EditorCommentAsset;
            SirenixEditorGUI.BeginHorizontalToolbar();
            if (SirenixEditorGUI.ToolbarButton("에셋 생성"))
            {
                var folderPath = selected == null ?
                    "Assets/Editor Default Resources/Editor Comments":
                    AssetDatabase.GetAssetPath(selected).Replace($"{selected.name}.asset", "");
                var asset = EditorComment.CreateAsset(folderPath, "New Comment");
                ForceMenuTreeRebuild();
            }

            if (selected == null) GUI.enabled = false;
            var bColor = GUI.backgroundColor;
            GUI.backgroundColor = Color.red;
            if (SirenixEditorGUI.ToolbarButton("에셋 삭제"))
            {
                if(!EditorUtility.DisplayDialog("에셋 삭제", $"정말 다음 에셋을 삭제하시겠습니까?\n{selected.name}", "삭제", "취소")) return;
                AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(selected));
                ForceMenuTreeRebuild();
            }
            GUI.backgroundColor = bColor;
            GUI.enabled = true;
            SirenixEditorGUI.EndHorizontalToolbar();
            base.OnBeginDrawEditors();
        }
    }
}