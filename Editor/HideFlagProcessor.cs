using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace OcUtility.Editor
{
    public class HideFlagProcessor : IProcessSceneWithReport
    {
        /*
         * HideFlagControl에서 제외된 컴포넌트랑 EditorComment를 빌드에서 제외시킴
         * EditorComment는 DevelopmentBuild가 아니어도 제외하는데, EditorCommentAsset이 포함된 채로 빌드할 경우, 오류가 나기 때문.
         */
        public int callbackOrder => 0;

        public void OnProcessScene(Scene scene, BuildReport report)
        {
            Debug.Log($"on process scene | scene : {scene} | report : {report}");
            if(report == null) return;
            
            var isDevelopmentBuild = report.summary.options.HasFlag(BuildOptions.Development);
            Debug.Log($"[HideFlagProcessor] | scene : {scene.name} | is Development build ? {isDevelopmentBuild}");
           
            HideEditorComment();
            HideDebugOnlyGameObjects(isDevelopmentBuild);
            
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

        void HideDebugOnlyGameObjects(bool isDevelopment)
        {
            if(isDevelopment) return;
            var objs = Object.FindObjectsOfType<HideFlagControl>();
            Debug.Log($"[HideFlagProcessor] 현재 씬에 존재하는 HideFlagControl 개수 : {objs.Length}");
            foreach (var obj in objs)
            {
                if (obj.myHideFlags == HideFlagControl.MyHideFlags.DEBUG)
                {
                    obj.gameObject.hideFlags = HideFlags.DontSaveInBuild;
                    Debug.Log($"Scene : {obj.gameObject.scene.name} | Debug Only GameObject |[{obj.gameObject.name}](이)가 빌드에서 제외됨.");
                }

                foreach (var component in obj.Others)
                {
                    if (component.myHideFlags == HideFlagControl.MyHideFlags.DEBUG)
                    {
                        component.component.hideFlags = HideFlags.DontSaveInBuild;
                        Debug.Log($"Scene : {component.component.gameObject.scene.name} | " +
                                  $"Debug Only GameObject |[{component.component.name}](이)가 빌드에서 제외됨.");
                    }
                }
                
            }
        }
        
    }
}