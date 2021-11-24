using System.Linq;
using System.Text;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace OcUtility.Editor
{
    public class NamingUtility : OdinEditorWindow
    {
        [MenuItem("Utility/네이밍 유틸리티")]
        static void Open()
        {
            var wnd = GetWindow<NamingUtility>(true);
        }

        [Button]
        void Replace(string from, string to)
        {
            if (Selection.objects == null) return;
            Undo.RecordObjects(Selection.objects, "네이밍_Replace");
            foreach (var o in Selection.objects)
            {
                o.name = o.name.Replace(from, to);
                RenameIfMainAsset(o, o.name.Replace(from, to));
            }
        }

        [Button]
        int CalcLength(string s)
        {
            Printer.Print($"[네이밍 유틸리티] {s}.Length => {s.Length}");
            return s.Length;
        }

        [Button]
        void Insert(int start, string s)
        {
            if(Selection.objects == null) return;
            Undo.RecordObjects(Selection.objects, "네이밍_Insert");
            foreach (var o in Selection.objects)
            {
                o.name = o.name.Insert(start, s);
                RenameIfMainAsset(o, o.name.Insert(start, s));
            }
        }

        void RenameIfMainAsset(Object o, string s)
        {
            if(IsSceneAsset(o)) return;
            if(!AssetDatabase.IsMainAsset(o)) return;
            AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(o), s);
        }

        bool IsSceneAsset(Object o)
        {
            if (o is GameObject gao && gao.scene.IsValid()) return true;
            return false;
        }
        [Button("클론 넘버 없애기")]
        void RemoveNumbers()
        {
            Undo.RecordObjects(Selection.objects, "네이밍_클론 넘버 없애기");
            foreach (var o in Selection.objects)
            {
                if(!IsSceneAsset(o)) return;

                var split = o.name.Split();
                
                if(split.Length == 1) continue;
                var lastBlock = split[split.Length - 1];
                if(!lastBlock.StartsWith('(')) continue;
                if(!lastBlock.EndsWith(')')) continue;
                var number = lastBlock.Substring(1, lastBlock.Length - 2);
                if(!int.TryParse(number, out var num)) continue;
                
                var sb = new StringBuilder();
                for (int i = 0; i < split.Length - 1; i++)
                {
                    sb.Append(split[i]);
                }
                o.name = sb.ToString();
            }
        }
    }
}