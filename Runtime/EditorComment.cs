#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEditor;

namespace OcUtility
{
    public class EditorComment : MonoBehaviour
        , IHierarchyIconDrawable
    {
        public Texture2D IconTexture => null;
        public string IconPath => "EditorComment Icon";
        public int DistanceToText => 250;
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
            Gizmos.color = ColorExtension.Rainbow(5f).SetA(0.54f);
            Gizmos.DrawSphere(transform.position, 0.1f * transform.localScale.magnitude);
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

    }
}
#endif