using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace OcUtility
{
    public class EditorComment : MonoBehaviour
#if UNITY_EDITOR
        , IHierarchyIconDrawable
#endif
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

            public enum ContextType
            {
                TextArea,
                CheckRow
            }

            [EnumToggleButtons, HideLabel] public ContextType Type;

            [TextArea(minLines: 5, maxLines: 20), HideLabel, ShowIf("Type", ContextType.TextArea)]
            public string content;

            [HideLabel, ShowIf("Type", ContextType.CheckRow), TableList]
            public CheckRow[] checkRow;

            [Serializable]
            public class CheckRow
            {
                [GUIColor("color")] public string text;
                [TableColumnWidth(25, false)]public bool V;

                Color color => V ? new Color(.5f, 1f, .5f) : Color.white;
            }
        }
#endif
    }
}