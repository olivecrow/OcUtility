using System.IO;
using UnityEditor;
using UnityEngine;

namespace OcUtility.Editor
{
    public class OcEditorUtility
    {
        /// <summary>
        /// ScriptableObject를 정해진 위치에 생성함.
        /// 만약 폴더가 없으면 폴더도 생성하며, 파일 이름이 겹치면 접미사로 숫자를 추가함. 
        /// </summary>
        public static T CreateAsset<T>(T asset, string folderPath, string fileName) where T : ScriptableObject
        {
            if(!AssetDatabase.IsValidFolder(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var path = $"{folderPath}/{fileName}.asset";
            var uniquePath = AssetDatabase.GenerateUniqueAssetPath(path);
            asset.name = Path.GetFileName(uniquePath);
            AssetDatabase.CreateAsset(asset, uniquePath);

            return asset;
        }
    }
}