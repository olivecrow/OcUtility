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
            foreach (var o in Selection.objects)
            {
                o.name = o.name.Insert(start, s);
                RenameIfMainAsset(o, o.name.Insert(start, s));
            }
        }

        void RenameIfMainAsset(Object o, string s)
        {
            if(o is GameObject gao && gao.scene.IsValid()) return;
            if(!AssetDatabase.IsMainAsset(o)) return;
            AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(o), s);
        }
    }
}