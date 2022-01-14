using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace OcUtility
{
    [CreateAssetMenu(fileName = "New Editor Comment", menuName = "Utility/Editor Comment", order = 0)]
    public class EditorCommentAsset : ScriptableObject
    {
        public Context[] Contexts;

        void Reset()
        {
            Contexts = new Context[1];
            hideFlags = HideFlags.DontSaveInBuild;
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