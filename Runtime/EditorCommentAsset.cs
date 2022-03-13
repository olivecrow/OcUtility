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