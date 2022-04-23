#if UNITY_EDITOR
using System;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace OcUtility
{
    [ExecuteInEditMode]
    public class EditorOnlyGameObject : MonoBehaviour
    {
        [BoxGroup("This GameObject")]public bool hideThisGameObject;
        [BoxGroup("This GameObject")][ShowInInspector][GUIColor("labelColor")]
        public HideFlags HideFlags => hideFlags;

        [BoxGroup("This GameObject")][ReadOnly]public HideFlags OriginalFlags;

        public EditorOnlyComponent[] Others;
        Color labelColor => HideFlags == HideFlags.DontSaveInBuild ? Color.cyan : Color.white;
        void Reset()
        {
            OriginalFlags = gameObject.hideFlags;
        }

        void OnDestroy()
        {
            if(Application.isPlaying) return;
            if(!gameObject.scene.isLoaded) return;
            ToDefault();
        }

        [Button][HorizontalGroup()]
        void ToEditorOnly()
        {
            if(hideThisGameObject) gameObject.hideFlags = HideFlags.DontSaveInBuild;
            foreach (var other in Others)
            {
                other.component.hideFlags = HideFlags.DontSaveInBuild;
            }
        }

        [Button][HorizontalGroup()]
        void ToDefault()
        {
            if(gameObject.hideFlags != OriginalFlags)
            {
                gameObject.hideFlags = OriginalFlags;
                Debug.Log($"[{name}] gameObejct hideFlags => {OriginalFlags}");
            }
            foreach (var other in Others)
            {
                if(other.component == null) continue;
                if(other.HideFlags == OriginalFlags) continue;
                other.component.hideFlags = other.OriginalFlags;
                Debug.Log($"[{other.component.name}] {other.component} hideFlags => {OriginalFlags}");
            }
        }

        [Serializable]
        public class EditorOnlyComponent
        {
            [ShowInInspector][PropertyOrder(-1)]
            public Component Component
            {
                get => component;
                set
                {
                    var isNew = component != value;
                    var before = component;
                    component = value;
                    if (isNew)
                    {
                        if (before != null) before.hideFlags = OriginalFlags;
                        if (component != null)OriginalFlags = component.hideFlags;
                    }
                }
            }
            [HideInInspector]public Component component;
            [GUIColor("labelColor")][ShowInInspector]public HideFlags HideFlags => Component == null ? HideFlags.None : Component.hideFlags;
            [ReadOnly]public HideFlags OriginalFlags;
            Color labelColor => HideFlags == HideFlags.DontSaveInBuild ? Color.cyan : Color.white;
        }
    }
}
#endif