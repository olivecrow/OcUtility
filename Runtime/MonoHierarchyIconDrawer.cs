using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace OcUtility
{
    public class MonoHierarchyIconDrawer : MonoBehaviour, IHierarchyIconDrawable
    {
#if UNITY_EDITOR
        public string IconPath => iconPath;
        public int DistanceToText => distanceToText;
        public Color IconTint => iconTint;
        
        [FilePath]public string iconPath = "EditorComment Icon";
        public int distanceToText = 15;
        public Color iconTint = Color.white;
#endif
    }
}
