using System;
using Sirenix.OdinInspector;

#if UNITY_EDITOR
using UnityEditor;
#endif
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
                               "씬에 변경사항이 없다면 IProcessSceneWithReport가 호출되지 않음\n" +
                               "이 플래그를 사용할 일이 있다면, 빌드 전에 씬을 변경한 후 저장해야함.";
                    case MyHideFlags.UNITY_EDITOR:
                        return PrefabUtility.IsPartOfAnyPrefab(gameObject) ?
                            "프리팹의 일부인 경우, HideFlag가 적용되지 않음!!!!!":
                            "UNITY_EDITOR => 빌드 시 제외됨";
                    default:
                        return "";
                }
            }
        }  

        Color guiColor => gameObjectHideFlags == HideFlags.None ? 
            myHideFlags == MyHideFlags.DEBUG ? Color.yellow : Color.white : 
            Color.cyan;

        void Reset()
        {
            hideFlags = HideFlags.DontSaveInBuild;
        }

        void OnValueChanged()
        {
            Undo.RecordObject(gameObject, "change hide flag");
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
            var id = Undo.GetCurrentGroup();
            Undo.RecordObject(this, "change hide flag");
            myHideFlags = MyHideFlags.None;
            OnValueChanged();
            foreach (var other in Others)
            {
                other.myHideFlags = MyHideFlags.None;
                other.OnValueChanged();
            }
            Undo.CollapseUndoOperations(id);
        }
        
        [Button][HorizontalGroup()]
        void ToDebugOnly()
        {
            var id = Undo.GetCurrentGroup();
            Undo.RecordObject(this, "change hide flag");
            myHideFlags = MyHideFlags.DEBUG;
            OnValueChanged();
            foreach (var other in Others)
            {
                other.myHideFlags = MyHideFlags.DEBUG;
                other.OnValueChanged();
            }
            Undo.CollapseUndoOperations(id);
        }

        [Button][HorizontalGroup()]
        void ToEditorOnly()
        {
            var id = Undo.GetCurrentGroup();
            Undo.RecordObject(this, "change hide flag");
            myHideFlags = MyHideFlags.UNITY_EDITOR;
            OnValueChanged();
            foreach (var other in Others)
            {
                other.myHideFlags = MyHideFlags.UNITY_EDITOR;
                other.OnValueChanged();
            }
            Undo.CollapseUndoOperations(id);
        }
#endif



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