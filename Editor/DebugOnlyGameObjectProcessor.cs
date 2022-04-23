using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace OcUtility.Editor
{
    public class DebugOnlyGameObjectProcessor : IProcessSceneWithReport
    {
        /*
         * DebugOnlyGameObject랑 EditorComment를 빌드에서 제외시킴
         * EditorComment는 DevelopmentBuild가 아니어도 제외하는데, EditorCommentAsset이 포함된 채로 빌드할 경우, 오류가 나기 때문.
         */
        public int callbackOrder { get; }

        public void OnProcessScene(Scene scene, BuildReport report)
        {
            if(report == null) return;
            
            HideEditorComment();
            if (!report.summary.options.Has(BuildOptions.Development)) return;
            SetFlagsDontSave();
        }

        void HideEditorComment()
        {
            var ECs = Object.FindObjectsOfType<EditorComment>();

            foreach (var ec in ECs)
            {
                Debug.Log($"Scene : {ec.gameObject.scene.name} | [{ec.name}] 의 EditorComment가 빌드에서 제외됨.");
                Object.DestroyImmediate(ec);
            }
        }

        void SetFlagsDontSave()
        {
            var objs = Object.FindObjectsOfType<DebugOnlyGameObject>();
            foreach (var obj in objs)
            {
                obj.gameObject.hideFlags = HideFlags.DontSaveInBuild;
                Debug.Log($"Scene : {obj.gameObject.scene.name} | Debug Only GameObject |[{obj.gameObject.name}]이 빌드에서 제외됨.");
            }
        }
        
    }
}