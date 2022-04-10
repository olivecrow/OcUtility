using System.Linq;
using UnityEditor;
using UnityEngine;
using Application = UnityEngine.Application;

namespace OcUtility.Editor
{
    [InitializeOnLoad]
    public static class HierarchyIconDrawer
    {
        static HierarchyIconDrawer()
        {
            EditorApplication.hierarchyWindowItemOnGUI += DrawAllIcon;
            EditorApplication.quitting += Release;
        }

        static void Release()
        {
            EditorApplication.hierarchyWindowItemOnGUI -= DrawAllIcon;
            EditorApplication.quitting -= Release;
        }

        static void DrawAllIcon(int instanceID, Rect rect)
        {
            var gao = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
            if(gao == null) return;

            // 한 게임 오브젝트에 여러개의 Drawer가 있을 수 있음.
            var drawers = gao.GetComponents<IHierarchyIconDrawable>();
            var labelWidth = GUI.skin.label.CalcSize(new GUIContent(gao.name)).x;

            var inputColor = GUI.color;
            for (int i = 0; i < drawers.Length; i++)
            {
                var drawer = drawers[i];
#if UNITY_2021_1_OR_NEWER
                var icon = 
                drawer.OverrideIcon == null ? EditorGUIUtility.GetIconForObject(drawer.IconTarget) : drawer.OverrideIcon;
#else
                var icon =
                drawer.OverrideIcon == null ? 
                    EditorGUIUtility.ObjectContent(drawer.IconTarget, drawer.IconTarget.GetType()).image : 
                    drawer.OverrideIcon;
#endif
                if(icon == null) continue;
                var rectX = rect.position.x + 30 + labelWidth + i * 15;
                var iconDrawRect = new Rect(
                     rectX, rect.yMin,
                    15, 15);
                GUI.color = drawer.IconTint.a == 0 ? inputColor : drawer.IconTint;

                GUI.DrawTexture(iconDrawRect, icon);
                GUI.color = inputColor;
                
            }
        }

        
    }
}