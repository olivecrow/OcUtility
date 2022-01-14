using Sirenix.OdinInspector.Editor;
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
                
                tree.Add(path.Replace(".asset",""), asset, EditorGUIUtility.GetIconForObject(asset));
            }

            return tree;
        }
    }
}