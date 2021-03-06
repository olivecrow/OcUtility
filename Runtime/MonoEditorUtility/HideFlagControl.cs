using System;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace OcUtility
{
    public class HideFlagControl : MonoBehaviour
    {
        public enum MyHideFlags
        {
            None,
            DEBUG,
            UNITY_EDITOR
        }
        [ShowInInspector][GUIColor("guiColor")][PropertyOrder(-1)]
        public HideFlags gameObjectHideFlags => gameObject.hideFlags;
        [InfoBox("@infoMessage")]
        [OnValueChanged("OnValueChanged")]public MyHideFlags myHideFlags;
        public ComponentTarget[] Others;
#if UNITY_EDITOR
        string infoMessage
        {
            get
            {
                switch (myHideFlags)
                {
                    case MyHideFlags.None:
                        return "None => 아무것도 안함";
                    case MyHideFlags.DEBUG:
                        return "DEBUG => 개발 빌드에서는 포함되고, 릴리즈 빌드에서는 제외됨\nn" +
                               "!!! 무슨 원인인지, 작동이 안됨. 이 옵션으로 사용하지 말것.";
                    case MyHideFlags.UNITY_EDITOR:
                        return PrefabUtility.IsPartOfAnyPrefab(gameObject) ?
                            "프리팹의 일부인 경우, HideFlag가 적용되지 않음!!!!!":
                            "UNITY_EDITOR => 빌드 시 제외됨";
                    default:
                        return "";
                }
            }
        }  
#endif
        Color guiColor => gameObjectHideFlags == HideFlags.None ? 
            myHideFlags == MyHideFlags.DEBUG ? Color.yellow : Color.white : 
            Color.cyan;

        void Reset()
        {
            hideFlags = HideFlags.DontSaveInBuild;
        }

        void OnValueChanged()
        {
            switch (myHideFlags)
            {
                case MyHideFlags.None:
                    gameObject.hideFlags = HideFlags.None;
                    break;
                case MyHideFlags.DEBUG:
                    gameObject.hideFlags = HideFlags.None;
                    break;
                case MyHideFlags.UNITY_EDITOR:
                    gameObject.hideFlags = HideFlags.DontSaveInBuild;
                    break;
            }
        }
        
        void OnDestroy()
        {
            if(Application.isPlaying) return;
            if(!gameObject.scene.isLoaded) return;
            ToDefault();
        }
        [Button][HorizontalGroup()]
        void ToDefault()
        {
            
            myHideFlags = MyHideFlags.None;
            OnValueChanged();
            foreach (var other in Others)
            {
                other.myHideFlags = MyHideFlags.None;
                other.OnValueChanged();
            }
        }
        
        [Button][HorizontalGroup()]
        void ToDebugOnly()
        {
            myHideFlags = MyHideFlags.DEBUG;
            OnValueChanged();
            foreach (var other in Others)
            {
                other.myHideFlags = MyHideFlags.DEBUG;
                other.OnValueChanged();
            }
        }

        [Button][HorizontalGroup()]
        void ToEditorOnly()
        {
            myHideFlags = MyHideFlags.UNITY_EDITOR;
            OnValueChanged();
            foreach (var other in Others)
            {
                other.myHideFlags = MyHideFlags.UNITY_EDITOR;
                other.OnValueChanged();
            }
        }
        



        [Serializable]
        public class ComponentTarget
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
                        if (before != null) before.hideFlags = HideFlags.None;
                    }
                }
            }
            [HideInInspector]public Component component;
            [GUIColor("guiColor")][ShowInInspector]public HideFlags HideFlags => Component == null ? HideFlags.None : Component.hideFlags;
            [OnValueChanged("OnValueChanged")]public MyHideFlags myHideFlags;
            Color guiColor => HideFlags == HideFlags.None ?
                myHideFlags == MyHideFlags.DEBUG ? Color.yellow : Color.white 
                : Color.cyan;
            
            public void OnValueChanged()
            {
                switch (myHideFlags)
                {
                    case MyHideFlags.None:
                        component.hideFlags = HideFlags.None;
                        break;
                    case MyHideFlags.DEBUG:
                        component.hideFlags = HideFlags.None;
                        break;
                    case MyHideFlags.UNITY_EDITOR:
                        component.hideFlags = HideFlags.DontSaveInBuild;
                        break;
                }
            }
        }
    }
}