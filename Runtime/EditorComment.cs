using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace OcUtility
{
    public class EditorComment : MonoBehaviour, IHierarchyIconDrawable
    {
#if UNITY_EDITOR || DEBUG_BUILD
        public string IconPath => "EditorComment Icon";
        public int DistanceToText => -55;
        public Color IconTint => Color.white;
        [HideInInspector] GameObject gizmoTarget;
        public Context[] Contexts;

        void Reset()
        {
            Contexts = new Context[1];
            gameObject.hideFlags = HideFlags.DontSaveInBuild;
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = ColorExtension.Rainbow();
            Gizmos.DrawWireCube(transform.position, Vector3.one * 0.3f);
            Gizmos.DrawWireSphere(transform.position, 0.1f);
        }


        [Serializable]
        public class Context
        {
            [HideLabel] public string header;

            [TextArea(minLines: 5, maxLines: 20), HideLabel]
            public string content;
        }
#endif
    }
}