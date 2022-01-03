#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEditor;
using UnityEditor.Presets;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace OcUtility
{
    public class EditorComment : MonoBehaviour
        , IHierarchyIconDrawable
    {
        public Object IconTarget => this;
        public Texture2D OverrideIcon => null;
        public Color IconTint { get; }
        public Context[] Contexts;

        static Dictionary<int, Preset> _playModeCopies;
        [HideInInspector]public int guid;
        bool _isDirty;
        bool _enteredPlaymode;
        [RuntimeInitializeOnLoadMethod]
        static void Init()
        {
            if(_playModeCopies == null)
                _playModeCopies = new Dictionary<int, Preset>();
            else _playModeCopies.Clear();
        }
        
        void Awake()
        {
            EditorApplication.playModeStateChanged += ApplyChanges;
        }

        void ApplyChanges(PlayModeStateChange change)
        {
            if(change != PlayModeStateChange.EnteredEditMode) return;
            if (_playModeCopies.ContainsKey(guid))
            {
                var editModeInstance = Resources
                    .FindObjectsOfTypeAll<EditorComment>().First(x => x.guid == guid);
                _playModeCopies[guid].ApplyTo(editModeInstance);
            }
            EditorApplication.playModeStateChanged -= ApplyChanges;
        }

        void OnValidate()
        {
            if(!Application.isPlaying) return;
            if (!_enteredPlaymode)
            {
                _enteredPlaymode = true;
                return;
            }
            _isDirty = true;
        }

        void OnDestroy()
        {
            if(_isDirty)
            {
                _playModeCopies[guid] = new Preset(this);
            }
        }

        void Reset()
        {
            guid = Random.Range(int.MinValue, int.MaxValue);
            Contexts = new Context[1];
            gameObject.hideFlags = HideFlags.DontSaveInBuild;
        }

        void OnDrawGizmos()
        {
            Gizmos.color = ColorExtension.Rainbow(5f).SetA(0.54f);

            var dist = Vector3.Distance(SceneView.lastActiveSceneView.camera.transform.position, transform.position);
            Gizmos.DrawSphere(transform.position, dist * 0.01f);
        }

        [Serializable]
        public class Context
        {
            [HideLabel] public string header;

            public enum ContextType
            {
                TextArea,
                CheckRow
            }

            [EnumToggleButtons, HideLabel] public ContextType Type;

            [TextArea(minLines: 5, maxLines: 20), HideLabel, ShowIf("Type", ContextType.TextArea)]
            public string content;

            [HideLabel, ShowIf("Type", ContextType.CheckRow), TableList]
            public CheckRow[] checkRow;

            [Serializable]
            public class CheckRow
            {
                [GUIColor("color")] public string text;
                [TableColumnWidth(25, false)]public bool V;

                Color color => V ? new Color(.5f, 1f, .5f) : Color.white;
            }
        }

    }
}
#endif