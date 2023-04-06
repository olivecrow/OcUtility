#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
namespace OcUtility
{
    [CreateAssetMenu(fileName = "New Editor Comment", menuName = "Utility/Editor Comment", order = 0)]
    public class EditorCommentAsset : ScriptableObject
    {
        [DelayedProperty][ShowInInspector][PropertyOrder(-1)]
        public string Name
        {
            get => name;
            set
            {
                name = value;
                AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(this), value);
            }
        }
        public Context[] Contexts;

        void Reset()
        {
            Contexts = new Context[1];
            hideFlags = HideFlags.DontSaveInBuild;
        }
        [Button("클립보드에 복사")]
        void Copy()
        {
            var sb = new StringBuilder();
            for (int i = 0; i < Contexts.Length; i++)
            {
                var context = Contexts[i];
                sb.Append($"{i + 1}.");
                sb.Append('\n');
                sb.Append(context.header);
                sb.Append('\n');
                sb.Append(context.content);
                
                if(i < Contexts.Length - 1)
                {
                    sb.Append('\n');
                    sb.Append('\n');
                }   
            }

            GUIUtility.systemCopyBuffer = sb.ToString();
        }

        public static EditorCommentAsset CreateAsset(string folderPath, string fileName)
        {
            if(!AssetDatabase.IsValidFolder(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var asset = CreateInstance<EditorCommentAsset>();
            var path = $"{folderPath}/{fileName}.asset";
            var uniquePath = AssetDatabase.GenerateUniqueAssetPath(path);
            asset.name = Path.GetFileName(uniquePath);
            AssetDatabase.CreateAsset(asset, uniquePath);

            return asset;
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

            [TextArea(5, 50), HideLabel, ShowIf("Type", ContextType.TextArea)]
            public string content;

            [HideLabel, ShowIf("Type", ContextType.CheckRow), TableList]
            public CheckRow[] checkRow;

            [Serializable]
            public class CheckRow
            {
                public enum State
                {
                    None,
                    Warning,
                    Error,
                    Clear
                }
                [TableColumnWidth(100, false)] public State state;
                [GUIColor("color")][TextArea(0,2)] public string text;
                [TextArea(0,2)]public string comment;

                Color color => state switch
                {
                    State.None => Color.white,
                    State.Warning => Color.yellow,
                    State.Error => Color.red,
                    State.Clear => Color.green,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
        }
    }
}
#endif