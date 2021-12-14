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

            // 한 게임 오브젝트에 여러개의 Drawer가 있을 수 있음.
            var drawers = gao.GetComponents<IHierarchyIconDrawable>();
            var labelWidth = rect.x + 15 + (gao.name.Length) * 8.2f; 
            for (int i = 0; i < drawers.Length; i++)
            {
                var drawer = drawers[i];
                
                var iconDrawRect = new Rect(
                     labelWidth + i * 15,
                    rect.yMin,
                    15,
                    15);
                GUI.color = drawer.IconTint.a == 0 ? Color.white : drawer.IconTint;
                GUI.DrawTexture(
                    iconDrawRect,
#if UNITY_2021_1_OR_NEWER
                    drawer.OverrideIcon == null ? EditorGUIUtility.GetIconForObject(drawer.IconTarget) : drawer.OverrideIcon);
#else
                    drawer.OverrideIcon == null ? 
                        EditorGUIUtility.ObjectContent(drawer.IconTarget, drawer.IconTarget.GetType()).image : 
                        drawer.OverrideIcon);
#endif
                    
                GUI.color = Color.white;
            }
        }

        
    }
}