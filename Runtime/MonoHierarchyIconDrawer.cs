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
        public Object IconTarget => this;
        public Texture2D OverrideIcon => iconTexture;
        public Color IconTint => iconTint;

        public Texture2D iconTexture;
        
        public Color iconTint = Color.white;
#endif
    }
}
