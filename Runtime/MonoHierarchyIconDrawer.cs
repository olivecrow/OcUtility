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
        [ShowIf("iconTexture", null)][FilePath]public string iconPath = "EditorComment Icon";

        public int distanceToText = 15;
        public Color iconTint = Color.white;
#endif
    }
}
