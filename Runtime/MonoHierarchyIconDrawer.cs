using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace OcUtility
{
    public class MonoHierarchyIconDrawer : MonoBehaviour
#if UNITY_EDITOR
        , IHierarchyIconDrawable
#endif
    {
#if UNITY_EDITOR
        public Texture2D IconTexture => iconTexture;
        public string IconPath => iconPath;
        public int DistanceToText => distanceToText;
        public Color IconTint => iconTint;

        public Texture2D iconTexture;
        [HideIf("iconTexture", null)][FilePath]public string iconPath = "EditorComment Icon";

        public int distanceToText = 250;
        public Color iconTint = Color.white;
#endif
    }
}
