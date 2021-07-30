using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;

public class NamingUtility : OdinEditorWindow
{
    [MenuItem("Utility/네이밍 유틸리티")]
    static void Open()
    {
        var wnd = GetWindow<NamingUtility>();
        wnd.ShowUtility();
    }

    [Button]
    void Replace(string from, string to)
    {
        if(Selection.gameObjects == null) return;
        foreach (var gameObject in Selection.gameObjects)
        {
            gameObject.name = gameObject.name.Replace(from, to);
        }
    }
}