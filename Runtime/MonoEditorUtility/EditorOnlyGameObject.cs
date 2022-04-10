using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace OcUtility
{
    [ExecuteInEditMode]
    public class EditorOnlyGameObject : MonoBehaviour
    {
        [ShowInInspector]
        public HideFlags HideFlags => hideFlags;

        void Reset()
        {
            ToEditorOnly();
        }
        
        [Button][HorizontalGroup()]
        void ToEditorOnly()
        {
            gameObject.hideFlags = HideFlags.DontSaveInBuild;
            Debug.Log($"[{name}] gameObject.hideFlags 변경 => {gameObject.hideFlags}");
        }

        [Button][HorizontalGroup()]
        void ToDefault()
        {
            gameObject.hideFlags = HideFlags.None;
            Debug.Log($"[{name}] gameObject.hideFlags 변경 => {gameObject.hideFlags}");
            DestroyImmediate(this);
        }
    }
}