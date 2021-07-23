using UnityEditor;
using UnityEngine;

namespace OcUtility.Editor
{
    [InitializeOnLoad]
    public static class HierarchyIconDrawer
    {
        static HierarchyIconDrawer()
        {
            Application.quitting += Init;
            Init();
        }

        static void Init()
        {
            EditorApplication.hierarchyWindowItemOnGUI += DrawAllIcon;
        }

        static void DrawAllIcon(int instanceID, Rect rect)
        {
            var gao = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
            if(gao == null) return;
            var drawables = gao.GetComponents<IHierarchyIconDrawable>();
            if(drawables == null || drawables.Length == 0) return;
            
            for (int i = 0; i < drawables.Length; i++)
            {
                var iconGUIContent = new GUIContent(GetIcon(drawables[i], out var xRect, out var tint));
                var iconDrawRect = new Rect(
                    rect.width * 0.75f + xRect,
                    rect.yMin,
                    rect.width,
                    rect.height);
                EditorGUIUtility.SetIconSize(new Vector2(15, 15));
                GUI.contentColor = tint;
                EditorGUI.LabelField(iconDrawRect, iconGUIContent);
                GUI.contentColor = Color.white;
            }
        }

        static Texture2D GetIcon(IHierarchyIconDrawable drawable, out int xRect, out Color tint)
        {
            Texture2D icon;
            xRect = drawable.DistanceToText;
            tint = drawable.IconTint;

            if (drawable.IconTexture != null)
            {
                icon = drawable.IconTexture;
                return icon;
            }
            
            icon = (Texture2D) EditorGUIUtility.Load(drawable.IconPath);
            if (icon == null) icon = Resources.Load<Texture2D>(drawable.IconPath);
            if (icon == null) icon = AssetDatabase.LoadAssetAtPath<Texture2D>(drawable.IconPath);
            
            if (icon == null) return null;
            return icon;
        }
        
    }
}